using System;
namespace PartyTimeline
{
	public enum SyncServices
	{
		EventList,
		EventDetails
	}

	public class SyncState : EventArgs
	{
		public bool EventListSyncing { get; set; }
		public bool EventDetailsSyncing { get; set; }
		public long EventIdSyncing { get; set; }

		public override string ToString()
		{
			return string.Format("[SyncState: EventListSyncing={0}, EventDetailsSyncing={1}]", EventListSyncing, EventDetailsSyncing, EventIdSyncing);
		}
	}
}
