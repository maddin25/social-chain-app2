using Xamarin.Forms;

using PartyTimeline.Services;

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
			viewModel = new EventPageViewModel(ref eventReference);
			BindingContext = viewModel;
		}

		protected override void OnAppearing()
		{
			viewModel.Initialize();
		}

		protected override void OnDisappearing()
		{
			viewModel.Deinitialize();
		}
	}
}
