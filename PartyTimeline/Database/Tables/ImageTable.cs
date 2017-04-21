
namespace PartyTimeline
{
	public class ImageTable : TableTemplate
	{
		public ImageTable() : base()
		{
			TABLE_NAME = "event_images";
			COLUMNS.Add(new Column { NAME = "uri", DATATYPE = "VARCHAR(256)", CONSTRAINT = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			COLUMNS.Add(new Column { NAME = "caption", DATATYPE = "VARCHAR(100)" });
			COLUMNS.Add(new Column { NAME = "date_taken", DATATYPE = Column.DATATYPES["INT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
			COLUMNS.Add(new Column { NAME = "date_last_modified", DATATYPE = Column.DATATYPES["INT"], CONSTRAINT = Column.CONSTRAINTS["NOT_NULL"] });
		}
	}
}
