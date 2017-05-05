using System;

using PartyTimeline;
using PartyTimeline.iOS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer))]
namespace PartyTimeline.iOS
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

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (!facebookLoginPage.InhibitAutomaticPrompt)
			{
				facebookLoginPage.IsAuthorizing = true;
				SessionInformationProvider.INSTANCE.AuthenticateUserIfRequired();
			}
		}
	}
}
