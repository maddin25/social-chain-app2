using System.Diagnostics;

using PartyTimeline.ViewModels;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventListPage : ContentPage
	{
		public EventListPage()
		{
			InitializeComponent();
			BindingContext = new EventListPageViewModel(ListViewEvents);
			NavigationPage.SetHasBackButton(this, false);
		}

		protected override bool OnBackButtonPressed()
		{
			Debug.WriteLine("Back pressed in EventListPage");
			if (Device.RuntimePlatform == Device.Android)
			{
				DependencyService.Get<SystemInterface>().Close();
			}
			return true;
		}
	}
}
