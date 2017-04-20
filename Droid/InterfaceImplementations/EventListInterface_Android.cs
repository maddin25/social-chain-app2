using System.Collections.Generic;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.EventSyncInterface_Android))]
namespace PartyTimeline.Droid
{
	public class EventListInterface_Android : EventListInterface
	{
		public EventListInterface_Android()
		{

		}

		public List<Event> ReadLocalEvents()
		{
			return null;
		}

		public List<Event> PollServerEventList()
		{
			return null;
		}

		public void WriteLocalEvent(Event eventReference)
		{

		}

		public void PushServerEvent(Event eventReference)
		{

		}
	}
}
