using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public class Event
	{
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public int NrPictures { get { return 1234; /* return Images.Count; */ } }
		public int NrContributors { get { return Contributors.Count; } }
		public List<Contributor> Contributors { get; set; }
		public List<EventImage> Images { get; set; }
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures > 1 ? "s" : "")); } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors > 1 ? "s" : "")); } }
		public string GetDateTimeString { get { return Date.ToString(); } }
		// The image should be in dimensions 3:1 (width:height)
		public string GetPreviewURL
		{
			get
			{
				return "https://dummyimage.com/600x200/fff/333.jpg&text=Hello+you+dummy";
			}
		}

		public Event()
		{

		}
	}
}
