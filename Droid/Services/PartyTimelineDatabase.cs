using System;
using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

using Xamarin.Forms;

namespace PartyTimeline.Droid
{
	public class PartyTimelineDatabase : SQLiteOpenHelper
	{
		private readonly string AlertDatabaseAccessFailed = "Database access failed";

		private static int DatabaseVersion = 1;
		private List<TableTemplate> Tables = new List<TableTemplate>();

		private readonly EventTable eventTable = new EventTable();
		private readonly EventMemberTable eventMemberTable = new EventMemberTable();
		private readonly ImageTable imageTable = new ImageTable();
		private readonly Event_EventMember_Table eventEventMemberTable = new Event_EventMember_Table();

		public PartyTimelineDatabase(Context context)
			: base(context, "PartyTimeline.db", null, DatabaseVersion)
		{
			SDebug.WriteLine("Adding table representations to internal list");
			Tables.Add(eventTable);
			Tables.Add(eventMemberTable);
			Tables.Add(imageTable);
			Tables.Add(eventEventMemberTable);
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			SDebug.WriteLine($"Creating tables for database {DatabaseName}");
			foreach (TableTemplate tableTemplate in Tables)
			{
				string query = tableTemplate.CreateTableQuery();
				db.ExecSQL(query);
			}
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			SDebug.WriteLine($"Upgrading database {DatabaseName}");
			OnClear(db);
			OnCreate(db);
		}

		public void OnClear(SQLiteDatabase db)
		{
			SDebug.WriteLine($"Clearing database {DatabaseName}");
			foreach (TableTemplate tableTemplate in Tables)
			{
				string query = tableTemplate.DropTableQuery();
				db.ExecSQL(query);
			}
		}

		public List<Event> ReadLocalEvents(SQLiteDatabase db)
		{
			List<Event> events = new List<Event>();
			Dictionary<string, int> columnIndexMapping = new Dictionary<string, int>();

			db.BeginTransaction();
			ICursor cursor = db.Query(eventTable.TableName, null, null, null, null, null, null);
			foreach (Column column in eventTable.Columns)
			{
				columnIndexMapping[column.Name] = cursor.GetColumnIndex(column.Name);
			}

			try
			{
				cursor.MoveToFirst();
				while (!cursor.IsAfterLast)
				{
					Event e = new Event();
					e.Id = cursor.GetLong(columnIndexMapping[eventTable.ColumnId]);
					e.Name = cursor.GetString(columnIndexMapping[eventTable.ColumnEventName]);
					e.Description = cursor.GetString(columnIndexMapping[eventTable.ColumnEventDescription]);
					e.DateCreated = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[eventTable.ColumnDateCreated]));
					e.DateLastModified = DateTime.FromFileTime(cursor.GetLong(columnIndexMapping[eventTable.ColumnLastModified]));
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

		public void WriteLocalEvent(SQLiteDatabase db, Event eventReference)
		{
			db.BeginTransaction();
			try
			{
				db.ExecSQL(eventTable.Insert(eventReference));
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
