using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventListPage : ContentPage
	{
		ObservableCollection<Event> events;

		public EventListPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			events = new ObservableCollection<Event>();

			events.Add(new Event());
			events.Add(new Event{ Name="Alone at Home" });
			events.Add(new Event{ Name="Development of cool app" });
			events.Add(new Event{ Name="Call regarding future" });
			events.Add(new Event{ Name="Drinking a lot of beer" });

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

		void Event_Tapped(object sender, ItemTappedEventArgs e)
		{
			var selectedEvent = e.Item as Event;
			var indexOfSelectedEvent = events.IndexOf(selectedEvent);
			Debug.WriteLine("Event nr {0} selected", indexOfSelectedEvent+1);
			Navigation.PushAsync(new EventPageThumbnails(ref selectedEvent));
		}
	}
}
