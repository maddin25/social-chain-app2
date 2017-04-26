using System;
using SQLite;

namespace PartyTimeline
{
	[Table("event_images")]
	public class EventImage : BaseModel
	{
		// TODO: how to create unique EventImage ID?
		[Column("caption")]
		public string Caption { get; set; }
		[Column("uri"), NotNull, Unique]
		public string URI { get; set; }
		[Column("uri_small"), Unique]
		public string URIsmall { get; set; }
		[Column("event_id"), NotNull]
		public long EventId { get; set; }
		[Column("event_member_id"), NotNull]
		public long EventMemberId { get; set; }

		public EventImage(DateTime dateCreated) : base(dateCreated)
		{
			Caption = string.Empty;
		}

		public EventImage()
		{
			Caption = string.Empty;
		}
	}
}
