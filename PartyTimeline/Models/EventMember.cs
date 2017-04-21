using System.Collections.Generic;

namespace PartyTimeline
{
	public enum ROLES { Administrator, Moderator, Contributor, Viewer };

	public class EventMember
	{
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
		}

		public string Id { get; set; }
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FacebookToken { get; set; }
		public int Role { get; set; }
	}
}
