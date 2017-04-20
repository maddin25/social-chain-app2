using System.Collections.Generic;

using PartyTimeline;

public interface EventListInterface
{
	List<Event> ReadLocalEvents();
	List<Event> PollServerEventList();
	void PushServerEvent(ref Event eventReference);
	void WriteLocalEvent(ref Event eventReference);
}
