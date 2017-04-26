using System;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.EventSyncInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class EventSyncInterface_iOS : EventSyncInterface
	{
		public EventSyncInterface_iOS()
		{
		}

		public void StartEventSyncing(Event eventReference)
		{
			
		}

		public void StopEventSyncing(Event eventReference)
		{
			
		}

		public void UploadNewImageLowRes(EventImage image)
		{
			
		}

		public void UpdateImageAnnotation(EventImage image)
		{
			
		}
	}
}
