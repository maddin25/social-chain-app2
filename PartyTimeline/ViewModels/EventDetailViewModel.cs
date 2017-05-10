using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using Xamarin.Forms;

namespace PartyTimeline.ViewModels
{
	public abstract class EventDetailViewModel : UIBindingHelper<EventImage>
	{
		protected Event _eventReference;
		public Command TakePhotoButtonCommand { get; set; }
		public Command PickPhotoButtonCommand { get; set; }
		public Event EventReference { get { return _eventReference; } set { _eventReference = value; } }

		public EventDetailViewModel()
		{
			TakePhotoButtonCommand = new Command(async () => await CheckCameraPermissions(TakePhoto));
			PickPhotoButtonCommand = new Command(async () => await CheckCameraPermissions(PickPhoto));
		}

		public override void Initialize()
		{
			base.Initialize();
			//DependencyService.Get<EventSyncInterface>().StartEventSyncing(EventReference);
		}

		public override void Deinitialize()
		{
			base.Deinitialize();
			//DependencyService.Get<EventSyncInterface>().StopEventSyncing(EventReference);
		}

		protected override async Task OnRefreshTriggered()
		{
			// TODO: only refresh from server here
			await EventService.INSTANCE.LoadEventImageList(EventReference);
		}

		private async Task CheckCameraPermissions(Action<bool> callBack)
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

		private async Task CheckStoragePermissions(Action<bool> callBack)
		{
			bool result = false;
			var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
			// TODO: use this method
		}

		private async void TakePhoto(bool permissionResult)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				Debug.WriteLine("Trying to take a picture, but no camera available.");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				Directory = EventReference.Id.ToString(),
				PhotoSize = PhotoSize.Full,
				CompressionQuality = 92
			});

			if (file == null)
			{
				return;
			}

			Debug.WriteLine($"Photo file Location: {file.Path}");

			AddEventImage(file);
		}

		private async void PickPhoto(bool permissionResult)
		{
			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				Debug.WriteLine("Photos Not Supported: Permission not granted to photos.");
				return;
			}
			MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Full
			});

			if (file == null)
			{
				return;
			}
			// TODO copy this image to the app memory

			AddEventImage(file);
		}

		private void AddEventImage(MediaFile file)
		{
			/** TODO: exif information is not contained in the images that are produced by Plugin.Media. However a fix
			 *	pull request is already on it's way: https://github.com/jamesmontemagno/MediaPlugin/pull/207/commits
			 */
			string pathNormal = file.Path;
			string pathSmall = Path.Combine(
				Path.GetDirectoryName(pathNormal),
				Path.GetFileNameWithoutExtension(pathNormal) + "small" + Path.GetExtension(pathNormal)
			);
			if (!DependencyService.Get<SystemInterface>().CompressImage(file.GetStream(), pathNormal, pathSmall))
			{
				Debug.WriteLine("Failed compressing image");
			}

			EventImage newEventImage = new EventImage(DateTime.Now)
			{
				Path = pathNormal,
				PathSmall = pathSmall,
				EventMemberId = SessionInformationProvider.INSTANCE.CurrentUserEventMember.Id,
				EventId = EventReference.Id,
				DateTaken = DateTime.Now
			};
			EventService.INSTANCE.AddImage(newEventImage);
		}

		// TODO maybe store every picture in the Album, read https://github.com/jamesmontemagno/MediaPlugin
	}
}
