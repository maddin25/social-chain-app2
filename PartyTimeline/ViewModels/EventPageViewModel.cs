using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;

using PartyTimeline.Views;

namespace PartyTimeline
{
	public class EventPageViewModel : INotifyPropertyChanged
	{
		Event _eventReference;
		EventImage _selectedImage;

		public Event EventReference { get { return _eventReference; } set { _eventReference = value; } }
		public EventImage SelectedImage
		{
			get { return _selectedImage; }
			set
			{
				_selectedImage = value;
				if (_selectedImage != null)
				{
					Debug.WriteLine($"Selected image {_selectedImage.URI}");
					Application.Current.MainPage.Navigation.PushAsync(new ImageGalleryPage(ref _selectedImage, ref _eventReference));
				}
			}
		}

		public Command<ItemTappedEventArgs> FlowItemTappedCommand { get; }
		public Command TakePhotoButtonCommand { get; set; }
		public Command PickVideoButtonCommand { get; set; }
		public Command PickPhotoButtonCommand { get; set; }
		public Command TakeVideoButtonCommand { get; set; }

		public EventPageViewModel(ref Event eventReference)
		{
			EventReference = eventReference;

			TakePhotoButtonCommand = new Command(async () => await CheckCameraPermissions(TakePhoto));
			TakeVideoButtonCommand = new Command(async () => await CheckCameraPermissions(TakeVideo));
			PickVideoButtonCommand = new Command(async () => await CheckCameraPermissions(PickVideo));
			PickPhotoButtonCommand = new Command(async () => await CheckCameraPermissions(PickPhoto));
			FlowItemTappedCommand = new Command<ItemTappedEventArgs>((args) =>
			{

			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		async Task CheckCameraPermissions(Action<bool> callBack)
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
				Debug.WriteLine("Permissions Denied: Unable to take photos.");
				//On iOS you may want to send your user to the settings screen.
				//CrossPermissions.Current.OpenAppSettings();
			}
			callBack(result);
		}

		private async void TakePhoto(bool permissionResult)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				Debug.WriteLine("Trying to take a picture, but no camera available.");
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

			Debug.WriteLine($"Photo file Location: {file.Path}");

			AddEventImage(file.Path);

			//            testImage.Source = ImageSource.FromStream(() =>
			//	        {
			//	            var stream = file.GetStream();
			//	            file.Dispose();
			//	            return stream;
			//	        });
		}



		private async void PickPhoto(bool permissionResult)
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				Debug.WriteLine("Photos Not Supported: Permission not granted to photos.");
				return;
			}
			var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions { PhotoSize = PhotoSize.Medium });

			if (file == null)
				return;

			AddEventImage(file.Path);
		}

		private async void TakeVideo(bool permissionResult)
		{
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
			{
				Debug.WriteLine("Trying to take a video, but no camera available.");
				return;
			}

			var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
			{
				Name = "video.mp4",
				Directory = "DefaultVideos",
			});

			if (file == null)
				return;

			Debug.WriteLine($"Video file Location: {file.Path}");
			// EventReference.AddEventImage(file.Path);
			file.Dispose();
		}



		private async void PickVideo(bool permissionResult)
		{
			if (!CrossMedia.Current.IsPickVideoSupported)
			{
				Debug.WriteLine("Videos Not Supported: Permission not granted to videos.");
				return;
			}
			var file = await CrossMedia.Current.PickVideoAsync();

			if (file == null)
				return;

			Debug.WriteLine($"Video file Location: {file.Path}");
			//EventReference.AddEventImage(file.Path);
			//file.Dispose(
		}

		public void AddEventImage(String path)
		{
			EventImage newEventImage = new EventImage(DateTime.Now);
			newEventImage.Caption = "Default Short Annotation";
			Random nrGenerator = new Random(DateTime.Now.Millisecond);
			newEventImage.Id = nrGenerator.Next();
			newEventImage.URI = path;

			EventReference.Images.Add(newEventImage);

			DependencyService.Get<EventSyncInterface>().UploadNewImageLowRes(ref newEventImage);
		}
	}
}
