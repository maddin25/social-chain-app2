using System;
using System.IO;
using System.Diagnostics;

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

		public bool DeleteFile(string path)
		{
			try
			{
				File.Delete(path);
			}
			catch (IOException e)
			{
				Debug.WriteLine(e.Message);
				return false;
			}
			return true;
		}

		public string GetApplicationDataFolder()
		{
			PrintPaths();
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		private void PrintPaths()
		{
			Debug.WriteLine($"MyDocuments: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
			Debug.WriteLine($"ApplicationData: {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
			Debug.WriteLine($"LocalApplicationData: {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");
			Debug.WriteLine($"Personal: {Environment.GetFolderPath(Environment.SpecialFolder.Personal)}");
			Debug.WriteLine($"Desktop: {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}");
		}
	}
}
