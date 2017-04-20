using System;

using PartyTimeline;

public interface EventSyncInterface
{
	void StartEventSyncing(ref Event eventReference);
	void StopEventSyncing(ref Event eventReference);

	void UploadNewImageLowRes(ref EventImage image);
	void UpdateImageAnnotation(ref EventImage image);
}
