using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EventListPageViewModel
	{
		private Event _selectedEvent;

		public EventListPageViewModel()
		{
			EventList = EventService.INSTANCE.EventList;
			AddEventCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new AddEventPage()));
			RefreshEventListCommand = new Command(EventService.INSTANCE.QueryLocalEventList);
		}

		public ObservableCollection<Event> EventList { get; private set; }
		public Command AddEventCommand { get; set; }
		public Command RefreshEventListCommand { get; set; }

		// FIXME: sometimes selecting an entry that has previously been selected does not trigger loading the new page
		public Event SelectedEvent
		{
			get { return _selectedEvent; }
			set
			{
				_selectedEvent = value;
				if (_selectedEvent != null)
				{
					var indexOfSelectedEvent = EventList.IndexOf(_selectedEvent);
					Debug.WriteLine($"Event Nr. {indexOfSelectedEvent + 1} selected");
					Application.Current.MainPage.Navigation.PushAsync(new EventPageThumbnails(ref _selectedEvent));
				}
			}
		}
	}
}