
namespace PartyTimeline
{
	public class Event_EventMember_Table : TableTemplate
	{
		private static Event_EventMember_Table _instance;

		public readonly string ColumnEventMemberId = "event_member_id";
		public readonly string ColumnEventId = "event_id";
		public readonly string ColumnRole = "role";

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
			Columns.Add(new Column { Name = ColumnEventMemberId, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column { Name = ColumnEventId, DataType = ColumnDatatypeId, Constraint = Column.CONSTRAINTS["NOT_NULL"] });
			Columns.Add(new Column
			{
				Name = ColumnRole,
				DataType = Column.DATATYPES["INT"],
				Constraint = $"CHECK ({ColumnRole} BETWEEN {EventMember.ROLE_ID_MIN} AND {EventMember.ROLE_ID_MAX})"
			});
			AddDateCreatedColumn();
			AddDateModifiedColumn();

			Relationships.Add(RelationshipForeignKey(ColumnEventMemberId, EventMemberTable.INSTANCE.TableName, ColumnId));
			Relationships.Add(RelationshipForeignKey(ColumnEventId, EventTable.INSTANCE.TableName, ColumnId));
			Relationships.Add($"PRIMARY KEY({ColumnEventMemberId}, {ColumnEventId})");
		}
	}
}
