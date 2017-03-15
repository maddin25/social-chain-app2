using System;
namespace PartyTimeline
{
	public class EventImage
	{
		public long ID { get; set; }
		public DateTime DateTaken { get; set; }
		public string ShortAnnotation { get; set; }
		public string LongAnnotation { get; set; }
		public string URI { get; set; }

		public EventImage()
		{
		}
	}
}
