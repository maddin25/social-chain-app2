using System.Collections.Generic;
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
		private List<Event> _eventsList;
		private Event _selectedEvent;

		public EventListPageViewModel()
		{
			EventService eventService = new EventService();
			EventsList = eventService.GetEvents();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public List<Event> EventsList
		{
			get { return _eventsList; }
			set
			{
				_eventsList = value;
				OnPropertyChanged();
			}
		}

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