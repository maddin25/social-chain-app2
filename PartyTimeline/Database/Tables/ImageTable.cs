
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
			string column_eventmember_id = "event_member_id";
			Columns.Add(new Column { Name = column_eventmember_id, DataType = COLUMNDATATYPE_ID, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			string column_event_id = "event_id";
			Columns.Add(new Column { Name = column_event_id, DataType = COLUMNDATATYPE_ID, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });

			Relationships.Add(RelationshipForeignKey(column_eventmember_id, new EventMemberTable().TableName, COLUMNNAME_ID));
			Relationships.Add(RelationshipForeignKey(column_event_id, new EventTable().TableName, COLUMNNAME_ID));
		}
	}
}
