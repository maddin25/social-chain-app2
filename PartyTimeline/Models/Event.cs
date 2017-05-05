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
		[JsonProperty("name", Required = Required.Always)]
		[Column("event_name"), NotNull]
		public string Name { get; set; }

		[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
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

		[Column("cover_url")]
		public string CoverUrl
		{ 
			get { return Cover?.Source.AbsoluteUri; }
			set { Cover.Source = new Uri(value); }
		}

		// TODO: add these properties as well
		//public string Location { get; set; }
		[Ignore]
		public SortableObservableCollection<EventMember> Contributors { get; set; }
		[Ignore]
		public SortableObservableCollection<EventImage> Images { get; set; }

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
		}
	}
}
