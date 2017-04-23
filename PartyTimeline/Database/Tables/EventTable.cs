using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class EventTable : TableTemplate
	{
		public readonly string ColumnEventName = "event_name";
		public readonly string ColumnEventDescription = "event_description";

		public EventTable() : base()
		{
			TableName = "events";
			Columns.Add(new Column { Name = ColumnEventName, DataType = Column.DATATYPES["TEXT"], Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = ColumnEventDescription, DataType = Column.DATATYPES["TEXT"] });
		}

		public string Insert(Event eventRef)
		{
			Dictionary<string, object> column_value_pairs = new Dictionary<string, object> {
				{ColumnId, eventRef.Id},
				{ColumnEventName, eventRef.Name},
				{ColumnDateCreated, eventRef.DateCreated.ToFileTimeUtc()},
				{ColumnLastModified, eventRef.DateLastModified.ToFileTimeUtc()}
			};
			if (!string.IsNullOrWhiteSpace(eventRef.Description))
			{
				column_value_pairs.Add(ColumnEventDescription, eventRef.Description);
			}
			string statement = StatementInsertInto(TableName, column_value_pairs);
			return statement;
		}
	}
}
