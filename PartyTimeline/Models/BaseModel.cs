using System;

namespace PartyTimeline
{
	public class BaseModel : IComparable<BaseModel>
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

		/// <summary>
		/// Compares other with this instance using <see cref="BaseModel.SortDateCreatedDescending"/>.
		/// </summary>
		/// <returns>A comparison value</returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int CompareTo(BaseModel other)
		{
			return SortDateCreatedDescending(other);
		}

		/// <summary>
		/// IComparable interface implementation sorting according to DateCreated in descending order. That is, if other
		/// is newer than this instance, a positive value is returned. If other is older, a negative value is returned.
		/// Otherwise 0 is returned.
		/// </summary>
		/// <returns>A comparison value (time difference in milliseconds)</returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int SortDateCreatedDescending(BaseModel other)
		{
			return (int) other.DateLastModified.Subtract(this.DateLastModified).TotalMilliseconds;
		}

		/// <summary>
		/// IComparable interface implementation sorting according to DateLastModified in descending order. That is, if
		/// other is newer than this instance, a positive value is returned. If other is older, a negative value is
		/// returned. Otherwise 0 is returned.
		/// </summary>
		/// <returns>A comparison value (time difference in milliseconds)</returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int SortDateLastModifiedDescending(BaseModel other)
		{
			return (int) other.DateLastModified.Subtract(this.DateLastModified).TotalMilliseconds;
		}
	}
}
