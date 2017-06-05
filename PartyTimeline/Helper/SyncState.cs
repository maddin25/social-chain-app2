using System;

using Newtonsoft.Json;

namespace PartyTimeline
{
	public enum SyncServices
	{
		EventList,
		EventDetails
	}

	public class SyncState : EventArgs
	{
		[JsonProperty("event_list_syncing")]
		public bool EventListSyncing { get; set; }
		[JsonProperty("event_details_syncing")]
		public bool EventDetailsSyncing { get; set; }
		[JsonProperty("event_id_syncing")]
		public long EventIdSyncing { get; set; }

		public override string ToString()
		{
            return JsonConvert.SerializeObject(this);
		}
	}
}
