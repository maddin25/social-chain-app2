using System;
using System.Collections.Generic;

namespace PartyTimeline
{
	public static class EventMembershipRoles
	{
		public enum ROLES { Administrator, Moderator, Contributor, Viewer };

		public static int RoleIdMin = 0;
		public static int RoleIdMax = 3;

		public static string RoleDescription(ROLES role)
		{
			switch (role)
			{
				case ROLES.Administrator:
					return Resources.AppResources.EventMembershipDescriptionAdministrator;
				case ROLES.Moderator:
					return Resources.AppResources.EventMembershipDescriptionModerator;
				case ROLES.Contributor:
					return Resources.AppResources.EventMembershipDescriptionContributor;
				case ROLES.Viewer:
					return Resources.AppResources.EventMembershipDescriptionContributor;
				default:
					throw new ArgumentException($"Role {role.ToString()} not known");
			}
		}

		public static int RoleId(ROLES role)
		{
			switch (role)
			{
				case ROLES.Administrator:
					return 0;
				case ROLES.Moderator:
					return 1;
				case ROLES.Contributor:
					return 2;
				case ROLES.Viewer:
					return 3;
				default:
					return -1;
			}
		}
	}
}
