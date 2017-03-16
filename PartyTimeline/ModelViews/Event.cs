using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PartyTimeline
{
	public class Event
	{
		public string Name { get; set; }
		public ObservableCollection<Contributor> Contributors { get; set; }
		public ObservableCollection<EventImage> Images { get; set; }
		public DateTime Date { get; set; }
		public string GetDateTimeString { get { return Date.ToString(); } }
		public int NrPictures { get { return 1234; /* return Images.Count; */ } }
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures > 1 ? "s" : "")); } }
		public int NrContributors { get { return 4; /* return Contributors.Count; */ } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors > 1 ? "s" : "")); } }
		// The image should be in dimensions 3:1 (width:height)
		public string GetPreviewURL 
		{
			get
			{
				return "https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg";
			}
		}

		public Event()
		{
			InitializeDummy();
		}

		void InitializeDummy()
		{
			Images = new ObservableCollection<EventImage>();

			int maxNumberPictures = 30;
			var nrGenerator = new Random(DateTime.Now.Millisecond);
			int numberPictures = nrGenerator.Next() % maxNumberPictures;

			for (int i = 0; i < numberPictures; i++)
			{
				Images.Add(new EventImage());
			}
			Debug.WriteLine("Event '{0}' initialized with {1} images", Name, numberPictures);
		}
	}
}
