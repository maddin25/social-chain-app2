using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class LoginPage : ContentPage
	{

		public LoginPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			LabelForgotPassword.GestureRecognizers.Add(new TapGestureRecognizer((View obj) => ForgotPassword_Clicked(obj)));
		}

		void Username_Completed(object sender, EventArgs e)
		{
			Debug.WriteLine("Username completed");
		}

		void Password_Completed(object sender, EventArgs e)
		{
			Debug.WriteLine("Password completed");
		}

		void Username_TextChanged(object sender, TextChangedEventArgs e)
		{
			Debug.WriteLine("Username TextChanged");
		}

		void Password_TextChanged(object sender, TextChangedEventArgs e)
		{
			Debug.WriteLine("Password TextChanged");
		}

		void Login_Clicked(object sender, EventArgs e)
		{
			Debug.WriteLine("Login clicked");
			Navigation.PushAsync(new EventListPage());
		}

		void ForgotPassword_Clicked(View obj)
		{
			Debug.WriteLine("ForgotPassword clicked");
		}
	}
}
