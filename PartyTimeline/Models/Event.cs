using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

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
			InitializeLists();
		}

		public Event()
		{
			InitializeLists();
		}

		private void InitializeLists()
		{
			Images = new SortableObservableCollection<EventImage>();
			Contributors = new SortableObservableCollection<EventMember>();
		}

		public override string ToString()
		{
			return $"Event: {Id}" +
				$"\n\tName={Name}" +
				$"\n\tDescription={Description}" +
				$"\n\tDateCreated={DateCreated}" +
				$"\n\tDateLastModified={DateLastModified}" +
				$"\n\tStartDate={StartDateTime}" +
				$"\n\tEndDate={EndDateTime}" +
				$"\n\tContributors={Contributors}" +
				$"\n\tImages={Images}" +
				$"\n\tGetDateTimeString={GetDateTimeString}" +
				$"\n\tNrPictures={NrPictures}" +
				$"\n\tNrContributors={NrContributors}" +
				$"\n\tGetNrPicturesString={GetNrPicturesString}" +
				$"\n\tGetNrContributorsString={GetNrContributorsString}" +
				$"\n\tGetPreviewURL={GetPreviewURL}";
		}
	}
}
