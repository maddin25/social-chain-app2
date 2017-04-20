using System.Collections.Generic;

using Android.Content;
using Android.Database;
using Android.Database.Sqlite;

namespace PartyTimeline.Droid
{
	public class EventDatabase : SQLiteOpenHelper
	{
		private static string DATABASE_NAME = "PartyTimeline.db";
		private static int DATABASE_VERSION = 1;
		private List<TableTemplate> DATABASE_TABLES;

		public EventDatabase(Context context)
			: base(context, DATABASE_NAME, null, DATABASE_VERSION)
		{
			DATABASE_TABLES.Add(new EventTable());
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			foreach (TableTemplate tableTemplate in DATABASE_TABLES)
			{
				// TODO: create table here
			}
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{

		}
	}
}
