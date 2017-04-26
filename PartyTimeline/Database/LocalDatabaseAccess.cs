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
		private static readonly string databaseDirectory = Path.Combine("..", "..", "databases");
		private static readonly string databaseFilename = "PartyTimeline.sqlite3";
		private SQLiteConnection dbConnection;

		public LocalDatabaseAccess()
		{
			string applicationFolder = DependencyService.Get<SystemInterface>().GetApplicationDataFolder();
			string dbPath = Path.Combine(applicationFolder, databaseDirectory, databaseFilename);
			Debug.WriteLine($"Connecting to local SQLite database '{dbPath}'");
			dbConnection = new SQLiteConnection(dbPath);
			dbConnection.CreateTable<Event>();
			dbConnection.CreateTable<EventMember>();
			dbConnection.CreateTable<Event_EventMember>();
			dbConnection.CreateTable<EventImage>();
			InsertMyself();
		}

		public void AddEvent(Event eventReference)
		{
			dbConnection.Insert(eventReference);
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

		public void WriteEventImage(EventImage eventImage, Event eventReference)
		{
			eventImage.EventId = eventReference.Id;
			eventImage.EventMemberId = SessionInformation.INSTANCE.CurrentUser.Id;
			dbConnection.Insert(eventImage);
		}

		public void WriteEvent(Event eventReference)
		{
			dbConnection.Insert(eventReference);
		}

		private void InsertMyself()
		{
			EventMember currentUser = SessionInformation.INSTANCE.CurrentUser;
			EventMember existingUser = dbConnection.Find<EventMember>(currentUser.Id);
			if (existingUser == null)
			{
				dbConnection.Insert(currentUser);
			}
		}
	}
}
