using System;
using System.Diagnostics;

using Xamarin.Auth;

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

		public void SetCurrentUser(Account account)
		{
			CurrentUser = new EventMember(DateTime.Now);
			if (account.Properties.ContainsKey(FacebookAccountProperties.Id))
			{
				CurrentUser.Id = long.Parse(account.Properties[FacebookAccountProperties.Id]);
			}
			if (account.Properties.ContainsKey(FacebookAccountProperties.Name))
			{
				CurrentUser.Name = account.Properties[FacebookAccountProperties.Name];
			}
			if (account.Properties.ContainsKey(FacebookAccountProperties.EMail))
			{
				CurrentUser.EmailAddress = account.Properties[FacebookAccountProperties.EMail];
			}
			if (account.Properties.ContainsKey(FacebookAccountProperties.AccessToken))
			{
				CurrentUser.FacebookToken = account.Properties[FacebookAccountProperties.AccessToken];
			}
			if (account.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
			{
				CurrentUser.SessionExpirationDate = DateTime.FromFileTime(long.Parse(account.Properties[FacebookAccountProperties.ExpiresOn]));
			}
			// TODO: remove this later
			EventService.INSTANCE.AddEventMember(CurrentUser);
			Debug.WriteLine(account.ToString());
		}

		private SessionInformation()
		{
			CurrentUser = null;
		}
	}
}
