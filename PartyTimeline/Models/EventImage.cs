using System;
using System.Diagnostics;

using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{

	[Table("event_images")]
	public class EventImage : BaseModel
	{
		private string caption;
		private string pathSmall;
		private string pathOriginal;

		public const int CaptionCharacterLimit = 80;

		// TODO: how to create unique EventImage ID?
		[Column("caption"), MaxLength(CaptionCharacterLimit)]
		public string Caption
		{
			get { return caption; }
			set
			{
				caption = value;
				// TODO: Maybe check constraints here
				OnPropertyChanged(nameof(CaptionLength));
				OnPropertyChanged(nameof(Caption));
			}
		}

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

		#region UIRelated
		[Ignore]
		public Command OnCaptionEditCompletedCommand { get; set; }

		[Ignore]
		public string CaptionLength
		{
			get
			{
				return $"{Caption.Length} / {CaptionCharacterLimit}";
			}
		}
		#endregion

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

		public static string ImageCaption { get { return "Caption"; } }

		private void Initialize()
		{
			Caption = string.Empty;
			OnCaptionEditCompletedCommand = new Command(async (obj) =>
			{
				await EventService.INSTANCE.PersistElement(this).ConfigureAwait(false);
			});
		}
	}
}
