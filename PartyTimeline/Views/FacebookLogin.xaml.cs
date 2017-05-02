using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class FacebookLogin : ContentPage
	{
		public FacebookLogin()
		{
			List<string> requestParts = new List<string>()
			{
				"client_id=1632106426819143",
				"redirect_uri=https://www.facebook.com/connect/login_success.html",
				"scope=email,user_events",
				"response_type=token"
			};
			InitializeComponent();
			FacebookLoginWebview.Source = "https://www.facebook.com/v2.9/dialog/oauth?" + string.Join("&", requestParts);
			FacebookLoginWebview.PropertyChanged += FacebookLoginWebview_PropertyChanged;
		}

		void FacebookLoginWebview_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Source")
			{
				var contentWebView = sender as WebView;
				var o = contentWebView.Source as UrlWebViewSource;
				Debug.WriteLine("New source: " + o.Url);
			}
		}
	}
}