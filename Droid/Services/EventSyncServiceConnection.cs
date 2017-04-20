using SDebug = System.Diagnostics.Debug;

using Android.Content;
using Android.OS;

namespace PartyTimeline.Droid
{
	public class EventSyncServiceConnection : Java.Lang.Object, IServiceConnection
	{
		static readonly string TAG = typeof(EventSyncServiceConnection).FullName;

		public bool IsConnected { get; private set; }
		public EventSyncBinder Binder { get; private set; }

		public EventSyncServiceConnection(Context context)
		{
			IsConnected = false;
			Binder = null;
		}

		public void OnServiceConnected(ComponentName name, IBinder binder)
		{
			Binder = binder as EventSyncBinder;
			IsConnected = Binder != null;
			SDebug.WriteLine($"{TAG}: OnServiceConnected {name.ClassName}");
		}

		public void OnServiceDisconnected(ComponentName name)
		{
			SDebug.WriteLine($"{TAG}: OnServiceDisconnected {name.ClassName}");
			IsConnected = false;
			Binder = null;
		}
	}
}
