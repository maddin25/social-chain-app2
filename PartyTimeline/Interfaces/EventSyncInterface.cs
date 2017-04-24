
using PartyTimeline;

public interface EventSyncInterface
{
	void StartEventSyncing(Event eventReference);
	void StopEventSyncing(Event eventReference);

	void UploadNewImageLowRes(EventImage image);
	void UpdateImageAnnotation(EventImage image);
}
