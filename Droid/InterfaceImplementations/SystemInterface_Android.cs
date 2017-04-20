using SDebug = System.Diagnostics.Debug;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.Droid.SystemInterface_Android))]
namespace PartyTimeline.Droid
{
	public class SystemInterface_Android : SystemInterface
	{
		public SystemInterface_Android()
		{
		}

		public void Close()
		{
			SDebug.WriteLine("Quitting Application");
			// TODO: maybe replace this with a less brutal or more proper way
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}
	}
}
