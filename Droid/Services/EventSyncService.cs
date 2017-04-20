using SDebug = System.Diagnostics.Debug;

using Android.App;
using Android.Content;
using Android.OS;

namespace PartyTimeline.Droid
{
	[Service]
	public class EventSyncService : Service
	{
		static readonly string TAG = typeof(EventSyncService).FullName;

		public override void OnCreate()
		{
			base.OnCreate();
		}

		public override IBinder OnBind(Intent intent)
		{
			SDebug.WriteLine($"{TAG}: OnBind");
			return new EventSyncBinder(this);
		}

		public override bool OnUnbind(Intent intent)
		{
			SDebug.WriteLine($"{TAG}: OnUnbind");
			return base.OnUnbind(intent);
		}

		public override void OnDestroy()
		{
			SDebug.WriteLine($"{TAG}: OnDestroy");
			base.OnDestroy();
		}

		public void UploadNewImageLowRes(ref EventImage image)
		{
			SDebug.WriteLine($"Uploading image {image.URI} to the server");
		}

		public void UpdateImageAnnotation(ref EventImage image)
		{
			SDebug.WriteLine($"Updating annotation of image {image.URI}");
		}
	}
}
