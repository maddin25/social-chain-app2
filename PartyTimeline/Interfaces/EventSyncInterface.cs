using System;

using PartyTimeline;

public interface EventSyncInterface
{
	void StartEventSyncing(ref Event eventReference);
	void StopEventSyncing(ref Event eventReference);
}
