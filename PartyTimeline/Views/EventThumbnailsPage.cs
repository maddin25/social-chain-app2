using System;
using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.ViewModels;

namespace PartyTimeline
{
	public partial class EventThumbnailsPage : ContentPage
	{
		long eventId;
		EventThumbnailsViewModel viewModel;

		public EventThumbnailsPage()
		{
			InitializeComponent();
		}

		public EventThumbnailsPage(ref Event eventReference)
		{
			InitializeComponent();
			eventId = eventReference.Id;
			BindingContext = viewModel = new EventThumbnailsViewModel(ref eventReference);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.Initialize();
			EventService.INSTANCE.SyncStateChanged += OnSyncStateChanged;
			SetActivityIndicator(EventService.INSTANCE.CurrentSyncState.EventListSyncing);
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			viewModel.Deinitialize();
			EventService.INSTANCE.SyncStateChanged -= OnSyncStateChanged;
		}

		public void OnSyncStateChanged(object sender, EventArgs e)
		{
			if (e is SyncState)
			{
				SyncState state = e as SyncState;
				if (state.EventIdSyncing == eventId)
				{
					SetActivityIndicator(state.EventDetailsSyncing);
				}
			}
		}

		private void SetActivityIndicator(bool active)
		{
			Debug.WriteLine($"{nameof(EventThumbnailsPage)}:{nameof(SetActivityIndicator)}: active={active}");
			ListViewEventThumbnails.IsRefreshing = active;
		}
	}
}
