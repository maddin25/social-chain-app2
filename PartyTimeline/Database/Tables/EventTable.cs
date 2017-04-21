
namespace PartyTimeline
{
	public class EventTable : TableTemplate
	{
		public EventTable() : base()
		{
			TableName = "events";
			Columns.Add(new Column { Name = "event_name", DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = "event_description", DataType = Column.DATATYPES["TEXT"] });
			Columns.Add(new Column { Name = "date_created", DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
		}
	}
}
