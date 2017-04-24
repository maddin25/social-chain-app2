using System;
using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Android.Database;
using Android.Database.Sqlite;

using Newtonsoft.Json;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.EventListInterface_Android))]
namespace PartyTimeline.Droid
{
	public class EventListInterface_Android : EventListInterface
	{
		private readonly string AlertDatabaseAccessFailed = "Database access failed";

		private SQLiteDatabase db;
		private PartyTimelineDatabase dbHelper;

		private EventMember defaultEventMember;

		public EventListInterface_Android()
		{
			dbHelper = new PartyTimelineDatabase(Android.App.Application.Context);
			db = dbHelper.WritableDatabase;

			defaultEventMember = new EventMember(DateTime.Now)
			{
				Id = 0,
				EmailAddress = "mailto@martin-patz.de",
				FirstName = "Martin",
				LastName = "Patz",
				Role = EventMember.RolesIds[ROLES.Administrator]
			};
		}

		public List<Event> PollServerEventList()
		{
			return null;
		}

		public void PushServerEvent(Event eventReference)
		{
			string serializedEvent = JsonConvert.SerializeObject(eventReference);
			SDebug.WriteLine($"Serialized event: {serializedEvent}");
		}

		public List<Event> ReadLocalEvents()
		{
			List<Event> events = new List<Event>();
			Dictionary<string, int> columnIndexMapping = new Dictionary<string, int>();

			db.BeginTransaction();
			ICursor cursor = db.Query(EventTable.INSTANCE.TableName, null, null, null, null, null, null);
			foreach (Column column in EventTable.INSTANCE.Columns)
			{
				columnIndexMapping[column.Name] = cursor.GetColumnIndex(column.Name);
			}

			try
			{
				cursor.MoveToFirst();
				while (!cursor.IsAfterLast)
				{
					Event e = new Event();
					e.Id = cursor.GetLong(columnIndexMapping[EventTable.INSTANCE.ColumnId]);
					e.Name = cursor.GetString(columnIndexMapping[EventTable.INSTANCE.ColumnEventName]);
					e.Description = cursor.GetString(columnIndexMapping[EventTable.INSTANCE.ColumnEventDescription]);
					e.DateCreated = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[EventTable.INSTANCE.ColumnDateCreated]));
					e.DateLastModified = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[EventTable.INSTANCE.ColumnLastModified]));
					events.Add(e);
					SDebug.Assert(cursor.MoveToNext(), "failed moving to the next row");
				}
				db.SetTransactionSuccessful();
			}
			catch (Exception e)
			{
				Application.Current.MainPage.DisplayAlert(AlertDatabaseAccessFailed, e.Message, "Ok");
			}
			db.EndTransaction();
			SDebug.WriteLine($"Retrieved {events.Count} events from the local database");
			return events;
		}

		public void WriteLocalEvent(Event eventReference)
		{
			ExecuteSimpleTransaction(EventTable.INSTANCE.Insert(eventReference));
		}

		public void WriteLocalEventImage(EventImage image, Event eventReference)
		{
			ExecuteSimpleTransaction(ImageTable.INSTANCE.Insert(image, defaultEventMember, eventReference));            
		}

		private void ExecuteSimpleTransaction(string query)
		{
			db.BeginTransaction();
			try
			{
				db.ExecSQL(query);
				db.SetTransactionSuccessful();
			}
			catch (Exception e)
			{
				Application.Current.MainPage.DisplayAlert(AlertDatabaseAccessFailed, e.Message, "Ok");
			}
			db.EndTransaction();
		}
	}
}
