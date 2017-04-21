using System;

namespace PartyTimeline
{
	public class EventImage
	{
		public long Id { get; set; }
		public DateTime DateTaken { get; set; }
		public DateTime DateLastModified { get; set; }
		public string Caption { get; set; }
		public string URI { get; set; }
	}
}
