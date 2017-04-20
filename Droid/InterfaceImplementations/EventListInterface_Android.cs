using System.Collections.Generic;
using SDebug = System.Diagnostics.Debug;

using Newtonsoft.Json;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.EventListInterface_Android))]
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

		public void WriteLocalEvent(ref Event eventReference)
		{

		}

		public void PushServerEvent(ref Event eventReference)
		{
			string serializedEvent = JsonConvert.SerializeObject(eventReference);
			SDebug.WriteLine($"Serialized event: {serializedEvent}");
		}
	}
}
