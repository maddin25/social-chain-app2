
using Android.App;
using Android.Support.CustomTabs;
using Xamarin.Auth;
using Xamarin.Forms;

namespace PartyTimeline.Droid.oauth
{
    public class OAuthLoginPresenter
    {
        public void Login(OAuth2Authenticator authenticator)
        {
            Android.Content.Intent i = null;
            //            Xamarin.Forms.Forms.Context.StartActivity(authenticator.GetUI(Xamarin.Forms.Forms.Context));
            Activity activity = (Activity)Forms.Context;
            System.Object ui_intent_as_object = authenticator.GetUI(activity);
            if (authenticator.IsUsingNativeUI == true)
            {
                // Add Android.Support.CustomTabs package 
                Android.Support.CustomTabs.CustomTabsIntent cti = null;
                var uiBuilder = (CustomTabsIntent.Builder) ui_intent_as_object;
                cti = (Android.Support.CustomTabs.CustomTabsIntent) uiBuilder.Build();
                i = cti.Intent;
            }
            else
            {
                i = (Android.Content.Intent)ui_intent_as_object;
            }
            Forms.Context.StartActivity(i);
        }
    }
}