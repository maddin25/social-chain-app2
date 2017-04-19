using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Plugin.Media.Abstractions;

namespace PartyTimeline
{
	public class Event
	{
		private string _name = "Unnamed Event";

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public ObservableCollection<Contributor> Contributors { get; set; }
		public ObservableCollection<EventImage> Images { get; set; }
		public DateTime Date { get; set; }
		public string GetDateTimeString { get { return Date.ToString(); } }
		public int NrPictures { get { return 1234; /* return Images.Count; */ } }
		public int NrContributors { get { return 4; /* return Contributors.Count; */ } }
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures > 1 ? "s" : "")); } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors > 1 ? "s" : "")); } }

		// The image should be in dimensions 3:1 (width:height)
		public string GetPreviewURL
		{
			get
			{
				return "https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg";
			}
		}

		public void AddEventImage(String path)
		{
			EventImage newEventImage = new EventImage();
			newEventImage.ShortAnnotation = "Default Short Annotation";
			newEventImage.DateTaken = DateTime.Now;
			Random nrGenerator = new Random(DateTime.Now.Millisecond);
			newEventImage.Id = nrGenerator.Next();
			newEventImage.URI = path;

			Images.Add(newEventImage);
		}

		public Event()
		{

		}
	}
}
