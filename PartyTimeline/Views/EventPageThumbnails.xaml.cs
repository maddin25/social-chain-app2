using PartyTimeline.Views;
using System;
using System.Collections.Generic;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{

		public EventPageThumbnails()
		{
			InitializeComponent();
		}

		public EventPageThumbnails(ref Event eventReference)
		{
			InitializeComponent();
			BindingContext = new EventPageViewModel(ref eventReference);
		}

	}
}
