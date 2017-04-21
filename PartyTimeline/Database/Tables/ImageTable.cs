using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PartyTimeline
{
	public class ImageTable : TableTemplate
	{
		string column_uri = "uri";
		string column_caption = "caption";
		string column_date_taken = "date_taken";
		string column_eventmember_id = "event_member_id";
		string column_event_id = "event_id";

		public ImageTable() : base()
		{
			TableName = "event_images";
			Columns.Add(new Column { Name = column_uri, DataType = "VARCHAR(256)", Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			Columns.Add(new Column { Name = column_caption, DataType = "VARCHAR(100)" });
			Columns.Add(new Column { Name = column_date_taken, DataType = Column.DATATYPES["INT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = column_eventmember_id, DataType = COLUMNDATATYPE_ID, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			Columns.Add(new Column { Name = column_event_id, DataType = COLUMNDATATYPE_ID, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });

			Relationships.Add(RelationshipForeignKey(column_eventmember_id, new EventMemberTable().TableName, COLUMNNAME_ID));
			Relationships.Add(RelationshipForeignKey(column_event_id, new EventTable().TableName, COLUMNNAME_ID));
		}

		public string Insert(EventImage image, EventMember eventMember, Event eventReference)
		{
			string statement = StatementInsertInto(
				TableName,
				new List<string> { column_uri, column_caption, column_date_taken, column_eventmember_id, column_event_id },
				new List<Object> { image.URI, image.Caption, image.DateCreated.ToFileTimeUtc(), eventMember.Id, eventReference.Id }
			);
			return statement;
		}
	}
}
