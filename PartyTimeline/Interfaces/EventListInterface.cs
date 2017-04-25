using System.Collections.Generic;
using System.Threading.Tasks;

using PartyTimeline;

public interface EventListInterface
{
	List<Event> ReadLocalEvents();
	List<EventImage> ReadLocalEventImages(Event eventReference);
	List<Event> PollServerEventList();

	Task PushServerEvent(Event eventReference);
	Task WriteLocalEvent(Event eventReference);
	Task WriteLocalEventImage(EventImage image, Event eventReference);
}
