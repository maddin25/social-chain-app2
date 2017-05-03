using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class FacebookLoginPage : ContentPage
	{
		bool isAuthorizing = false;

		FacebookClient fbCommunicator;

		public FacebookLoginPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			if (isAuthorizing)
			{
				return;
			}

			fbCommunicator = new FacebookClient();
			if (fbCommunicator.IsAuthorized())
			{
				fbCommunicator.OnIsAuthorized();
			}
			else
			{
				isAuthorizing = true;
				DependencyService.Get<FacebookInterface>().LaunchLogin(fbCommunicator.Authenticator);
			}
		}
	}
}