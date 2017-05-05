using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Auth;

namespace PartyTimeline
{
	public class SessionInformationProvider
	{
		private Account _currentUser;
		private static SessionInformationProvider _instance;

		public static readonly string AppName = "PartyTimeline";

		public EventHandler SessionStateChanged;

		public Account CurrentUser
		{
			get
			{
				if (_currentUser == null)
				{
					_currentUser = ReadAccountFromAccountStore();
				}
				return _currentUser;
			}
			private set
			{
				_currentUser = value;
			}
		}

		// TODO: remove
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

		public static SessionInformationProvider INSTANCE
		{
			get
			{
				if (_instance == null)
				{
					_instance = new SessionInformationProvider();
				}
				return _instance;
			}
			set { _instance = value; }
		}

		public string GetUserProperty(string propertyName)
		{
			if (CurrentUser?.Properties.ContainsKey(propertyName) ?? false)
			{
				return CurrentUser.Properties[propertyName];
			}
			return null;
		}

		public void BeginSession(Account account)
		{
			AccountStore.Create().Save(account, AppName);
			CurrentUser = account;
			OnSessionStateChanged(new SessionState { IsAuthenticated = true });
		}

		public void EndSession()
		{
			AccountStore.Create().Delete(CurrentUser, AppName);
			CurrentUser = null;
			OnSessionStateChanged(new SessionState { IsAuthenticated = false });
		}

		public Task UpdateSession(Account account)
		{
			UpdateCurrentUser(account);
			return AccountStore.Create().SaveAsync(CurrentUser, AppName);
		}

		public bool ActiveSessionAvailable
		{
			get
			{
				if (CurrentUser == null)
				{
					return false;
				}
				if (!CurrentUser.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
				{
					Debug.WriteLine($"WARNING: Account {CurrentUser.Username} does not contain the {nameof(FacebookAccountProperties.ExpiresOn)} property");
					EndSession();
					return false;
				}
				DateTime expiresOn = DateTime.FromFileTime(long.Parse(CurrentUser.Properties[FacebookAccountProperties.ExpiresOn]));
				return expiresOn > DateTime.Now; // is true, if the Account token is not yet expired
			}
		}

		public void AuthenticateUserIfRequired()
		{
			if (!ActiveSessionAvailable)
			{
				FacebookClient fbCommunicator = new FacebookClient();
				fbCommunicator.Authorize((success) => OnSessionStateChanged(new SessionState { IsAuthenticated = success }));
			}
			else
			{
				OnSessionStateChanged(new SessionState { IsAuthenticated = true });
			}
		}

		private void UpdateCurrentUser(Account account)
		{
			lock (CurrentUser)
			{
				if (CurrentUser == null)
				{
					CurrentUser = account;
					return;
				}
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

		private Account ReadAccountFromAccountStore()
		{
			AccountStore store = AccountStore.Create();

			List<Account> accounts = new List<Account>(store.FindAccountsForService(AppName));
			if (accounts.Count > 1)
			{
				Debug.WriteLine($"WARNING: more than one account found, cleaning up");
				foreach (Account account in accounts)
				{
					store.Delete(account, AppName);
				}
				return null;
			}
			if (accounts.Count == 0)
			{
				return null;
			}
			// Only one account was found
			return accounts[0];
		}

		private void OnSessionStateChanged(SessionState state)
		{
			Debug.WriteLine($"{nameof(SessionStateChanged)} event triggered with state {state.ToString()}");
			SessionStateChanged?.Invoke(this, state);
		}

		private SessionInformationProvider()
		{

		}
	}
}
