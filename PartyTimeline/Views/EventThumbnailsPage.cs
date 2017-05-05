using Xamarin.Forms;

using PartyTimeline.ViewModels;

namespace PartyTimeline
{
	public partial class EventThumbnailsPage : ContentPage
	{
		public EventThumbnailsPage()
		{
			InitializeComponent();
		}

		public EventThumbnailsPage(ref Event eventReference)
		{
			InitializeComponent();
			BindingContext = new EventPageViewModel(ref eventReference, ListViewEventThumbnails);
		}
	}
}
