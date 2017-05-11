using System;
using System.IO;
using SDebug = System.Diagnostics.Debug;
using System.Threading.Tasks;

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

		public async Task<bool> CompressImage(Stream fileStream, string inputFile, string outputFile)
		{
			SDebug.WriteLine($"Original image file '{inputFile}' (Size: {new FileInfo(inputFile).Length / 1024} KB)");
			Bitmap img = await BitmapFactory.DecodeStreamAsync(fileStream);

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

			FileStream writeStream = new FileStream(path: outputFile,
			                                        mode: FileMode.Create,
			                                        access: FileAccess.Write,
			                                        share: FileShare.None,
			                                        bufferSize: 8,
			                                        useAsync: true);
			bool success = await img.CompressAsync(Bitmap.CompressFormat.Jpeg, ImageCompression.CompressionFactorJpeg, writeStream);  // async version available
			writeStream.Close();
			SDebug.WriteLine($"Wrote compressed image file to '{outputFile}' (Size: {new FileInfo(outputFile).Length / 1024} KB)");

			return success;
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
