using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Auth;
using Xamarin.Forms;

using Newtonsoft.Json;

namespace PartyTimeline
{
	public class FacebookCommunicator
	{
		AccountStore store;

		public OAuth2Authenticator Authenticator { get; set; }

		public bool IsAuthorized
		{
			get
			{
				Account account = AuthorizedAccount;
				if (account == null)
				{
					return false;
				}
				if (account.Properties.ContainsKey(FacebookAccountProperties.ExpiresOn))
				{
					DateTime expiresOn = DateTime.FromFileTime(long.Parse(account.Properties[FacebookAccountProperties.ExpiresOn]));
					return expiresOn > DateTime.Now; // is true, if the Account token is not yet expired
				}
				Debug.WriteLine($"WARNING: Account {account.Username} does not contain the {nameof(FacebookAccountProperties.ExpiresOn)} property");
				store.Delete(account, Resources.AppResources.AppName);
				return false;
			}
		}

		public Account AuthorizedAccount
		{
			get
			{
				IEnumerable<Account> accounts = store.FindAccountsForService(Resources.AppResources.AppName);
				List<Account> accountsList = new List<Account>(accounts);
				if (accountsList.Count > 1)
				{
					Debug.WriteLine($"WARNING: more than one account found, cleaning up");
					foreach (Account acc in accounts)
					{
						store.Delete(acc, Resources.AppResources.AppName);
					}
					return null;
				}
				if (accountsList.Count == 0)
				{
					return null;
				}
				// Only one account was found
				return accountsList[0];
			}
		}

		public FacebookCommunicator()
		{
			store = AccountStore.Create();

			Authenticator = new OAuth2Authenticator(
				clientId: "1632106426819143",
				scope: "email,user_events",
				authorizeUrl: new Uri("https://www.facebook.com/v2.9/dialog/oauth/"),
				redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
				isUsingNativeUI: false
			);
			Authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs e) =>
			{
				if (e.IsAuthenticated)
				{
					await CompleteAccountInformation(e.Account);
					AccountStore.Create().Save(e.Account, Resources.AppResources.AppName);
					OnIsAuthorized();
				}
				Debug.WriteLine($"Authenticated: {e.IsAuthenticated}");
			};
		}

		public async Task CompleteAccountInformation(Account account)
		{
			// Calculate the absolute expiration date
			DateTime expiresOn = DateTime.Now.AddSeconds(int.Parse(account.Properties[FacebookAccountProperties.ExpiresIn]));
			account.Properties[FacebookAccountProperties.ExpiresOn] = expiresOn.ToFileTimeUtc().ToString();
			// Pull the remaining information from the server
			var request = new OAuth2Request(
				"GET",
				new Uri("https://graph.facebook.com/v2.9/me?fields=id,name,email"),
				null,
				account
			);
			var response = await request.GetResponseAsync();
			if (response != null)
			{
				var accountInformation = JsonConvert.DeserializeObject<FacebookAccountInformation>(response.GetResponseText());
				account.Properties[FacebookAccountProperties.Id] = accountInformation.id;
				account.Properties[FacebookAccountProperties.Name] = accountInformation.name;
				account.Properties[FacebookAccountProperties.EMail] = accountInformation.email;
			}
			else
			{
				throw new AuthException("Could not pull the remaining information from the Facebook server");
			}
		}

		public void OnIsAuthorized()
		{
			SessionInformation.INSTANCE.SetCurrentUser(AuthorizedAccount);
			Application.Current.MainPage.Navigation.PushModalAsync(new EventListPage());
		}

		private class FacebookAccountInformation
		{
			public string id { get; set; }
			public string name { get; set; }
			public string email { get; set; }
		}
	}
}
