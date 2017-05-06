using System;
using System.ComponentModel;
using System.Diagnostics;
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

		public EventDetailViewModel(ListView refreshableListView) : base(refreshableListView)
		{
			TakePhotoButtonCommand = new Command(async () => await CheckCameraPermissions(TakePhoto));
			PickPhotoButtonCommand = new Command(async () => await CheckCameraPermissions(PickPhoto));
		}

		public override void OnAppearing()
		{
			base.OnAppearing();
			DependencyService.Get<EventSyncInterface>().StartEventSyncing(EventReference);
			RefreshListCommand.Execute(null);
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			DependencyService.Get<EventSyncInterface>().StopEventSyncing(EventReference);
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
				AllowCropping = true,
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
				PhotoSize = PhotoSize.Full,
				CompressionQuality = 92
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
			EventImage newEventImage = new EventImage(DateTime.Now)
			{
				Path = file.Path,
				EventMemberId = SessionInformationProvider.INSTANCE.CurrentUserEventMember.Id,
				EventId = EventReference.Id,
				DateTaken = DateTime.Now
			};
			EventService.INSTANCE.AddImage(newEventImage);
		}

		public void OnEventServicePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == EventReference.Name)
			{
				Debug.WriteLine($"The referenced event {EventReference.Name} has been updated");
			}
			Debug.WriteLine($"Called {nameof(OnEventServicePropertyChanged)} of {this.GetType().Name}");
		}

		protected override async Task OnRefreshTriggered()
		{
			await EventService.INSTANCE.QueryLocalEventImageList(EventReference);
		}

		// TODO maybe store every picture in the Album, read https://github.com/jamesmontemagno/MediaPlugin
	}
}
