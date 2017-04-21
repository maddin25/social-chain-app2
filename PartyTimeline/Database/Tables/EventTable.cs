
namespace PartyTimeline
{
	public class EventTable : TableTemplate
	{
		public EventTable() : base()
		{
			TableName = "events";
			Columns.Add(new Column { NAame= "event_name", DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { NAame= "event_description", DataType = Column.DATATYPES["TEXT"] });
			Columns.Add(new Column { NAame= "date_created", DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { NAame= "date_last_updated", DataType = Column.DATATYPES["INT"] });
		}
	}
}
