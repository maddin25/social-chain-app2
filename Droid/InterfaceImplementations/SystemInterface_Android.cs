using System;
using System.IO;
using SDebug = System.Diagnostics.Debug;

using Android.Graphics;

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

		public bool CompressImage(Stream fileStream, string outputFile)
		{
			Bitmap img = BitmapFactory.DecodeStream(fileStream); // async version available
			switch (ImageCompression.DeterminePrimaryScaleDimension(img.Height, img.Width))
			{
				case ImageCompression.ScaleDown.Height:
					img = Bitmap.CreateScaledBitmap(
						img,
						ImageCompression.SecondaryTargetSize(img.Height, img.Width),
						ImageCompression.MaximumDimension,
						true);
					break;
				case ImageCompression.ScaleDown.Width:
					img = Bitmap.CreateScaledBitmap(
						img,
						ImageCompression.MaximumDimension,
						ImageCompression.SecondaryTargetSize(img.Width, img.Height),
						true);
					break;
				case ImageCompression.ScaleDown.None:
					break;
			}

			FileStream writeStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);  // async version available
			return img.Compress(Bitmap.CompressFormat.Jpeg, ImageCompression.CompressionFactorJpeg, writeStream);  // async version available
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
