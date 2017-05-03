using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class LocalDatabaseAccess
	{
		private static readonly string databaseDirectory = Path.Combine("");
		private static readonly string databaseFilename = "PartyTimeline.sqlite3";
		// TODO: remove this property in realease builds and implement migration behavior
		private readonly bool dropTables = true;
		private SQLiteConnection dbConnection;

		public LocalDatabaseAccess()
		{
			string applicationFolder = DependencyService.Get<SystemInterface>().GetApplicationDataFolder();
			string dbPath = Path.Combine(applicationFolder, databaseFilename);
			Debug.WriteLine($"Connecting to local SQLite database '{dbPath}'");
			dbConnection = new SQLiteConnection(dbPath);
			Init();
		}

		public List<Event> ReadEvents()
		{
			var cursor = dbConnection.Table<Event>();
			// TODO: modify table query such as to look only events where the current user is an active member
			List<Event> events = new List<Event>();
			foreach (var eventReference in cursor)
			{
				events.Add(eventReference);
				Debug.WriteLine(eventReference);
			}
			return events;
		}

		public List<EventImage> ReadEventImages(Event eventReference)
		{
			var cursor = from image in dbConnection.Table<EventImage>()
						 where image.EventId == eventReference.Id
						 select image;
			List<EventImage> eventImages = new List<EventImage>();
			foreach (EventImage image in cursor)
			{
				eventImages.Add(image);
			}
			return eventImages;
		}

		public void WriteEvent(Event eventReference)
		{
			dbConnection.Insert(eventReference);
		}

		public void WriteEventImage(EventImage eventImage, Event eventReference)
		{
			eventImage.EventId = eventReference.Id;
			eventImage.EventMemberId = SessionInformation.INSTANCE.UserId;
			dbConnection.Insert(eventImage);
		}

		public void WriteEventMember(EventMember member)
		{
			EventMember dbMember = null;
			try
			{
				dbMember = dbConnection.Get<EventMember>(member.Id);
			}
			catch { }
			if (dbMember == null)
			{
				dbConnection.Insert(member);
			}
			else
			{
				// TODO: implement update query
				Debug.WriteLine($"EventMember {member.Name} (id={member.Id}) already exists in the database");
			}
		}

		public void RemoveEvent(Event eventReference)
		{
			var cursor = from image in dbConnection.Table<EventImage>()
						 where image.EventId == eventReference.Id
						 select image;
			foreach (EventImage image in cursor)
			{
				dbConnection.Delete<EventImage>(image.Id);
			}
			dbConnection.Delete<Event>(eventReference.Id);
		}

		public long RemoveEventImage(EventImage image)
		{
			// TODO: this Get method can throw a NotFoundException, surround with try - catch block
			Event eventReference = dbConnection.Get<Event>(image.EventId);
			dbConnection.Delete<EventImage>(image.Id);
			return eventReference != null ? eventReference.Id : -1;
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
			dbConnection.DropTable<Event>();
			dbConnection.DropTable<EventMember>();
			dbConnection.DropTable<Event_EventMember>();
			dbConnection.DropTable<EventImage>();
		}

		private void CreateAllTables()
		{
			dbConnection.CreateTable<Event>();
			dbConnection.CreateTable<EventMember>();
			dbConnection.CreateTable<Event_EventMember>();
			dbConnection.CreateTable<EventImage>();
		}
	}
}
