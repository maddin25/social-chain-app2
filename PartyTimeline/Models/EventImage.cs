using System;
using System.Diagnostics;

using SQLite;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace PartyTimeline
{
	[JsonObject(MemberSerialization.OptIn)]
	[Table("event_images")]
	public class EventImage : BaseModel
	{
		private string caption;
		private string pathOriginal;
		private string pathSmall;

        public const string UidCaption = "caption";
		public const string UidEventId = "event_id";
		public const string UidEventMemberId = "user_id";
        public const string UidPathSmall = "path_small";
        public const string UidPathOriginal = "path_original";
        public const string UidDateTaken = "date_taken";

        public const int CaptionCharacterLimit = 80;

		// TODO: how to create unique EventImage ID?
		[JsonProperty(UidCaption)]
		[Column(UidCaption), MaxLength(CaptionCharacterLimit)]
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

		[JsonIgnore]
		[Column(UidPathOriginal), NotNull, Unique]
		public string PathOriginal
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
                OnPropertyChanged(nameof(PathSmall));
                OnPropertyChanged(nameof(PathOriginal));
			}
		}

        [JsonIgnore]
		[Column(UidPathSmall), Unique]
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
                OnPropertyChanged(nameof(PathOriginal));
            }
		}

		[JsonProperty(UidEventId)]
		[Column(UidEventId), NotNull]
		public long EventId { get; set; }

		/// <summary>
		/// The event member who took the picture
		/// </summary>
		/// <value>The event member identifier</value>
		[JsonProperty(UidEventMemberId)]
		[Column(UidEventMemberId), NotNull]
		public long EventMemberId { get; set; }

		[JsonProperty(UidDateTaken)]
		[Column(UidDateTaken), NotNull]
		public DateTime DateTaken { get; set; }

		#region UIRelated
		[JsonIgnore]
		[Ignore]
		public Command OnCaptionEditCompletedCommand { get; set; }

		[JsonIgnore]
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
				PathOriginal = img.PathOriginal;
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
