
namespace PartyTimeline
{
	public class EventTable : TableTemplate
	{
		public EventTable() : base()
		{
			TABLE_NAME = "events";
			COLUMNS.Add(new Column { NAME = "name", DATATYPE = Column.DATATYPES["TEXT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
			COLUMNS.Add(new Column { NAME = "date_created", DATATYPE = Column.DATATYPES["INT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
			COLUMNS.Add(new Column { NAME = "date_last_updated", DATATYPE = Column.DATATYPES["INT"] });
		}
	}
}
