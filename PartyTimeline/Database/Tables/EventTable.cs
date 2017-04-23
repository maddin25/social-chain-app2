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
			string statement = StatementInsertInto(
				TableName,
				new Dictionary<string, object> {
				{ColumnId, eventRef.Id},
				{ColumnEventName, eventRef.Name},
				{ColumnEventDescription, eventRef.Description},
				{ColumnDateCreated, eventRef.DateCreated.ToFileTimeUtc()},
				{ColumnLastModified, eventRef.DateLastModified.ToFileTimeUtc()}
				}
			);
			return statement;
		}
	}
}
