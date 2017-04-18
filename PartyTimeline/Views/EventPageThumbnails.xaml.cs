using PartyTimeline.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Media;
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

	    private async void TakePhotoButton_OnClicked(object sender, EventArgs e)
	    {
	        await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
	        {
	            DisplayAlert("No Camera", ":( No camera available.", "OK");
	            return;
	        }

	        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
	        {
	            Directory = "Sample",
	            Name = "test.jpg"
	        });

	        if (file == null)
	            return;

	        await DisplayAlert("File Location", file.Path, "OK");

	        testImage.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();
	            return stream;
	        });
        }

	    private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
	    {
	        if (!CrossMedia.Current.IsPickPhotoSupported)
	        {
	            DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
	            return;
	        }
	        var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
	        {
	            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
	        });


	        if (file == null)
	            return;

	        testImage.Source = ImageSource.FromStream(() =>
	        {
	            var stream = file.GetStream();
	            file.Dispose();
	            return stream;
	        });
        }

	    private async void TakeVideoButton_OnClicked(object sender, EventArgs e)
	    {
	        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
	        {
	            DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
	            return;
	        }

	        var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
	        {
	            Name = "video.mp4",
	            Directory = "DefaultVideos",
	        });

	        if (file == null)
	            return;

	        DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

	        file.Dispose();
        }

	    private async void PickVideoButton_OnClicked(object sender, EventArgs e)
	    {
	        if (!CrossMedia.Current.IsPickVideoSupported)
	        {
	            DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
	            return;
	        }
	        var file = await CrossMedia.Current.PickVideoAsync();

	        if (file == null)
	            return;

	        DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
	        file.Dispose();
        }
	}
}
