using System;
using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;

using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Permissions;
using FFImageLoading.Forms.Droid;

namespace PartyTimeline.Droid
{
	[Activity(Label = "PartyTimeline.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override async void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			CachedImageRenderer.Init();
			UserDialogs.Init(this);
			base.OnCreate(bundle);
		    await CrossMedia.Current.Initialize();

            global::Xamarin.Forms.Forms.Init(this, bundle);

			Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
			//AndroidBug5497WorkaroundForXamarinAndroid.assistActivity(this);

			LoadApplication(new App());
		}
	    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
	    {
	        PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
	    }

    }
}
