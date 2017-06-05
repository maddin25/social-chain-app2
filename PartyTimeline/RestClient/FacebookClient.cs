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
		private readonly string FacebookApiUrl = "https://www.facebook.com";
		private readonly string GraphApiUrl = "https://graph.facebook.com";
		private readonly string ApiVersion = "v2.9";

		private readonly string FacebookApiLoginOauth = "dialog/oauth";
		private readonly string FacebookApiLoginRedirect = "connect/login_success.html";
		private readonly string GraphApiNodeMe = "me";
		private readonly string GraphApiNodeEvents = "events";

		private readonly string RequestGet = "GET";
		private readonly string RequestParameterFields = "fields";

		public void Authorize(Action callbackOnFailure)
		{
			var authenticator = new OAuth2Authenticator(
				clientId: AppId,
				scope: "email,user_events",
				authorizeUrl: WebUriBuilder(FacebookApiUrl, ApiVersion, FacebookApiLoginOauth),
				redirectUrl: WebUriBuilder(FacebookApiUrl, FacebookApiLoginRedirect),
				isUsingNativeUI: false
			);

			authenticator.Completed += async (object sender, AuthenticatorCompletedEventArgs e) =>
			{
				if (e.IsAuthenticated)
				{
					await CompleteAccountInformation(e.Account);
					SessionInformationProvider.INSTANCE.BeginSession(e.Account);
				}
				else
				{
					callbackOnFailure.Invoke();
				}
				Debug.WriteLine($"Authenticated: {e.IsAuthenticated}");
			};

			DependencyService.Get<FacebookInterface>().LaunchLogin(authenticator);
		}

		public async Task<Event> GetEventDetails(long id)
		{
			string response = await MakeIdRequest(SessionInformationProvider.INSTANCE.CurrentUserAccount,
			                                      id,
			                                      new Dictionary<string, string> {
				{ RequestParameterFields, "id,name,start_time,end_time,updated_time,cover{id,source}" }
			});
			Event eventReference = JsonConvert.DeserializeObject<Event>(response);
			return eventReference;
		}

		public async Task<List<Event>> GetEventHeaders()
		{
            Debug.WriteLine($"Facebook:GetEventHeaders for {SessionInformationProvider.INSTANCE.CurrentUserEventMember.Name}");
            List<Event> events = new List<Event>();

			var initialRequest = new OAuth2Request(
				method: RequestGet,
				url: WebUriBuilder(GraphApiUrl, ApiVersion, GraphApiNodeMe, GraphApiNodeEvents),
				parameters: new Dictionary<string, string> { { RequestParameterFields, "id,start_time,updated_time,is_canceled,is_draft" } },
				account: SessionInformationProvider.INSTANCE.CurrentUserAccount
			);
			Response response = await initialRequest.GetResponseAsync();

			await ParseEventQueryResponse(response.GetResponseText(), events);

			return events;
		}

		private async Task ParseEventQueryResponse(string response, List<Event> events)
		{
			if (string.IsNullOrWhiteSpace(response))
			{
				return;
			}
            var data = JObject.Parse(response).SelectToken("data");
            List<Event> eventsInResponse = data.ToObject<List<Event>>();
			DateTime toleranzeInPast = DateTime.Now.Subtract(EventService.LimitEventsInPast);
			foreach (Event eventReference in eventsInResponse)
			{
				if (eventReference.StartDateTime < toleranzeInPast)
				{
					return;
				}
				events.Add(eventReference);
			}
			// Look, if a pager is available and if yes, follow it
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
				RequestGet,
				WebUriBuilder(GraphApiUrl, ApiVersion, GraphApiNodeMe),
				new Dictionary<string, string> { { RequestParameterFields, "id,name,email" } },
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

		private async Task<string> MakeIdRequest(Account account, long id, Dictionary<string, string> parameters = null)
		{
			var request = new OAuth2Request(
				method: RequestGet,
				url: WebUriBuilder(GraphApiUrl, ApiVersion, id.ToString()),
				parameters: parameters,
				account: account
			);
			var response = await request.GetResponseAsync();
			return response.GetResponseText();
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

		private static Uri WebUriBuilder(params string[] parts)
		{
			return new Uri(string.Join("/", parts));
		}

		private class FacebookPager
		{
			public string next { get; set; }
		}
	}
}
