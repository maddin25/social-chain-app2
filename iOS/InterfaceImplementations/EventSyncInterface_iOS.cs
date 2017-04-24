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

		public void StartEventSyncing(Event eventReference)
		{
			throw new NotImplementedException();
		}

		public void StopEventSyncing(Event eventReference)
		{
			throw new NotImplementedException();
		}

		public void UploadNewImageLowRes(EventImage image)
		{
			throw new NotImplementedException();
		}

		public void UpdateImageAnnotation(EventImage image)
		{
			throw new NotImplementedException();
		}
	}
}
