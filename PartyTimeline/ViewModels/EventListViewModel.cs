using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public class EventListViewModel : UIBindingHelper<Event>
	{
		public ObservableCollection<Event> EventList { get; private set; }
		public Command AddEventCommand { get; set; }
		public Command LogoutCommand { get; set; }

		public EventListViewModel(ListView refreshableListView) : base(refreshableListView)
		{
			EventList = EventService.INSTANCE.EventList;
			AddEventCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new AddEventPage()));
			LogoutCommand = new Command(async () =>
			{
				bool logout = await Application.Current.MainPage.DisplayAlert(
					"Confirm Logout",
					"Are you sure want to log out?",
					"Yes",
					"Cancel"
				);
				if (logout)
				{
					SessionInformation.INSTANCE.EndSession();
					await Application.Current.MainPage.Navigation.PushModalAsync(new FacebookLoginPage());
				}
			});
		}

		protected override void OnSelect(ref Event selectedEvent)
		{
			var indexOfSelectedEvent = EventList.IndexOf(selectedEvent);
			Debug.WriteLine($"Event Nr. {indexOfSelectedEvent + 1} selected");
			Application.Current.MainPage.Navigation.PushAsync(new EventThumbnailsPage(ref selectedEvent));
		}

		protected override async Task OnRefreshTriggered()
		{
			await EventService.INSTANCE.QueryFacebookEventListAsync();
			//await EventService.INSTANCE.QueryLocalEventListAsync();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			RefreshListCommand.Execute(null);
		}
	}
}