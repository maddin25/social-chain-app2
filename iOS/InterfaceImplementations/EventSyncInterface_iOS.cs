using System;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.SystemInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class EventSyncInterface_iOS : EventSyncInterface
	{
		public EventSyncInterface_iOS()
		{
		}

		public void StartEventSyncing(ref Event eventReference)
		{
			throw new NotImplementedException();
		}

		public void StopEventSyncing(ref Event eventReference)
		{
			throw new NotImplementedException();
		}
	}
}
