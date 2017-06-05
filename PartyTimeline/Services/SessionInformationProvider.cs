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
		private UserSession _currentUserSession;
		private EventMember _currentUserEventMember;

		private RestClientSessions clientSessions;
		private static SessionInformationProvider _instance;

		#region PublicMethods
		public static readonly string AppName = "PartyTimeline";

		public EventHandler SessionStateChanged;

		public EventMember CurrentUserEventMember
		{
			get
			{
				if (_currentUserEventMember == null && CurrentUserAccount != null)
				{
					_currentUserEventMember = new EventMember
					{
						Id = long.Parse(CurrentUserAccount.Properties[FacebookAccountProperties.Id]),
						Name = CurrentUserAccount.Properties[FacebookAccountProperties.Name],
						EmailAddress = CurrentUserAccount.Properties[FacebookAccountProperties.EMail]
					};
				}
				return _currentUserEventMember;
			}
		}

		public UserSession CurrentUserSession
		{
			get
			{
				if (_currentUserSession == null && CurrentUserAccount != null)
				{
					_currentUserSession = new UserSession
					{
						Id = GetUserProperty(FacebookAccountProperties.AccessToken),
						EventMemberId = CurrentUserEventMember.Id,
						ExpiresOn = DateTime.FromFileTime(long.Parse(GetUserProperty(FacebookAccountProperties.ExpiresOn)))
					};
				}
				return _currentUserSession;
			}
		}

		public Account CurrentUserAccount
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
				_currentUserEventMember = null;
				_currentUserSession = null;
				_currentUser = value;
			}
		}

		public bool ActiveSessionAvailable
		{
			get
			{
				if (CurrentUserAccount == null)
				{
					return false;
				}
				if (!CurrentUserAccount.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
				{
					Debug.WriteLine($"WARNING: Account {CurrentUserAccount.Username} does not contain the {nameof(FacebookAccountProperties.ExpiresOn)} property");
					EndSession();
					return false;
				}
				return CurrentUserSession.ExpiresOn > DateTime.Now; // is true, if the Account token is not yet expired
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

		#endregion

		#region PublicMethods

		public string GetUserProperty(string propertyName)
		{
			if (CurrentUserAccount?.Properties.ContainsKey(propertyName) ?? false)
			{
				return CurrentUserAccount.Properties[propertyName];
			}
			return null;
		}

		public void BeginSession(Account account)
		{
			UpdateSession(account);
			OnSessionStateChanged(new SessionState { IsAuthenticated = true });
		}

		public void EndSession()
		{
			AccountStore.Create().Delete(CurrentUserAccount, AppName);
			CurrentUserAccount = null;
			OnSessionStateChanged(new SessionState { IsAuthenticated = false });
		}

		public async void UpdateSession(Account account)
		{
			AccountStore.Create().Save(account, AppName);
			CurrentUserAccount = account;
			await clientSessions.Register(CurrentUserSession);
			EventService.INSTANCE.AddEventMember(CurrentUserEventMember);
		}

		public async void AuthenticateUserIfRequired()
		{
			if (!ActiveSessionAvailable)
			{
				FacebookClient fbCommunicator = new FacebookClient();
				// TODO: remove callback if possible
				fbCommunicator.Authorize(() => OnSessionStateChanged(new SessionState { IsAuthenticated = false }));
			}
			else
			{
				OnSessionStateChanged(new SessionState { IsAuthenticated = true });
			}
		}

		#endregion

		private void UpdateCurrentUser(Account account)
		{
			lock (CurrentUserAccount)
			{
				if (CurrentUserAccount == null)
				{
					CurrentUserAccount = account;
					return;
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.Id))
				{
					CurrentUserAccount.Properties[FacebookAccountProperties.Id] = account.Properties[FacebookAccountProperties.Id];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.Name))
				{
					CurrentUserAccount.Properties[FacebookAccountProperties.Name] = account.Properties[FacebookAccountProperties.Name];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.EMail))
				{
					CurrentUserAccount.Properties[FacebookAccountProperties.EMail] = account.Properties[FacebookAccountProperties.EMail];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.AccessToken))
				{
					CurrentUserAccount.Properties[FacebookAccountProperties.AccessToken] = account.Properties[FacebookAccountProperties.AccessToken];
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
				{
					CurrentUserAccount.Properties[FacebookAccountProperties.ExpiresOn] = account.Properties[FacebookAccountProperties.ExpiresOn];
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
			clientSessions = new RestClientSessions();
		}
	}
}
