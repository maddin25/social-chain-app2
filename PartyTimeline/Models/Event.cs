using System;
using SQLite;

using Xamarin.Forms;

namespace PartyTimeline
{
	[Table("events")]
	public class Event : BaseModel
	{
		// TODO: how to create unique Event ID?
		[Column("event_name"), NotNull]
		public string Name { get; set; }
		[Column("event_description")]
		public string Description { get; set; }
		[Column("start_date"), NotNull]
		public DateTime StartDateTime { get; set; }
		[Column("end_date"), NotNull]
		public DateTime EndDateTime { get; set; }
		// TODO: add these properties as well
		//public string Location { get; set; }
		[Ignore]
		public SortableObservableCollection<EventMember> Contributors { get; set; }
		[Ignore]
		public SortableObservableCollection<EventImage> Images { get; set; }
		[Ignore]
		public string GetDateTimeString { get { return DateCreated.ToString(); } }
		[Ignore]
		public int NrPictures { get { return Images == null ? 0 : Images.Count; } }
		[Ignore]
		public int NrContributors { get { return Contributors == null ? 0 : Contributors.Count; } }
		[Ignore]
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures == 1 ? "" : "s")); } }
		[Ignore]
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors == 1 ? "" : "s")); } }
		// The image should be in dimensions 3:1 (width:height)
		[Ignore]
		public string GetPreviewURL { get { return "https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg"; } }

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
