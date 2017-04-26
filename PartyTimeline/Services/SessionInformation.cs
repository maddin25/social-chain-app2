using System;
namespace PartyTimeline
{
	public class SessionInformation
	{
		private static SessionInformation _instance;

		public EventMember CurrentUser { get; set; }

		public static SessionInformation INSTANCE
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SessionInformation();
				}
				return _instance;
			}
			set { _instance = value; }
		}

		private SessionInformation()
		{
			CurrentUser = new EventMember(DateTime.Now)
			{
				Id = 1,
				EmailAddress = "mailto@martin-patz.de",
				FirstName = "Martin",
				LastName = "Patz",
				Role = EventMember.RolesIds[ROLES.Administrator]
			};
		}
	}
}
