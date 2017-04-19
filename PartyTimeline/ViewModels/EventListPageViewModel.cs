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
			EventTappedCommand = new Command<Event>((selectedEvent) =>
			{
				if (selectedEvent == null)
				{
					return;
				}
				var indexOfSelectedEvent = EventsList.IndexOf(selectedEvent);
				Debug.WriteLine("Event nr {0} selected", indexOfSelectedEvent + 1);
				Application.Current.MainPage.Navigation.PushAsync(new EventPageThumbnails(ref selectedEvent));
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Command EventTappedCommand { get; }

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
			get
			{
				return _selectedEvent;
			}
			set
			{
				_selectedEvent = value;
				if (_selectedEvent != null)
				{
					EventTappedCommand.Execute(_selectedEvent);
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