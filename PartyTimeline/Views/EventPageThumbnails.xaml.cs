using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{

		public EventPageThumbnails()
		{
			InitializeComponent();
		}

		public EventPageThumbnails(ref Event eventReference)
		{
			InitializeComponent();
			BindingContext = new EventPageViewModel(ref eventReference);
		}

	}
}
