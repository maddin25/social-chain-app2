using SQLite;

namespace PartyTimeline
{
	public class Event_EventMember
	{
		[Column("event_id"), NotNull]
		public long EventId { get; set; }
		[Column("event_member_id"), NotNull]
		public long EventMemberId { get; set; }
		[Column("role"), NotNull]
		public int Role { get; set; }
	}
}
