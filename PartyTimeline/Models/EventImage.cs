using System;
using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{
	[Table("event_images")]
	public class EventImage : BaseModel
	{
		private string pathSmall;
		private string pathOriginal;

		// TODO: how to create unique EventImage ID?
		[Column("caption")]
		public string Caption { get; set; }

		[Column("path"), NotNull, Unique]
		public string Path
		{
			get
			{
				if (string.IsNullOrEmpty(pathOriginal))
				{
					return pathSmall;
				}
				return pathOriginal;
			}
			set
			{
				pathOriginal = value;
				OnPropertyChanged(nameof(Path));
			}
		}

		[Column("path_small"), Unique]
		public string PathSmall
		{
			get
			{
				if (string.IsNullOrEmpty(pathSmall))
				{
					return pathOriginal;
				}
				return pathSmall;
			}
			set
			{
				pathSmall = value;
				OnPropertyChanged(nameof(PathSmall));
			}
		}

		[Column("event_id"), NotNull]
		public long EventId { get; set; }

		/// <summary>
		/// The event member who took the picture
		/// </summary>
		/// <value>The event member identifier</value>
		[Column("event_member_id"), NotNull]
		public long EventMemberId { get; set; }

		[Column("date_taken"), NotNull]
		public DateTime DateTaken { get; set; }

		public EventImage(DateTime dateCreated) : base(dateCreated)
		{
			Initialize();
		}

		public EventImage()
		{
			Initialize();
		}

		public override void Delete()
		{
			EventService.INSTANCE.Remove(this);
		}

		public override void Update(BaseModel update)
		{
			base.Update(update);
			if (update is EventImage)
			{
				EventImage img = update as EventImage;
				Caption = img.Caption;
				Path = img.Path;
				PathSmall = img.PathSmall;
				EventId = img.EventId;
				EventMemberId = img.EventMemberId;
				DateTaken = img.DateTaken;
			}
		}

		private void Initialize()
		{
			Caption = string.Empty;
		}
	}
}
