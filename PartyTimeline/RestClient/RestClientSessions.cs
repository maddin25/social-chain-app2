using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PartyTimeline
{
	public class RestClientSessions : RestClient<UserSession>
	{
		public RestClientSessions() : base("users")
		{
			
		}

		public async Task<bool> Register(UserSession session)
		{
			Debug.WriteLine($"Registering new {nameof(UserSession)} for {nameof(UserSession.EventMemberId)}: {session.EventMemberId}");
			return await PostAsync(session, "login");
		}
	}
}
