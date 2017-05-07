using System;
using System.Diagnostics;
using System.Threading.Tasks;

using PartyTimeline.ViewModels;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventListPage : ContentPage
	{
		public EventListPage()
		{
			InitializeComponent();
			BindingContext = new EventListViewModel();
			NavigationPage.SetHasNavigationBar(this, true);
			NavigationPage.SetHasBackButton(this, false);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			EventService.INSTANCE.SyncStateChanged += OnSyncStateChanged;
			SetActivityIndicator(EventService.INSTANCE.CurrentSyncState.EventListSyncing);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			EventService.INSTANCE.SyncStateChanged -= OnSyncStateChanged;
		}

		public void OnSyncStateChanged(object sender, EventArgs e)
		{
			if (e is SyncState)
			{
				SyncState state = e as SyncState;
				SetActivityIndicator(state.EventListSyncing);
			}
		}

		protected override bool OnBackButtonPressed()
		{
			Debug.WriteLine($"Back pressed in {nameof(EventListPage)}");
			if (Device.RuntimePlatform == Device.Android)
			{
				DependencyService.Get<SystemInterface>().Close();
			}
			return true;
		}

		private void SetActivityIndicator(bool active)
		{
			Debug.WriteLine($"{nameof(EventListPage)}:{nameof(SetActivityIndicator)}: active={active}");
			ListViewEvents.IsRefreshing = active;
		}
	}
}
