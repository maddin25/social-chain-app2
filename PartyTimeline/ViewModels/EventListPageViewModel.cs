using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EventListPageViewModel : UIBindingHelper<Event>
	{
		public ObservableCollection<Event> EventList { get; private set; }
		public Command AddEventCommand { get; set; }

		public EventListPageViewModel(ListView refreshableListView) : base(refreshableListView)
		{
			EventList = EventService.INSTANCE.EventList;
			AddEventCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new AddEventPage()));
		}

		// FIXME: sometimes selecting an entry that has previously been selected does not trigger loading the new page
		protected override void OnSelect(ref Event selectedEvent)
		{
			var indexOfSelectedEvent = EventList.IndexOf(selectedEvent);
			Debug.WriteLine($"Event Nr. {indexOfSelectedEvent + 1} selected");
			Application.Current.MainPage.Navigation.PushAsync(new EventPageThumbnails(ref selectedEvent));
		}

		protected override async Task OnRefreshTriggered()
		{
			await EventService.INSTANCE.QueryLocalEventListAsync();
		}

		new public void OnAppearing()
		{
			base.OnAppearing();
			RefreshListCommand.Execute(null);
		}
	}
}