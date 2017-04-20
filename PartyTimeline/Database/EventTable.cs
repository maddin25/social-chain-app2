
namespace PartyTimeline
{
	public class EventTable : TableTemplate
	{
		public EventTable() : base()
		{
			TABLE_NAME = "events";
			COLUMNS.Add(new Column { NAME = "name", DATATYPE = DATATYPES.TEXT, CONSTRAINT = CONSTRAINTS.NOT_NULL });
			COLUMNS.Add(new Column { NAME = "date_created", DATATYPE = DATATYPES.INT, CONSTRAINT = CONSTRAINTS.NOT_NULL });
			COLUMNS.Add(new Column { NAME = "date_last_updated", DATATYPE = DATATYPES.INT });
		}
	}
}
