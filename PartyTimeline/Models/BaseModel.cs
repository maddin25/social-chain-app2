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

		public const string UidId = "id";
        public const string UidDateCreated = "date_created";
        public const string UidDateModified = "date_modified";

        [JsonProperty(UidId, Required = Required.Always)]
		[PrimaryKey, Column("_id")]
		public long Id { get; set; }

		[JsonIgnore]
		[Column(UidDateCreated), NotNull]
		public DateTime DateCreated { get; set; }

		[JsonProperty(UidDateModified)]
		[Column(UidDateModified), NotNull]
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
		/// Compares other with this instance using <see cref="DateLastModified"/>.
		/// </summary>
		/// <returns>A positive value, if this instance was modified after <paramref name="other"/></returns>
		/// <param name="other">Other instance of a <see cref="BaseModel"/></param>
		public int CompareTo(BaseModel other)
		{
			return DateLastModified.Subtract(other.DateLastModified).Milliseconds;
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
