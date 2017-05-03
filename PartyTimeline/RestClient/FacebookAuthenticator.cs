using System;
using System.Diagnostics;

using Xamarin.Auth;
using Xamarin.Forms;

namespace PartyTimeline
{
	public class FacebookAuthenticator
	{
		public OAuth2Authenticator Authenticator { get; set; }

		public FacebookAuthenticator()
		{
			Authenticator = new OAuth2Authenticator(
				clientId: "1632106426819143",
				scope: "email,user_events",
				authorizeUrl: new Uri("https://www.facebook.com/v2.9/dialog/oauth/"),
				redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
				isUsingNativeUI: false
			);
			Authenticator.Completed += (object sender, AuthenticatorCompletedEventArgs e) =>
			{
				if (e.IsAuthenticated)
				{
					SessionInformation.INSTANCE.SetCurrentUser(e.Account);
					Application.Current.MainPage.Navigation.PushModalAsync(new EventListPage());
				}
				Debug.WriteLine($"Authenticated: {e.IsAuthenticated}");
			};
		}
	}
}
