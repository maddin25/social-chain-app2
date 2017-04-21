
namespace PartyTimeline
{
	public class EventMemberTable : TableTemplate
	{
		public EventMemberTable() : base()
		{
			TableName = "event_members";
			Columns.Add(new Column { Name = "email_address", DataType = "VARCHAR(254)", Constraint = Column.CONSTRAINTS["UNIQUE"] });
			Columns.Add(new Column { Name = "first_name", DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = "last_name", DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = "facebook_token", DataType = "VARCHAR(256)", Constraint = Column.CONSTRAINTS["UNIQUE"] });
		}
	}
}
