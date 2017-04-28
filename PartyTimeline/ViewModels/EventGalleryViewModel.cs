using System;
using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.Views;

namespace PartyTimeline.ViewModels
{
	public class EventPageViewModel : EventDetailModel
	{
		public EventPageViewModel(ref Event eventReference, ListView refreshableListView) : base(refreshableListView)
		{
			EventReference = eventReference;
		}

		protected override void OnSelect(ref EventImage eventImage)
		{
			Application.Current.MainPage.Navigation.PushAsync(new ImageGalleryPage(ref eventImage, ref _eventReference));
		}
	}
}
