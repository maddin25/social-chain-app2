using System;
using System.ComponentModel;

using SQLite;

using Xamarin.Forms;

using Newtonsoft.Json;

namespace PartyTimeline
{
	[Table("BaseModel")]
	public class BaseModel : IComparable<BaseModel>, INotifyPropertyChanged
	{
		private static Random idGenerator = new Random(DateTime.Now.Millisecond);

		[JsonProperty("id", Required = Required.Always)]
		[PrimaryKey, Column("_id")]
		public long Id { get; set; }

		[JsonIgnore]
		[Column("date_created"), NotNull]
		public DateTime DateCreated { get; set; }

		[JsonProperty("updated_time", Required = Required.Always)]
		[Column("date_modified"), NotNull]
		public DateTime DateLastModified { get; set; }

		[JsonIgnore]
		[Ignore]
		public Command OnDelete { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public BaseModel()
		{
			Initialize();
		}

		public BaseModel(DateTime dateCreated)
		{
			Initialize();
			SetDateCreated(dateCreated);
		}

		public void SetDateCreated(DateTime dateCreated)
		{
			DateCreated = dateCreated;
			DateLastModified = dateCreated;
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
			return (int)Id;
		}

		/// <summary>
		/// Compares other with this instance using <see cref="BaseModel.SortDateCreatedDescending"/>.
		/// </summary>
		/// <returns>A comparison value</returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int CompareTo(BaseModel other)
		{
			return this.DateLastModified.Subtract(other.DateLastModified).Milliseconds;
		}

		// TODO: remove this
		public bool ModifiedAfter(BaseModel other)
		{
			return CompareTo(other) > 0;
		}

		public virtual void Update(BaseModel update)
		{
			DateLastModified = update.DateLastModified;
		}

		public virtual void Delete()
		{

		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SetRandomId()
		{
			Id = idGenerator.Next();
		}

		private void Initialize()
		{
			SetRandomId();
			OnDelete = new Command(Delete); // dummy
		}
	}
}
