using System;

namespace PartyTimeline
{
	public class BaseModel
	{
		public DateTime DateCreated { get; private set; }
		public DateTime DateLastModified { get; set; }

		public BaseModel()
		{

		}

		public BaseModel(DateTime dateCreated)
		{
			SetDateCreated(dateCreated);
		}

		public void SetDateCreated(DateTime dateCreated)
		{
			DateCreated = dateCreated;
			DateLastModified = dateCreated;
		}
	}
}
