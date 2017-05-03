using System;
using SDebug = System.Diagnostics.Debug;

using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.App;
using Android.Content;

using PartyTimeline;
using PartyTimeline.Droid;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer))]
namespace PartyTimeline.Droid
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		private FacebookCommunicator auth;

		public FacebookLoginPageRenderer()
		{
			auth = new FacebookCommunicator();
			SDebug.WriteLine("Instantiated custom renderer");
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

			if (auth.IsAuthorized)
			{
				auth.OnIsAuthorized();
			}
			else
			{
				try
				{
					Activity activity = (Activity)Forms.Context;
					Intent intent = (Intent)auth.Authenticator.GetUI(activity);
					Forms.Context.StartActivity(intent);
				}
				catch (Exception ex)
				{
					SDebug.WriteLine(ex.Message);
				}
			}
		}
	}
}
