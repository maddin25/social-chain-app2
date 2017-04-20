using System.Collections.Generic;

using PartyTimeline;

public interface EventListInterface
{
	List<Event> ReadLocalEvents();
	List<Event> PollServerEventList();
	void WriteLocalEvent(Event eventReference);
	void PushServerEvent(Event eventReference);
}
