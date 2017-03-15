using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventListPage : ContentPage
	{
		public EventListPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			ObservableCollection<Event> events = new ObservableCollection<Event>();

			events.Add(new Event{ Name="Development of cool app", NrPictures=10, NrContributors=4 });
			events.Add(new Event{ Name="Call regarding future", NrPictures=2, NrContributors=2 });
			events.Add(new Event{ Name="Drinking a lot of beer", NrPictures=3241, NrContributors=4 });

			eventListView.ItemsSource = events;
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
