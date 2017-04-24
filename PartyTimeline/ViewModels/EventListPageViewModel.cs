using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using PartyTimeline.Annotations;
using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EventListPageViewModel : INotifyPropertyChanged
	{
		private Event _selectedEvent;

		public EventListPageViewModel()
		{
			EventsList = EventService.INSTANCE.EventList;
			AddEventCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new AddEventPage()));
			RefreshEventListCommand = new Command(EventService.INSTANCE.QueryLocalEventList);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<Event> EventsList { get; private set; }

		public Command AddEventCommand { get; set; }
		public Command RefreshEventListCommand { get; set; }

		public Event SelectedEvent
		{
			get { return _selectedEvent; }
			set
			{
				_selectedEvent = value;
				if (_selectedEvent != null)
				{
					var indexOfSelectedEvent = EventsList.IndexOf(_selectedEvent);
					Debug.WriteLine($"Event Nr. {indexOfSelectedEvent + 1} selected");
					DependencyService.Get<EventSyncInterface>().StartEventSyncing(ref _selectedEvent);
					Application.Current.MainPage.Navigation.PushAsync(new EventPageThumbnails(ref _selectedEvent));
				}
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}