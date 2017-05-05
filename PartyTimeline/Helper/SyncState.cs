using System;
namespace PartyTimeline
{
	public enum SyncServices
	{
		EventList,
		EventImage,
		EventDetails
	}

	public class SyncState : EventArgs
	{
		public bool IsSyncing { get; set; }
		public SyncServices SyncService { get; set; }

		public override string ToString()
		{
			return string.Format("[SyncState: IsSyncing={0}, SyncService={1}]", IsSyncing, SyncService);
		}
	}
}
