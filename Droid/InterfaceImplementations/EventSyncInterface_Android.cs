using SDebug = System.Diagnostics.Debug;

using Android.Content;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.EventSyncInterface_Android))]
namespace PartyTimeline.Droid
{
	public class EventSyncInterface_Android : EventSyncInterface
	{
		private EventSyncServiceConnection eventSyncingConnection;

		public EventSyncInterface_Android()
		{
		}

		public void StartEventSyncing(ref Event eventReference)
		{
			SDebug.WriteLine($"Starting syncing service for event {eventReference.Name}");

			Context context = Android.App.Application.Context;
			Intent startEventSyncingIntent = new Intent(context, typeof(EventSyncService));

			eventSyncingConnection = new EventSyncServiceConnection(context);

			context.BindService(startEventSyncingIntent, eventSyncingConnection, Bind.AutoCreate);
		}

		public void StopEventSyncing(ref Event eventReference)
		{
			SDebug.WriteLine($"Stopping syncing service for event {eventReference.Name}");
		}

		public void UploadNewImageLowRes(ref EventImage image)
		{
			eventSyncingConnection.Binder.Service.UploadNewImageLowRes(ref image);
		}

		public void UpdateImageAnnotation(ref EventImage image)
		{

		}
	}
}
