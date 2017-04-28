using System;
using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{
	[Table("event_images")]
	public class EventImage : BaseModel
	{
		// TODO: how to create unique EventImage ID?
		[Column("caption")]
		public string Caption { get; set; }
		[Column("path"), NotNull, Unique]
		public string Path { get; set; }
		[Column("path_small"), Unique]
		public string PathSmall { get; set; }
		[Column("event_id"), NotNull]
		public long EventId { get; set; }
		[Column("event_member_id"), NotNull]
		public long EventMemberId { get; set; }
		// TODO: should we introduce a date taken DateTime as well?

		public EventImage(DateTime dateCreated) : base(dateCreated)
		{
			Initialize();
		}

		public EventImage()
		{
			Initialize();
		}

		private void Initialize()
		{
			OnDelete = new Command<BaseModel>(EventService.INSTANCE.Remove);
			Caption = string.Empty;
		}
	}
}
