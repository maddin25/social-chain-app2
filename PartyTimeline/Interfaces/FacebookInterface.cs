using PartyTimeline;

using Xamarin.Auth;

public interface FacebookInterface
{
	void LaunchLogin(Authenticator authenticator);
	void CloseLogin();
}
