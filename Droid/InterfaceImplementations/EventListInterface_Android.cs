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
		private SQLiteOpenHelper dbHelper;

		private EventTable eventTable = new EventTable();
		private readonly EventMemberTable eventMemberTable = new EventMemberTable();
		private readonly ImageTable imageTable = new ImageTable();
		private readonly Event_EventMember_Table eventEventMemberTable = new Event_EventMember_Table();

		public EventListInterface_Android()
		{
			dbHelper = new EventDatabase(Android.App.Application.Context);
			db = dbHelper.WritableDatabase;
		}

		public List<Event> ReadLocalEvents()
		{
			return null;
		}

		public List<Event> PollServerEventList()
		{
			return null;
		}

		public void WriteLocalEvent(ref Event eventReference)
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

		public void PushServerEvent(ref Event eventReference)
		{
			string serializedEvent = JsonConvert.SerializeObject(eventReference);
			SDebug.WriteLine($"Serialized event: {serializedEvent}");
		}
	}
}
