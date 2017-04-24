using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class EventMemberTable : TableTemplate
	{
		private static EventMemberTable _instance;

		public readonly string ColumnEmailAddress = "email_address";
		public readonly string ColumnFirstName = "first_name";
		public readonly string ColumnLastName = "last_name";
		public readonly string ColumnFacebookToken = "facebook_token";

		public static EventMemberTable INSTANCE
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EventMemberTable();
				}
				return _instance;
			}
		}

		private EventMemberTable()
		{
			TableName = "event_members";
			Columns.Add(new Column { Name = ColumnEmailAddress, DataType = "VARCHAR(254)", Constraint = Column.CONSTRAINTS["UNIQUE"] });
			Columns.Add(new Column { Name = ColumnFirstName, DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = ColumnLastName, DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = ColumnFacebookToken, DataType = "VARCHAR(256)", Constraint = Column.CONSTRAINTS["UNIQUE"] });
		}

		public string Insert(EventMember eventMember)
		{
			string statement = StatementInsertInto(
				TableName,
				new Dictionary<string, object>
				{
				{ColumnId, eventMember.Id},
				{ColumnDateCreated, eventMember.DateCreated},
				{ColumnLastModified, eventMember.DateLastModified},
				{ColumnEmailAddress, eventMember.EmailAddress},
				{ColumnFirstName, eventMember.FirstName},
				{ColumnLastName, eventMember.LastName},
				{ColumnFacebookToken, eventMember.FacebookToken}
				}
			);
			return statement;
		}
	}
}
