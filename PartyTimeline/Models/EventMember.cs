using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace PartyTimeline
{
	[Table("event_members")]
	public class EventMember : BaseModel
	{
		[Column("email_address"), NotNull, Unique]
		public string EmailAddress { get; set; }
		[Column("name"), NotNull]
		public string Name { get; set; }
		[Column("membership_role"), NotNull]
		public int Role { get; set; }

		public EventMember()
		{

		}

		public EventMember(DateTime dateCreated) : base(dateCreated)
		{
			
		}
	}
}
