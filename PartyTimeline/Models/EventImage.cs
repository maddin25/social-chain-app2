using System;

namespace PartyTimeline
{
	public class EventImage : BaseModel
	{
		// TODO: how to create unique EventImage ID?
		public string Caption { get; set; }
		public string URI { get; set; }

		public EventImage(DateTime dateCreated) : base(dateCreated)
		{
			Caption = string.Empty;
		}

		public EventImage()
		{
			Caption = string.Empty;
		}
	}
}
