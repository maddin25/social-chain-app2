using System;
using System.Diagnostics;

using Xamarin.Forms;

using PartyTimeline.Views;

namespace PartyTimeline.ViewModels
{
	public class EventThumbnailsViewModel : EventDetailViewModel
	{
		public EventThumbnailsViewModel(ref Event eventReference, ListView refreshableListView) : base(refreshableListView)
		{
			EventReference = eventReference;
		}

		protected override void OnSelect(ref EventImage element)
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventGalleryPage(ref element, ref _eventReference));
		}
	}
}
