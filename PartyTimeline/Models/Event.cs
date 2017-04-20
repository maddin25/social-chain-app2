using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Plugin.Media.Abstractions;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class Event
	{
		private string _name = "Unnamed Event";

		public string ID;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		public ObservableCollection<Contributor> Contributors { get; set; }
		public ObservableCollection<EventImage> Images { get; set; }
		public DateTime Date { get; set; }
		public string GetDateTimeString { get { return Date.ToString(); } }
		public int NrPictures { get { return Images == null ? 0 : Images.Count; } }
		public int NrContributors { get { return Contributors == null ? 0 : Contributors.Count; } }
		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures > 1 ? "s" : "")); } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors > 1 ? "s" : "")); } }

		// The image should be in dimensions 3:1 (width:height)
		public string GetPreviewURL { get { return "https://farm8.staticflickr.com/7351/16355627795_204bf423e9.jpg"; } }

		public Event()
		{

		}
	}
}
