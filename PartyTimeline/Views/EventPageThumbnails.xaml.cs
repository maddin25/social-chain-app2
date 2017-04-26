using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{
		private EventPageViewModel viewModel;

		public EventPageThumbnails()
		{
			InitializeComponent();
		}

		public EventPageThumbnails(ref Event eventReference)
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
