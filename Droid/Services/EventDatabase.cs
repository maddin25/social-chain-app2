using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Android.Content;
using Android.Database.Sqlite;

namespace PartyTimeline.Droid
{
	public class EventDatabase : SQLiteOpenHelper
	{
		private static string DATABASE_NAME = "PartyTimeline.db";
		private static int DATABASE_VERSION = 1;
		private List<TableTemplate> DATABASE_TABLES = new List<TableTemplate>();

		public EventDatabase(Context context)
			: base(context, DATABASE_NAME, null, DATABASE_VERSION)
		{
			SDebug.WriteLine("Adding table representations to internal list");
			DATABASE_TABLES.Add(new EventTable());
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			foreach (TableTemplate tableTemplate in DATABASE_TABLES)
			{
				string query = tableTemplate.CreateTableQuery();
				db.ExecSQL(query);
			}
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			foreach (TableTemplate tableTemplate in DATABASE_TABLES)
			{
				string query = tableTemplate.DropTableQuery();
				db.ExecSQL(query);
			}
			OnCreate(db);
		}
	}
}
