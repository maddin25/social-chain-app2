using System;
using System.Diagnostics;
using System.Windows.Input;

namespace PartyTimeLineWebServices.Models
{
	public class EventImage
	{
	    public int Id { get; set; }
		public DateTime DateTaken { get; set; }
		public string ShortAnnotation { get; set; }
		public string LongAnnotation { get; set; }
		public string URI { get; set; }
        public object Navigation { get; private set; }

		public EventImage()
		{
		}
	}
}
