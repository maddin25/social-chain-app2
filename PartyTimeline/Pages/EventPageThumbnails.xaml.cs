using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{
		public Event EventReference{ get; set; }

		public EventPageThumbnails()
		{
			InitializeComponent();
		}

		public EventPageThumbnails(ref Event eventReference)
		{
			InitializeComponent();
			EventReference = eventReference;
			SetupUI();
		}

		void SetupUI()
		{
			EventImageList.FlowItemsSource = EventReference.Images;
			EventNameLabel.Text = EventReference.Name;
		}
	}
}
