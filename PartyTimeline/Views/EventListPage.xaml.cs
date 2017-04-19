using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Generic;
using CarouselView.FormsPlugin.Abstractions;
using PartyTimeline.Services;
using PartyTimeline.ViewModels;
using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventListPage : ContentPage
	{
		public EventListPage()
		{
			InitializeComponent();
			BindingContext = new EventListPageViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override bool OnBackButtonPressed()
		{
			Debug.WriteLine("Back pressed in EventListPage");
			if (Device.OS == TargetPlatform.Android)
			{
				DependencyService.Get<SystemInterface>().Close();
			}
			return true;
		}
	}
}
