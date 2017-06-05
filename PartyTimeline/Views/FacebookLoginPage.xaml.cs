using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

using PartyTimeline.Resources;

namespace PartyTimeline
{
	public partial class FacebookLoginPage : ContentPage
	{
		private bool _inhibitAutomaticPrompt;
		private bool _isAuthorizing;
		private string _statusMessage;
		public bool IsAuthorizing
		{
			get { return _isAuthorizing; }
			set
			{
				_isAuthorizing = value;
				if (_isAuthorizing)
				{
					StatusMessage = AppResources.LoginStatusLoggingIn;
				}
				OnPropertyChanged(nameof(IsAuthorizing));
			}
		}
		public bool InhibitAutomaticPrompt
		{
			get { return _inhibitAutomaticPrompt; }
			set
			{
				_inhibitAutomaticPrompt = value;
				if (_inhibitAutomaticPrompt)
				{
					StatusMessage = AppResources.LoginStatusWaitForUser;
				}
				else
				{
					StatusMessage = AppResources.LoginStatusLoggingIn;
				}
				OnPropertyChanged(nameof(InhibitAutomaticPrompt));
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

		public Command ManualLoginCommand { get; set; }

		public FacebookLoginPage(bool inhibitAutomaticPrompt)
		{
			InitializeComponent();
			InhibitAutomaticPrompt = inhibitAutomaticPrompt;
			ManualLoginCommand = new Command(() =>
			{
				Debug.WriteLine($"{nameof(ManualLoginCommand)} called");
			    IsAuthorizing = true;
			    InhibitAutomaticPrompt = true;
                SessionInformationProvider.INSTANCE.AuthenticateUserIfRequired();
			});
			SessionInformationProvider.INSTANCE.SessionStateChanged += OnSessionStateChanged;
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = this;
		}

		protected override bool OnBackButtonPressed()
		{
			Debug.WriteLine($"Back pressed in {nameof(FacebookLoginPage)}");
			if (Device.RuntimePlatform == Device.Android)
			{
				DependencyService.Get<SystemInterface>().Close();
			}
			return true;
		}

		public void OnSessionStateChanged(object sender, EventArgs e)
		{
			if (e is SessionState)
			{
				SessionState state = e as SessionState;
				if (state.IsAuthenticated)
				{
					SessionInformationProvider.INSTANCE.SessionStateChanged -= OnSessionStateChanged;
					StatusMessage = AppResources.LoginStatusSuccess
						+ SessionInformationProvider.INSTANCE.GetUserProperty(FacebookAccountProperties.Name) ?? string.Empty;
					DependencyService.Get<FacebookInterface>().CloseLogin();
				    IsAuthorizing = false;
					// FIXME: if the facebook login page was initialized before, this call does not open a new event list page
					Application.Current.MainPage.Navigation.PushAsync(new EventListPage());
				}
				else // authentication failure or user abort
				{
					InhibitAutomaticPrompt = true;
					StatusMessage = AppResources.LoginStatusFailed;
				}
			}
		}
	}
}