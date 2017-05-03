using System;
using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content;

using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.FacebookInterface_Android))]
namespace PartyTimeline.Droid
{
	public class FacebookInterface_Android : FacebookInterface
	{
		public void LaunchLogin(Authenticator authenticator)
		{
			// TODO: do this https://forums.xamarin.com/discussion/92167/oauth2authenticator-getui-this-not-working
			try
			{
				Activity activity = (Activity)Forms.Context;
				Intent intent = (Intent)authenticator.GetUI(activity);
				Forms.Context.StartActivity(intent);
			}
			catch (Exception ex)
			{
				SDebug.WriteLine(ex.Message);
			}
		}
	}
}
