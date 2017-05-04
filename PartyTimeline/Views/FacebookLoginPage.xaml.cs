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
			Action loadEventList = () =>
			{
				isAuthorizing = false;
				Application.Current.MainPage.Navigation.PushAsync(new EventListPage());
			};

			if (SessionInformation.INSTANCE.ActiveSessionAvailable())
			{
				loadEventList.Invoke();
			}
			else
			{
				isAuthorizing = true;
				fbCommunicator = new FacebookClient();
				fbCommunicator.Authorize(loadEventList);
			}
		}
	}
}