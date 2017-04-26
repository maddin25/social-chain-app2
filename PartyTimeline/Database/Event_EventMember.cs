
using SQLite;

namespace PartyTimeline
{
	public class Event_EventMember
	{
		[Column("event_id"), NotNull]
		public long EventId { get; set; }
		[Column("event_member_id"), NotNull]
		public long EventMemberId { get; set; }
		// TODO: make this a foreign key to another role table
		[Column("role"), NotNull]
		public int Role { get; set; }
	}
}
