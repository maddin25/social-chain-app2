using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Android.Content;
using Android.Database.Sqlite;

namespace PartyTimeline.Droid
{
	public class EventDatabase : SQLiteOpenHelper
	{
		private static int DatabaseVersion = 2;
		private List<TableTemplate> Tables = new List<TableTemplate>();

		public EventDatabase(Context context)
			: base(context, "PartyTimeline.db", null, DatabaseVersion)
		{
			SDebug.WriteLine("Adding table representations to internal list");
			Tables.Add(new EventTable());
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
	}
}
