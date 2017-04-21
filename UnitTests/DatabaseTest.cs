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
			tables.Add(new EventMemberTable());
			tables.Add(new ImageTable());
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

		[Test()]
		public void TestInsertStatements()
		{
			ImageTable imageTable = new ImageTable();

			EventImage image1 = new EventImage(DateTime.Now.AddMinutes(-12)) { URI = "k24m235kl124n2k234k234j", Caption = "image1", Id = 1 };
			EventImage image2 = new EventImage(DateTime.Now) { URI = "2k3259xi23kjsd092390ksdk", Caption = "image2", Id = 2 };

			Event event1 = new Event(DateTime.Now.AddMinutes(-20)) { Id = 1, Description = "event1 description", Name = "awesome test event1" };

			EventMember eventMember1 = new EventMember(DateTime.Now.AddMinutes(-18)) { Id = 32, FirstName = "Hamza", LastName = "Tahir", EmailAddress = "htahir111@gmail.com", Role = EventMember.RolesIds[ROLES.Administrator] };
			EventMember eventMember2 = new EventMember(DateTime.Now.AddMinutes(-17)) { Id = 33, FirstName = "Martin", LastName = "Patz", EmailAddress = "mailto@martin-patz.de", Role = EventMember.RolesIds[ROLES.Administrator] };

			Console.WriteLine(imageTable.Insert(image1, eventMember1, event1));
			Console.WriteLine(imageTable.Insert(image2, eventMember2, event1));
		}
	}
}
