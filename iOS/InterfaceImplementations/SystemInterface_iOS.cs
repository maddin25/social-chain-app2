using System;
using System.IO;

using Xamarin.Forms;

[assembly: Dependency(typeof(PartyTimeline.iOS.SystemInterface_iOS))]
namespace PartyTimeline.iOS
{
	public class SystemInterface_iOS : SystemInterface
	{
		private static readonly string CACHE_DIRECTORY = "./Library";

		public SystemInterface_iOS()
		{
		}

		public void Close()
		{
			throw new NotImplementedException("Close method is not implemented for iOS");
		}

		public string GetApplicationDataFolder()
		{
			Directory.CreateDirectory(CACHE_DIRECTORY);
			return CACHE_DIRECTORY;
		}
	}
}
