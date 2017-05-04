using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

using PartyTimeline.Resources;

namespace PartyTimeline
{
	public partial class FacebookLoginPage : ContentPage
	{
		private bool inhibitAutomaticPrompt;
		private bool _isAuthorizing;
		private string _statusMessage = AppResources.LoginStatusLoggingIn;
		public bool IsAuthorizing
		{
			get { return _isAuthorizing; }
			set
			{
				_isAuthorizing = value;
				OnPropertyChanged(nameof(IsAuthorizing));
			}
		}
		public string StatusMessage
		{
			get { return _statusMessage; }
			set
			{
				_statusMessage = value;
				OnPropertyChanged(nameof(StatusMessage));
			}
		}

		FacebookClient fbCommunicator;

		public Command StartLoginCommand { get; set;}

		public FacebookLoginPage()
		{
			InitializeComponent();
			StartLoginCommand = new Command(() =>
			{
				IsAuthorizing = true;
				StatusMessage = AppResources.LoginStatusLoggingIn;
				fbCommunicator = new FacebookClient();
				fbCommunicator.Authorize(onSessionReady, onFailure);
			});
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (IsAuthorizing || inhibitAutomaticPrompt)
			{
				return;
			}
			// FIXME: launching new page several times
			if (SessionInformation.INSTANCE.ActiveSessionAvailable())
			{
				onSessionReady();
			}
			else
			{
				StartLoginCommand.Execute(null);
			}
		}

		protected void onSessionReady()
		{
			Debug.WriteLine("User session is ready.");
			IsAuthorizing = false;
			StatusMessage = AppResources.LoginStatusSuccess
        		+ SessionInformation.INSTANCE.GetUserProperty(FacebookAccountProperties.Name) ?? string.Empty;
			Task.Delay(2000);
			Application.Current.MainPage.Navigation.PushModalAsync(new EventListPage());
		}

		protected void onFailure(string reason)
		{
			StatusMessage = AppResources.LoginStatusFailed;
			IsAuthorizing = false;
            inhibitAutomaticPrompt = true;
		}
	}
}