using System;
using SDebug = System.Diagnostics.Debug;

using PartyTimeline;
using PartyTimeline.Droid;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer))]
namespace PartyTimeline.Droid
{
	public class FacebookLoginPageRenderer : PageRenderer
	{
		FacebookLoginPage facebookLoginPage
		{
			get
			{
				return Element as FacebookLoginPage;
			}
		}

		protected override void OnVisibilityChanged(Android.Views.View changedView, Android.Views.ViewStates visibility)
		{
			base.OnVisibilityChanged(changedView, visibility);
			SDebug.WriteLine($"{nameof(FacebookLoginPage)} became {visibility}");

			if (visibility == Android.Views.ViewStates.Visible)
			{
				if (!facebookLoginPage.InhibitAutomaticPrompt)
				{
					facebookLoginPage.IsAuthorizing = true;
					SessionInformationProvider.INSTANCE.AuthenticateUserIfRequired();
				}
			}
		}
	}
}
