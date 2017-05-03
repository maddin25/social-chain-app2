using Xamarin.Forms;

using PartyTimeline.ViewModels;

namespace PartyTimeline
{
	public partial class EventThumbnailsPage : ContentPage
	{
		private EventPageViewModel viewModel;

		public EventThumbnailsPage()
		{
			InitializeComponent();
		}

		public EventThumbnailsPage(ref Event eventReference)
		{
			InitializeComponent();
			BindingContext = viewModel = new EventPageViewModel(ref eventReference, ListViewEventThumbnails);
		}

		protected override void OnAppearing()
		{
			viewModel.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			viewModel.OnDisappearing();
		}
	}
}
