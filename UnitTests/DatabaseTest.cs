using NUnit.Framework;

using System;

using PartyTimeline;

namespace UnitTests
{
	[TestFixture()]
	public class DatabaseTest
	{
		[Test()]
		public void TestCreateTableStatements()
		{
			Console.WriteLine(new EventTable().CreateTableQuery());
		}
	}
}
