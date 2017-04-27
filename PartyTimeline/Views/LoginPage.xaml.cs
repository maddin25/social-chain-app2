using Xamarin.Forms;

using PartyTimeline.ViewModels;

namespace PartyTimeline
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new LoginPageViewModel();
		}
	}
}
