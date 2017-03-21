using Foundation;
using UIKit;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace PartyTimeline.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			#region Resolver Init
			SimpleContainer container = new SimpleContainer();
			container.Register<IDevice>(t => AppleDevice.CurrentDevice);
			container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
			container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

			Resolver.SetResolver(container.GetResolver());
			#endregion

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
