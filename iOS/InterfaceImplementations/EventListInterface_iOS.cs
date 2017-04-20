using System.Collections.Generic;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.EventSyncInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class EventListInterface_iOS : EventListInterface
	{
		public List<Event> ReadLocalEvents()
		{
			return null;
		}

		public List<Event> PollServerEventList()
		{
			return null;
		}
	}
}
