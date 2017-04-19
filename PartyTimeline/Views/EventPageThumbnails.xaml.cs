using PartyTimeline.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace PartyTimeline
{
	public partial class EventPageThumbnails : ContentPage
	{
		public Event EventReference { get; set; }

		public EventPageThumbnails()
		{
			InitializeComponent();
		}

		public EventPageThumbnails(ref Event eventReference)
		{
			InitializeComponent();
			BindingContext = new EventPageViewModel(eventReference);
			EventReference = eventReference;
			EventImageList.FlowItemTapped += FlowItemTapped_Command;
		}

		void FlowItemTapped_Command(object sender, ItemTappedEventArgs args)
		{
			Debug.WriteLine(args.ToString());
			EventImage typed = (EventImage)args.Item;
			Event _event = EventReference;
			Navigation.PushAsync(new ImageGalleryPage(ref typed, ref _event));
		}

		private async void CheckCameraPermissions(Action<bool> callBack)
		{
			bool result = false;

			var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
			var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

			if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
			{
				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
				cameraStatus = results[Permission.Camera];
				storageStatus = results[Permission.Storage];
			}

			if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
			{
				result = true;
			}
			else
			{
				result = false;
				await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
				//On iOS you may want to send your user to the settings screen.
				//CrossPermissions.Current.OpenAppSettings();
			}
			callBack(result);
		}

		private void TakePhotoButton_OnClicked(object sender, EventArgs e)
		{
			CheckCameraPermissions(TakePhoto);
		}

		private async void TakePhoto(bool permissionResult)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", "No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				Directory = "Sample",
				Name = "test.jpg",
				PhotoSize = PhotoSize.Medium,
				SaveToAlbum = false,
				AllowCropping = true,
				CompressionQuality = 92
			});

			if (file == null)
				return;

			await DisplayAlert("File Location", file.Path, "OK");

			EventReference.AddEventImage(file.Path);

			//            testImage.Source = ImageSource.FromStream(() =>
			//	        {
			//	            var stream = file.GetStream();
			//	            file.Dispose();
			//	            return stream;
			//	        });
		}

		private void PickPhotoButton_OnClicked(object sender, EventArgs e)
		{
			CheckCameraPermissions(PickPhoto);
		}

		private async void PickPhoto(bool permissionResult)
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				await DisplayAlert("Photos Not Supported", "Permission not granted to photos.", "OK");
				return;
			}
			var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
			{
				PhotoSize = PhotoSize.Medium
			});

			if (file == null)
				return;

			EventReference.AddEventImage(file.Path);
		}

		private void TakeVideoButton_OnClicked(object sender, EventArgs e)
		{
			CheckCameraPermissions(TakeVideo);
		}

		private async void TakeVideo(bool permissionResult)
		{
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
			{
				await DisplayAlert("No Camera", "No camera avaialble.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
			{
				Name = "video.mp4",
				Directory = "DefaultVideos",
			});

			if (file == null)
				return;

			await DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");
			// EventReference.AddEventImage(file.Path);
			file.Dispose();
		}

		private void PickVideoButton_OnClicked(object sender, EventArgs e)
		{
			CheckCameraPermissions(PickVideo);
		}

		private async void PickVideo(bool permissionResult)
		{
			if (!CrossMedia.Current.IsPickVideoSupported)
			{
				await DisplayAlert("Videos Not Supported", "Permission not granted to videos.", "OK");
				return;
			}
			var file = await CrossMedia.Current.PickVideoAsync();

			if (file == null)
				return;

			await DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
			//EventReference.AddEventImage(file.Path);
			//file.Dispose();
		}
	}
}
