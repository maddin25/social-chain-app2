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
			EventService.INSTANCE.PropertyChanged += OnEventServicePropertyChanged;
			EventList = new ObservableCollection<Event>(EventService.INSTANCE.EventList);
			ReloadEventList();
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

		public void OnEventServicePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(EventService.EventList))
			{
				ReloadEventList();
			}
			else if (e.PropertyName == null)
			{
				ReloadEventList();
			}
		}

		private void ReloadEventList()
		{
			// TODO: ugly implementation, always copying the list
			// maybe get around this by reading from the database and ordering by date created
			EventList.Clear();
			foreach (Event eventReference in EventService.INSTANCE.EventList)
			{
				EventList.Add(eventReference);
			}
		}
	}
}