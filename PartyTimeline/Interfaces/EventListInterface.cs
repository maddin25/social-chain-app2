using System.Collections.Generic;

using PartyTimeline;

public interface EventListInterface
{
	List<Event> ReadLocalEvents();
	List<Event> PollServerEventList();
	void PushServerEvent(Event eventReference);
	void WriteLocalEvent(Event eventReference);
	void WriteLocalEventImage(EventImage image, Event eventReference);
}
