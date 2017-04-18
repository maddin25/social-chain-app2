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

        public event PropertyChangedEventHandler PropertyChanged;

        public Command EventTappedCommand
        {
            get
            { 
                return new Command<ItemTappedEventArgs> ((e) =>
                {
                    //specify a parameter type like above 
                    // and 'p' will be the item which was tapped
                    var selectedEvent = e.Item as Event;
                    var indexOfSelectedEvent = EventsList.IndexOf(selectedEvent);
                    Debug.WriteLine("Event nr {0} selected", indexOfSelectedEvent + 1);
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(
                        new EventPageThumbnails(ref selectedEvent));
                });
                
            }
        }

        private List<Event> _eventsList;

        public List<Event> EventsList
        {
            get { return _eventsList; }
            set
            {
                _eventsList = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EventListPageViewModel()
        {
            EventService eventService = new EventService();
            EventsList = eventService.GetEvents();
        }
    }
}