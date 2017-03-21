using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content.PM;
using Android.OS;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace PartyTimeline.Droid
{
	[Activity(Label = "PartyTimeline.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity  // XFormsApplicationDroid
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			#region Resolver Init
			SimpleContainer container = new SimpleContainer();
			container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
			container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
			container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

			Resolver.SetResolver(container.GetResolver());
			#endregion

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());
		}
	}
}
