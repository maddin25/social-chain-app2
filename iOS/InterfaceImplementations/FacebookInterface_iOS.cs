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
		private UIViewController loginViewController;

		public void LaunchLogin(Authenticator authenticator)
		{
			try
			{
				// native: SFSafariViewController
				loginViewController = (UIViewController)authenticator.GetUI();
				UIViewController rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
				while (rootViewController.PresentedViewController != null)
				{
					rootViewController = rootViewController.PresentedViewController;
				}
				Device.BeginInvokeOnMainThread(() => rootViewController.PresentViewController(loginViewController, true, null));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public void CloseLogin()
		{
			loginViewController?.DismissViewController(true, null);
		}
	}
}
