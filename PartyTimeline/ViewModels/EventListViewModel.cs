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
		public Command LogoutCommand { get; set; }

		public EventListViewModel()
		{
			EventList = EventService.INSTANCE.EventList;
			LogoutCommand = new Command(async () =>
			{
				bool logout = await Application.Current.MainPage.DisplayAlert(
					// TODO: use resource strings here
					"Confirm Logout",
					"Are you sure want to log out?",
					"Yes",
					"Cancel"
				);
				if (logout)
				{
					SessionInformationProvider.INSTANCE.EndSession();
					await Application.Current.MainPage.Navigation.PushModalAsync(new FacebookLoginPage(inhibitAutomaticPrompt: true));
				}
			});
			// If this command is run on on a different thread, the app crashes
			EventService.INSTANCE.LoadEventList();
		}

		protected override void OnSelect(ref Event element)
		{
			var indexOfSelectedEvent = EventList.IndexOf(element);
			Debug.WriteLine($"Event Nr. {indexOfSelectedEvent + 1} selected");
			Application.Current.MainPage.Navigation.PushAsync(new EventThumbnailsPage(ref element), true);
		}

		protected override async Task OnRefreshTriggered()
		{
			await EventService.INSTANCE.LoadEventList();
		}
	}
}