
namespace PartyTimeline
{
	public class Event_EventMember_Table : TableTemplate
	{
		private static Event_EventMember_Table _instance;

		public static Event_EventMember_Table INSTANCE
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Event_EventMember_Table();
				}
				return _instance;
			}
		}

		private Event_EventMember_Table()
		{
			TableName = "event_eventmember";

			Columns.Clear();
			string column_event_member_id = "event_member_id";
			Columns.Add(new Column { Name = column_event_member_id, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });
			string column_event_id = "event_id";
			Columns.Add(new Column { Name = column_event_id, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL_UNIQUE"] });

			string role_column_name = "role";
			Columns.Add(new Column
			{
				Name = role_column_name,
				DataType = Column.DATATYPES["INT"],
				Constraint = $"CHECK ({role_column_name} BETWEEN {EventMember.ROLE_ID_MIN} AND {EventMember.ROLE_ID_MAX})"
			});
			AddDateCreatedColumn();
			AddDateModifiedColumn();

			Relationships.Add(RelationshipForeignKey(column_event_member_id, EventMemberTable.INSTANCE.TableName, ColumnId));
			Relationships.Add(RelationshipForeignKey(column_event_id, EventTable.INSTANCE.TableName, ColumnId));
			Relationships.Add($"PRIMARY KEY({column_event_member_id}, {column_event_id})");
		}
	}
}
