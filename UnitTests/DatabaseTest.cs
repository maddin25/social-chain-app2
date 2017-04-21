using NUnit.Framework;

using System;
using System.Collections.Generic;

using PartyTimeline;

namespace UnitTests
{
	[TestFixture()]
	public class DatabaseTest
	{
		List<TableTemplate> tables = new List<TableTemplate>();

		public DatabaseTest()
		{
			tables.Add(new EventTable());
			tables.Add(new ImageTable());
			tables.Add(new EventMemberTable());
			tables.Add(new Event_EventMember_Table());
		}

		[Test()]
		public void TestCreateTableStatements()
		{
			foreach (TableTemplate table in tables)
			{
				Console.WriteLine(table.CreateTableQuery());
			}
			Assert.Pass();
		}

		[Test()]
		public void TestDropTableStatements()
		{
			foreach (TableTemplate table in tables)
			{
				Console.WriteLine(table.DropTableQuery());
			}
			Assert.Pass();
		}
	}
}
