using System;
using System.IO;
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

		public bool DeleteFile(string path)
		{
			try
			{
				File.Delete(path);
			}
			catch (IOException e)
			{
				SDebug.WriteLine(e.Message);
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
			SDebug.WriteLine($"MyDocuments: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}");
			SDebug.WriteLine($"ApplicationData: {Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}");
			SDebug.WriteLine($"LocalApplicationData: {Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}");
			SDebug.WriteLine($"Personal: {Environment.GetFolderPath(Environment.SpecialFolder.Personal)}");
			SDebug.WriteLine($"Desktop: {Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}");
		}
	}
}
