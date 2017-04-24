using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Android.Database.Sqlite;

using Newtonsoft.Json;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.EventListInterface_Android))]
namespace PartyTimeline.Droid
{
	public class EventListInterface_Android : EventListInterface
	{
		private SQLiteDatabase db;
		private PartyTimelineDatabase dbHelper;

		public EventListInterface_Android()
		{
			dbHelper = new PartyTimelineDatabase(Android.App.Application.Context);
			db = dbHelper.WritableDatabase;
		}

		public List<Event> ReadLocalEvents()
		{
			return dbHelper.ReadLocalEvents(db);
		}

		public List<Event> PollServerEventList()
		{
			return null;
		}

		public void WriteLocalEvent(Event eventReference)
		{
			dbHelper.WriteLocalEvent(db, eventReference);
		}

		public void PushServerEvent(Event eventReference)
		{
			string serializedEvent = JsonConvert.SerializeObject(eventReference);
			SDebug.WriteLine($"Serialized event: {serializedEvent}");
		}
	}
}
