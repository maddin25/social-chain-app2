using Xamarin.Forms;

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
