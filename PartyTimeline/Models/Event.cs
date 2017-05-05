using System;
using SQLite;

using Xamarin.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace PartyTimeline
{
	[Table("events")]
	public class Event : BaseModel
	{
		[JsonProperty("name")]
		[Column("event_name"), NotNull]
		public string Name { get; set; }

		[JsonProperty("description")]
		[Column("event_description")]
		public string Description { get; set; }

		[JsonProperty("start_time", Required = Required.Always)]
		[Column("start_date"), NotNull]
		public DateTime StartDateTime { get; set; }

		[JsonProperty("end_time", NullValueHandling = NullValueHandling.Ignore)]
		[Column("end_date")]
		public DateTime EndDateTime { get; set; }

		[JsonProperty("cover")]
		[Ignore]
		public CoverImage Cover { get; set; }

		[JsonProperty("is_canceled")]
		[Ignore]
		public bool IsCancelled { get; set; }

		[JsonProperty("is_draft")]
		[Ignore]
		public bool IsDraft { get; set; }

		[JsonIgnore]
		[Column("cover_url")]
		public string CoverUrl
		{
			get { return Cover.Source?.AbsoluteUri ?? string.Empty; }
			set { Cover.Source = new Uri(value); }
		}

		// TODO: add these properties as well
		//public string Location { get; set; }

		[JsonIgnore]
		[Ignore]
		public SortableObservableCollection<EventMember> Contributors { get; set; }

		[JsonIgnore]
		[Ignore]
		public SortableObservableCollection<EventImage> Images { get; set; }

		public void Update(Event e)
		{
			this.DateLastModified = e.DateLastModified;
			this.Name = e.Name;
			this.Description = e.Description;
			this.StartDateTime = e.StartDateTime;
			this.EndDateTime = e.EndDateTime;
			this.Cover = e.Cover;
			this.IsCancelled = e.IsCancelled;
			this.IsDraft = e.IsDraft;
		}

		public Event(DateTime dateCreated) : base(dateCreated)
		{
			Initialize();
		}

		public Event()
		{
			Initialize();
		}

		private void Initialize()
		{
			OnDelete = new Command<BaseModel>(EventService.INSTANCE.Remove);
			Images = new SortableObservableCollection<EventImage>();
			Contributors = new SortableObservableCollection<EventMember>();
			Cover = new CoverImage();
		}

		public override string ToString()
		{
			return string.Format($"[Event: Name={Name}, StartDateTime={StartDateTime}, Id={Id}");
		}
	}
}
