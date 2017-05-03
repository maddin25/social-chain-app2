using System;
namespace PartyTimeline
{
	/// <summary>
	/// The account stored in the AccountStore will contain properties with these fields
	/// </summary>
	public static class FacebookAccountProperties
	{
		public static readonly string Id = "id";
		public static readonly string Name = "name";
		public static readonly string EMail = "email";
		public static readonly string AccessToken = "access_token";
		/// <summary>
		/// This property contains an integer, describing the seconds the token will expire after it was retrieved
		/// </summary>
		public static readonly string ExpiresIn = "expires_in";
		/// <summary>
		/// This property contains a long in UTC format, describing the DateTime the token will expire on
		/// </summary>
		public static readonly string ExpiresOn = "expires_on";
	}
}
