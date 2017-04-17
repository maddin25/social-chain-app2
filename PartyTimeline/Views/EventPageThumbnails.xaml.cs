using PartyTimeline.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Title = EventReference.Name;
            SetupUI();
        }

        void SetupUI()
		{
			EventImageList.FlowItemsSource = EventReference.Images;
            EventImageList.FlowItemTapped += FlowItemTapped_Command;
            EventNameLabel.Text = EventReference.Name;
        }

        void FlowItemTapped_Command(object sender, ItemTappedEventArgs args)
        {
            Debug.WriteLine(args.ToString());
            EventImage typed = (EventImage) args.Item;
            Event _event = EventReference;
            Navigation.PushAsync(new ImageGalleryPage(ref typed, ref _event));
        }

    }
}
