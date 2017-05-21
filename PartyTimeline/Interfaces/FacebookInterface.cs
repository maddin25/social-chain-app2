using PartyTimeline;

using Xamarin.Auth;

public interface FacebookInterface
{
	void LaunchLogin(OAuth2Authenticator authenticator);
	void CloseLogin();
}
