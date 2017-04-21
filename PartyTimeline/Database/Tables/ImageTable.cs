
namespace PartyTimeline
{
	public class ImageTable : TableTemplate
	{
		public ImageTable() : base()
		{
			TableName = "event_images";
			Columns.Add(new Column { Name = "uri", DataType = "VARCHAR(256)", Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			Columns.Add(new Column { Name = "caption", DataType = "VARCHAR(100)" });
			Columns.Add(new Column { Name = "date_taken", DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = "date_last_modified", DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
		}
	}
}
