using System;
using System.ComponentModel;

namespace PartyTimeline
{
	public class EventPageViewModel : Event, INotifyPropertyChanged
	{

		public EventPageViewModel(ref Event eventReference)
		{

		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string GetNrPicturesString { get { return (NrPictures.ToString() + " Picture" + (NrPictures > 1 ? "s" : "")); } }
		public string GetNrContributorsString { get { return (NrContributors.ToString() + " User" + (NrContributors > 1 ? "s" : "")); } }
	}
}
