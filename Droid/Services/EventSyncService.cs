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

		private string id;

		public override void OnCreate()
		{
			base.OnCreate();
		}

		public override IBinder OnBind(Intent intent)
		{
			id = "default_id";
			SDebug.WriteLine($"{id}: OnBind");
			return new EventSyncBinder(this);
		}

		public override bool OnUnbind(Intent intent)
		{
			SDebug.WriteLine($"{id}: OnUnbind");
			return base.OnUnbind(intent);
		}

		public override void OnDestroy()
		{
			SDebug.WriteLine($"{id}: OnDestroy");
			base.OnDestroy();
		}
	}
}
