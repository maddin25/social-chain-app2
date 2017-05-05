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
					StatusMessage = AppResources.LoginStatusSuccess
						+ SessionInformationProvider.INSTANCE.GetUserProperty(FacebookAccountProperties.Name) ?? string.Empty;
					SessionInformationProvider.INSTANCE.SessionStateChanged -= OnSessionStateChanged;
					DependencyService.Get<FacebookInterface>().CloseLogin();

					// Create the page first to let it register to the EventService events
					EventListPage eventListPage = new EventListPage();

					Task.Run(EventService.INSTANCE.LoadEventList);
					Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(eventListPage));
				}
				else
				{
					InhibitAutomaticPrompt = true;
					StatusMessage = AppResources.LoginStatusFailed;
				}
			}
		}
	}
}