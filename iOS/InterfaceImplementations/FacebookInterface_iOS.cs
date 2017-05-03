using System;
using System.Diagnostics;

using UIKit;

using Xamarin.Forms;
using Xamarin.Auth;

[assembly: Dependency(typeof(PartyTimeline.iOS.FacebookInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class FacebookInterface_iOS : FacebookInterface
	{
		public void LaunchLogin(Authenticator authenticator)
		{
			try
			{
				// native: SFSafariViewController
				UIViewController viewController = (UIViewController)authenticator.GetUI();
				UIViewController rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;

				//UIViewController rootViewController = window.RootViewController;
				rootViewController.PresentViewController(viewController, false, null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
