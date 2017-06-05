using System;

using PartyTimeline;
using PartyTimeline.iOS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginPageRenderer_iOS))]
namespace PartyTimeline.iOS
{
	public class FacebookLoginPageRenderer_iOS : PageRenderer
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
				facebookLoginPage.ManualLoginCommand.Execute(null);
			}
		}
	}
}
