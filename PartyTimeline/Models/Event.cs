using System;
using System.Collections.ObjectModel;

namespace PartyTimeline
{
	public class Event : BaseModel
	{
		private string _name = "";
		// TODO: how to create unique Event ID?
		public long Id;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public string Description { get; set; }
		public ObservableCollection<EventMember> Contributors { get; set; }
		public ObservableCollection<EventImage> Images { get; set; }
		public string GetDateTimeString { get { return DateCreated.ToString(); } }
		public int NrPictures { get { return Images == null ? 0 : Images.Count; } }
		public int NrContributors { get { return Contributors == null ? 0 : Contributors.Count; } }
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures == 1 ? "" : "s")); } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors == 1 ? "" : "s")); } }

		// The image should be in dimensions 3:1 (width:height)
		public string GetPreviewURL { get { return "https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg"; } }

		public Event(DateTime dateCreated) : base(dateCreated)
		{
			Images = new ObservableCollection<EventImage>();
			Contributors = new ObservableCollection<EventMember>();
		}

		public Event()
		{

		}
	}
}
