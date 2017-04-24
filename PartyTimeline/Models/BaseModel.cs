using System;

namespace PartyTimeline
{
	public class BaseModel
	{
		public DateTime DateCreated { get; set; }
		public DateTime DateLastModified { get; set; }

		public BaseModel()
		{

		}

		public BaseModel(DateTime dateCreated)
		{
			SetDate(dateCreated);
		}

		public void SetDate(DateTime date)
		{
			DateCreated = date;
			DateLastModified = date;
		}
	}
}
