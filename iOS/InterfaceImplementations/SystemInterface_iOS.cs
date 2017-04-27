using System;
using System.IO;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.SystemInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class SystemInterface_iOS : SystemInterface
	{
		public SystemInterface_iOS()
		{
		}

		public void Close()
		{
			throw new NotImplementedException("Close method is not implemented for iOS");
		}

		public string GetApplicationDataFolder()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		}
	}
}
