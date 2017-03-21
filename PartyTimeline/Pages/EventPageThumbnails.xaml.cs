using System;
using System.Diagnostics;

using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{
		CameraViewModel cameraOps;

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
			cameraOps = new CameraViewModel();
		}

		async void TakePhoto_Clicked(object sender, EventArgs e)
		{
			await cameraOps.TakePicture();
			Debug.WriteLine("Status: {0}", cameraOps.Status);
		}
	}
}
