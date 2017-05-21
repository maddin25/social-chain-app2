using System;
using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content;
using PartyTimeline.Droid.oauth;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.FacebookInterface_Android))]
namespace PartyTimeline.Droid
{
	public class FacebookInterface_Android : FacebookInterface
	{
		public void LaunchLogin(OAuth2Authenticator authenticator)
		{
			// TODO: do this https://forums.xamarin.com/discussion/92167/oauth2authenticator-getui-this-not-working
			try
			{
				/*Activity activity = (Activity)Forms.Context;
				Intent intent = (Intent)authenticator.GetUI(activity);
				Forms.Context.StartActivity(intent);*/

			    Xamarin.Auth.Presenters.OAuthLoginPresenter.PlatformLogin = (authenticator2) =>
			    {
			        var oAuthLogin = new OAuthLoginPresenter();
			        oAuthLogin.Login(authenticator);
			    };
            }
			catch (Exception ex)
			{
				SDebug.WriteLine(ex.Message);
			}
		}

		public void CloseLogin()
		{

		}
	}
}
