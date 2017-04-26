using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace PartyTimeline
{
	public enum ROLES { Administrator, Moderator, Contributor, Viewer };

	[Table("event_members")]
	public class EventMember : BaseModel
	{
		public static int ROLE_ID_MIN = 0;
		public static int ROLE_ID_MAX = 3;

		public static Dictionary<ROLES, string> RoleDescriptions = new Dictionary<ROLES, string>
		{
			{ROLES.Administrator, "Administrator"},
			{ROLES.Moderator, "Moderator"},
			{ROLES.Contributor, "Contributor, can upload content and change existing content"},
			{ROLES.Viewer, "Viewer, only read access"}
		};

		public static Dictionary<ROLES, int> RolesIds = new Dictionary<ROLES, int>
		{
			{ROLES.Administrator, 0},
			{ROLES.Moderator, 1},
			{ROLES.Contributor, 2},
			{ROLES.Viewer, 3}
		};

		// TODO: how to create unique EventMember ID?
		[Column("email_address"), NotNull, Unique]
		public string EmailAddress { get; set; }
		[Column("first_name"), NotNull]
		public string FirstName { get; set; }
		[Column("last_name"), NotNull]
		public string LastName { get; set; }
		[Column("facebook_token"), MaxLength(256), Unique]
		public string FacebookToken { get; set; }
		[Ignore]
		public int Role { get; set; }

		public EventMember()
		{
			
		}

		public EventMember(DateTime dateCreated) : base(dateCreated)
		{
			
		}
	}
}
