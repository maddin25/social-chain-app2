using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class LocalDatabaseAccess
	{
		private static readonly string databaseFilename = "PartyTimeline.sqlite3";
		// TODO: remove this property in realease builds and implement migration behavior
		private readonly bool dropTables = false;
		private SQLiteAsyncConnection dbConnection;

		public LocalDatabaseAccess()
		{
			string applicationFolder = DependencyService.Get<SystemInterface>().GetApplicationDataFolder();
			string dbPath = Path.Combine(applicationFolder, databaseFilename);
			Debug.WriteLine($"Connecting to local SQLite database '{dbPath}'");
			dbConnection = new SQLiteAsyncConnection(dbPath);
			Init();
		}

		public async Task<List<Event>> ReadEvents()
		{
			List<Event> events = new List<Event>();
			long currentUserId = SessionInformationProvider.INSTANCE.CurrentUserEventMember.Id;
			List<Event_EventMember> userEventMemberships = await dbConnection
				.Table<Event_EventMember>()
				.Where((Event_EventMember eem) => eem.EventMemberId == currentUserId)
				.ToListAsync();
			Debug.WriteLine($"{nameof(LocalDatabaseAccess)}: Found {userEventMemberships.Count} events for the relevant user.");
			if (userEventMemberships.Count == 0)
			{
				return events;
			}

			DateTime EventStartTimeThreshold = DateTime.Now.Subtract(EventService.LimitEventsInPast);
			await Task.WhenAll(userEventMemberships.Select(async (Event_EventMember arg) =>
			{
				Event e = await dbConnection.GetAsync<Event>((Event ie) => ie.Id == arg.EventId);
				if (e.StartDateTime > EventStartTimeThreshold)
				{
					Debug.WriteLine($"{nameof(ReadEvents)}: {e.ToString()}");
					lock (events)
					{
						events.Add(e);
					}
				}
			}));

			Debug.WriteLine($"{nameof(LocalDatabaseAccess)}: Found {events.Count} events in the relevant time frame");

			return events;
		}

		public async Task<List<EventImage>> ReadEventImages(Event eventReference)
		{
			List<EventImage> eventImages = await dbConnection
				.Table<EventImage>()
				.Where((EventImage image) => image.EventId == eventReference.Id)
				.ToListAsync();
			
			return eventImages;
		}

		public void AssociateEventMemberWithEvent(Event e, EventMember em, EventMembershipRoles.ROLES role)
		{
			var entry = new Event_EventMember
			{
				EventId = e.Id,
				EventMemberId = em.Id,
				Role = EventMembershipRoles.RoleId(role)
			};
			dbConnection.InsertOrReplaceAsync(entry);
		}

		public void WriteEvent(Event eventReference)
		{
			dbConnection.InsertAsync(eventReference);
		}

		public void WriteEventImage(EventImage eventImage)
		{
			// TODO: verify that all properties are set
			dbConnection.InsertAsync(eventImage);
		}

		public void UpdateEvent(Event e)
		{
			dbConnection.InsertOrReplaceAsync(e);
		}

		public void WriteEventMember(EventMember member)
		{
			dbConnection.InsertOrReplaceAsync(member);
		}

		public async Task RemoveEvent(Event e)
		{
			List<EventImage> eventImages = await dbConnection.Table<EventImage>()
															 .Where((image) => image.EventId == e.Id)
															 .ToListAsync();
			Task.WhenAll(eventImages.Select((EventImage image) => dbConnection.DeleteAsync(image)));
			dbConnection.DeleteAsync(e);
		}

		public async Task<long> RemoveEventImage(EventImage image)
		{
			// TODO: this Get method can throw a NotFoundException, surround with try - catch block
			Event e = await dbConnection.FindAsync<Event>((Event de) => de.Id == image.EventId);
			dbConnection.DeleteAsync(image);
			return e?.Id ?? -1;
		}

		private void Init()
		{
			if (dropTables)
			{
				DropAllTables();
			}
			CreateAllTables();
		}

		private void DropAllTables()
		{
			dbConnection.DropTableAsync<Event>();
			dbConnection.DropTableAsync<EventMember>();
			dbConnection.DropTableAsync<Event_EventMember>();
			dbConnection.DropTableAsync<EventImage>();
		}

		private void CreateAllTables()
		{
			try
			{
				dbConnection.CreateTablesAsync<Event, EventMember, Event_EventMember, EventImage>();
			}
			catch (Exception e)
			{
				Debug.WriteLine($"Failed creating tables: {e.Message}\nTrying again.");
				DropAllTables();
				CreateAllTables();
			}
		}
	}
}
