using System;

namespace PartyTimeline
{
	public class BaseModel
	{
		public long Id { get; set; }
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

		public override bool Equals(object obj)
		{
			if (!(obj is BaseModel))
			{
				return false;
			}
			return ((BaseModel)obj).Id == this.Id;
		}

		public override int GetHashCode()
		{
			return (int) Id;
		}

		public bool IsNewerThan(BaseModel other)
		{
			return other.DateLastModified > this.DateLastModified;
		}
	}
}
