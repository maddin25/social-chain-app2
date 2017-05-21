using System;
namespace PartyTimeline
{
	public class RestClientEvents : RestClient<Event>
	{
		public RestClientEvents() : base("events")
		{
		}
	}
}
