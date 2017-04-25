using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EventListPageViewModel : PropertyChangedNotifier
	{
		private Event _selectedEvent;
		private bool _isRefreshing = false;

		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}
		public ObservableCollection<Event> EventList { get; private set; }
		public Command AddEventCommand { get; set; }
		public Command RefreshEventListCommand { get; set; }

		public EventListPageViewModel()
		{
			EventList = EventService.INSTANCE.EventList;
			AddEventCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new AddEventPage()));
			RefreshEventListCommand = new Command(() =>
			{
				IsRefreshing = true;
				Task task = Task.Factory.StartNew(() => EventService.INSTANCE.QueryLocalEventList());
				task.ContinueWith((obj) => IsRefreshing = false);
			});
			RefreshEventListCommand.Execute(null);
		}

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