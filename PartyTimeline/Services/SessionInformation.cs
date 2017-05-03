using System;
using System.Diagnostics;

using Xamarin.Auth;

namespace PartyTimeline
{
	public class SessionInformation
	{

		private static SessionInformation _instance;

		public Account CurrentUser { get; set; }

		public long UserId
		{
			get
			{
				lock (CurrentUser)
				{
					if (CurrentUser.Properties.ContainsKey(FacebookAccountProperties.Id))
					{
						return long.Parse(CurrentUser.Properties[FacebookAccountProperties.Id]);
					}
					else
					{
						throw new InvalidOperationException("The current user's ID is not known");
					}
				}
			}
		}

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
			CurrentUser = account;
			UpdateCurrentUser(account);
		}

		public void UpdateCurrentUser(Account account)
		{
			lock (CurrentUser)
			{
				if (account.Properties.ContainsKey(FacebookAccountProperties.Id))
				{
					CurrentUser.Properties[FacebookAccountProperties.Id] = account.Properties[FacebookAccountProperties.Id];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.Name))
				{
					CurrentUser.Properties[FacebookAccountProperties.Name] = account.Properties[FacebookAccountProperties.Name];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.EMail))
				{
					CurrentUser.Properties[FacebookAccountProperties.EMail] = account.Properties[FacebookAccountProperties.EMail];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.AccessToken))
				{
					CurrentUser.Properties[FacebookAccountProperties.AccessToken] = account.Properties[FacebookAccountProperties.AccessToken];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
				{
					CurrentUser.Properties[FacebookAccountProperties.ExpiresOn] = account.Properties[FacebookAccountProperties.ExpiresOn];
				}
			}
		}

		private SessionInformation()
		{
			CurrentUser = new Account();
		}
	}
}
