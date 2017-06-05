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
			Debug.WriteLine($"Registering {session.ToString()}");
			return await PostAsync(session, "login");
		}
	}
}
