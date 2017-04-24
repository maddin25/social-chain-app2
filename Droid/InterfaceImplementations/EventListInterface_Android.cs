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

			ICursor cursor = db.Query(EventTable.INSTANCE.TableName, null, null, null, null, null, null);
			Dictionary<string, int> columnIndexMapping = GetColumnMappings(cursor, EventTable.INSTANCE.Columns);
			ExecuteCustomTransaction(new Command(() =>
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
			}));

			SDebug.WriteLine($"Retrieved {events.Count} events from the local database");
			return events;
		}

		public void WriteLocalEvent(Event eventReference)
		{
			ExecuteSimpleTransaction(EventTable.INSTANCE.Insert(eventReference));
		}

		public List<EventImage> ReadLocalEventImages(Event eventReference)
		{
			List<EventImage> images = new List<EventImage>();
			ICursor cursor = db.Query(
				ImageTable.INSTANCE.TableName,
				null,
				$"{ImageTable.INSTANCE.ColumnEventId} = {eventReference.Id}",
				null, null, null, null);
			Dictionary<string, int> columnIndexMapping = GetColumnMappings(cursor, ImageTable.INSTANCE.Columns);
			ExecuteCustomTransaction(new Command(() =>
			{
				cursor.MoveToFirst();
				while (!cursor.IsAfterLast)
				{
					EventImage image = new EventImage
					{
						Id = cursor.GetLong(columnIndexMapping[ImageTable.INSTANCE.ColumnId]),
						URI = cursor.GetString(columnIndexMapping[ImageTable.INSTANCE.ColumnUri]),
						Caption = cursor.GetString(columnIndexMapping[ImageTable.INSTANCE.ColumnCaption]),
						DateCreated = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[ImageTable.INSTANCE.ColumnDateCreated])),
						DateLastModified = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[ImageTable.INSTANCE.ColumnLastModified]))
					};
					images.Add(image);
					SDebug.Assert(cursor.MoveToNext(), "failed moving to the next row");
				}
			}));
			SDebug.WriteLine($"Retrieved {images.Count} images from the local database");
			return images;
		}

		public void WriteLocalEventImage(EventImage image, Event eventReference)
		{
			ExecuteSimpleTransaction(ImageTable.INSTANCE.Insert(image, defaultEventMember, eventReference));
		}

		private void ExecuteSimpleTransaction(string query)
		{
			ExecuteCustomTransaction(new Command(() => db.ExecSQL(query)));
		}

		private void ExecuteCustomTransaction(Command customCommand)
		{
			db.BeginTransaction();
			try
			{
				customCommand.Execute(null);
				db.SetTransactionSuccessful();
			}
			catch (Exception e)
			{
				Application.Current.MainPage.DisplayAlert(AlertDatabaseAccessFailed, e.Message, "Ok");
			}
			finally
			{
				db.EndTransaction();
			}
		}

		private Dictionary<string, int> GetColumnMappings(ICursor cursor, List<Column> columns)
		{
			Dictionary<string, int> columnIndexMapping = new Dictionary<string, int>(columns.Count);
			foreach (Column column in columns)
			{
				columnIndexMapping[column.Name] = cursor.GetColumnIndex(column.Name);
			}
			return columnIndexMapping;
		}
	}
}
