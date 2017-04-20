using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PartyTimeline
{
	public class LoginPageViewModel
	{
		private string username_;
		private string password_;


		public LoginPageViewModel()
		{
			Username = "testuser@nomads.com";
			LoginCommand = new Command(async () => await Login());
			SignUpCommand = new Command(async () => await Application.Current.MainPage.DisplayAlert("Welcome", "Please sign up (not implemented)", "OK"));
			ForgotPasswordCommand = new Command(async () => await Application.Current.MainPage.DisplayAlert("Idiot", "You forgot your password", "Acknowledged"));
		}

		public string Username
		{
			get
			{
				return username_;
			}
			set
			{
				username_ = value;
			}
		}

		public string Password
		{
			get
			{
				return password_;
			}
			set
			{
				password_ = value;
			}
		}

		public Command LoginCommand { get; }
		public Command ForgotPasswordCommand { get; }
		public Command SignUpCommand { get; }

		async Task Login()
		{
			Debug.WriteLine($"Logging in with email {Username} and password {Password}");
			if (Username == null || Username.Equals(string.Empty))
			{
				return;
			}
			// TODO: Start polling event list here
			await Application.Current.MainPage.Navigation.PushAsync(new EventListPage());
		}
	}
}
