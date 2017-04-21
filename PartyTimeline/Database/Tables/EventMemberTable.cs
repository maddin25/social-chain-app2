using System;
namespace PartyTimeline
{
	public class EventMemberTable : TableTemplate
	{
		public EventMemberTable() : base()
		{
			TABLE_NAME = "event_members";
			COLUMNS.Add(new Column { NAME = "email_address", DATATYPE = "VARCHAR(254)", CONSTRAINT = Column.CONSTRAINTS["UNIQUE"] });
			COLUMNS.Add(new Column { NAME = "first_name", DATATYPE = Column.DATATYPES["TEXT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
			COLUMNS.Add(new Column { NAME = "last_name", DATATYPE = Column.DATATYPES["TEXT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
			COLUMNS.Add(new Column { NAME = "facebook_token", DATATYPE = "VARCHAR(256)", CONSTRAINT = Column.CONSTRAINTS["UNIQUE"] });
			string role_column_name = "role";
			COLUMNS.Add(new Column
			{
				NAME = role_column_name,
				DATATYPE = Column.DATATYPES["INT"],
				CONSTRAINT = $"CHECK ({role_column_name} BETWEEN {EventMember.ROLE_ID_MIN} AND {EventMember.ROLE_ID_MAX})"
			});
		}
	}
}
