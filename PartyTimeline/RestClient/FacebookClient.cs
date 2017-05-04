using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Auth;
using Xamarin.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PartyTimeline
{
	public class FacebookClient
	{
		private readonly string AppId = "1632106426819143";
		private readonly TimeSpan LimitEventsInPast = TimeSpan.FromDays(180);

		public void Authorize(Action onSuccess)
		{
			var authenticator = new OAuth2Authenticator(
				clientId: AppId,
				scope: "email,user_events",
				authorizeUrl: new Uri("https://www.facebook.com/v2.9/dialog/oauth/"),
				redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"),
				isUsingNativeUI: false
			);
			authenticator.Completed += (object sender, AuthenticatorCompletedEventArgs e) =>
			{
				if (e.IsAuthenticated)
				{
					SessionInformation.INSTANCE.BeginSession(e.Account);
					onSuccess.BeginInvoke(onSuccess.EndInvoke, null);
					CompleteAccountInformation(e.Account);
				}
				Debug.WriteLine($"Authenticated: {e.IsAuthenticated}");
			};
			DependencyService.Get<FacebookInterface>().LaunchLogin(authenticator);
		}

		public async Task<List<Event>> GetEvents(Account account)
		{
			List<Event> events = new List<Event>();

			var initialRequest = new OAuth2Request(
				"GET",
				new Uri("https://graph.facebook.com/v2.9/me/events"),
				new Dictionary<string, string> { { "fields", "id,name,start_time,end_time,cover{id,source}" } },
				account
			);
			Response response = await initialRequest.GetResponseAsync();

			await ParseEventQueryResponse(response?.GetResponseText(), events);

			return events;
		}

		private async Task ParseEventQueryResponse(string response, List<Event> events)
		{
			if (string.IsNullOrWhiteSpace(response))
			{
				return;
			}
			List<Event> eventsInResponse = JObject.Parse(response).SelectToken("data").ToObject<List<Event>>();
			DateTime toleranzeInPast = DateTime.Now.Subtract(LimitEventsInPast);
			foreach (Event eventReference in eventsInResponse)
			{
				if (eventReference.StartDateTime < toleranzeInPast)
				{
					return;
				}
				events.Add(eventReference);
			}

			string cursor = JObject.Parse(response).SelectToken("paging")?.ToObject<FacebookPager>()?.next;
			if (!string.IsNullOrEmpty(cursor))
			{
				HttpWebRequest cursorRequest = WebRequest.CreateHttp(cursor);
				WebResponse cursorResponse = await cursorRequest.GetResponseAsync();
				StreamReader reader = new StreamReader(cursorResponse.GetResponseStream());
				await ParseEventQueryResponse(reader.ReadToEnd(), events);
			}
		}

		public async Task<bool> VerifyTokenValidity(Account account)
		{
			var request = new OAuth2Request(
				"GET",
				new Uri("https://graph.facebook.com/v2.9/debug_token"),
				new Dictionary<string, string>
				{
					// FIXME: use the app access token here https://developers.facebook.com/docs/facebook-login/access-tokens#apptokens
					{"input_token", account.Properties[FacebookAccountProperties.AccessToken]}
				},
				account
			);

			var response = await request.GetResponseAsync();
			if (response != null)
			{
				string responseText = response.GetResponseText();
				var tokenPermission = JObject.Parse(responseText).SelectToken("data").ToObject<FacebookTokenInspection>();
				return tokenPermission.is_valid;
			}
			return false;
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
				await SessionInformation.INSTANCE.UpdateSession(account);
			}
			else
			{
				throw new AuthException("Could not pull the remaining information from the Facebook server");
			}
		}

		public void OnIsAuthorized()
		{
			Application.Current.MainPage.Navigation.PushModalAsync(new EventListPage());
		}

		private class FacebookAccountInformation
		{
			public string id { get; set; }
			public string name { get; set; }
			public string email { get; set; }
		}

		private class FacebookTokenInspection
		{
			public long app_id { get; set; }
			public long user_id { get; set; }
			public string application { get; set; }
			public long expires_at { get; set; }
			public long issued_at { get; set; }
			public bool is_valid { get; set; }
			public List<string> scopes { get; set; }
		}

		private class FacebookPager
		{
			public string next { get; set; }
		}
	}
}
