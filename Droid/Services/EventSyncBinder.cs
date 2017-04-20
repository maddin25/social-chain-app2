using SDebug = System.Diagnostics.Debug;

using Android.OS;

namespace PartyTimeline.Droid
{
	public class EventSyncBinder : Binder
	{
		public EventSyncService Service { get; private set; }

		public EventSyncBinder(EventSyncService service)
		{
			this.Service = service;
		}
	}
}
