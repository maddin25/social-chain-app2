using System.Collections.Generic;

using Newtonsoft.Json;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.EventListInterface_iOS))]
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

		public void WriteLocalEvent(Event eventReference)
		{

		}

		public void PushServerEvent(Event eventReference)
		{
			string serializedEvent = JsonConvert.SerializeObject(eventReference);
		}
	}
}
