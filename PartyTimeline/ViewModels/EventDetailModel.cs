using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using PartyTimeline.Services;

using Xamarin.Forms;

namespace PartyTimeline
{
	public class EventDetailModel
	{
		protected Event _eventReference;
		public Command TakePhotoButtonCommand { get; set; }
		public Command PickPhotoButtonCommand { get; set; }

		public Event EventReference { get { return _eventReference; } set { _eventReference = value; } }

		public EventDetailModel()
		{
			TakePhotoButtonCommand = new Command(async () => await CheckCameraPermissions(TakePhoto));
			PickPhotoButtonCommand = new Command(async () => await CheckCameraPermissions(PickPhoto));
		}

		public void Initialize()
		{
			DependencyService.Get<EventSyncInterface>().StartEventSyncing(EventReference);
			EventService.INSTANCE.QueryLocalEventImageList(EventReference);
		}

		public void Deinitialize()
		{
			DependencyService.Get<EventSyncInterface>().StopEventSyncing(EventReference);
			EventService.INSTANCE.PropertyChanged -= OnEventServicePropertyChanged;
		}

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
			{
				return;
			}

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
			{
				return;
			}

			AddEventImage(file.Path);
		}

		public void AddEventImage(String path)
		{
			EventImage newEventImage = new EventImage(DateTime.Now) { URI = path };
			EventService.INSTANCE.AddImageToEvent(newEventImage, EventReference);
		}

		public void OnEventServicePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == EventReference.Name)
			{
				Debug.WriteLine($"The referenced event {EventReference.Name} has been updated");
			}
			Debug.WriteLine($"Called {nameof(OnEventServicePropertyChanged)} of {this.GetType().Name}");
		}
	}
}
