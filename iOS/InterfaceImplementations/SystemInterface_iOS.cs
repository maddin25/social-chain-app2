using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

using CoreGraphics;
using Foundation;
using UIKit;

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

		public async Task<bool> CompressImage(Stream fileStream, string inputFile, string outputFile)
		{
			NSError error = await Task.Run(() =>
			{
				Debug.WriteLine($"Original image file '{inputFile}' (Size: {new FileInfo(inputFile).Length / 1024} KB)");

				UIImage img = new UIImage(NSData.FromStream(fileStream));

				CGSize imgSize = img.Size;
				switch (ImageCompression.DeterminePrimaryScaleDimension(imgSize.Height, imgSize.Width))
				{
					case ImageCompression.ScaleDown.Height:
						imgSize = new CGSize(
							ImageCompression.SecondaryTargetSize(imgSize.Height, imgSize.Width),
							ImageCompression.MaximumDimension
						);
						img = img.Scale(imgSize);
						break;
					case ImageCompression.ScaleDown.Width:
						imgSize = new CGSize(
							ImageCompression.MaximumDimension,
							ImageCompression.SecondaryTargetSize(imgSize.Width, imgSize.Height)
						);
						img = img.Scale(imgSize);
						break;
					case ImageCompression.ScaleDown.None:
						break;
				}

				NSData imgData = img.AsJPEG(ImageCompression.CompressionFactorJpegFloat);
				imgData.Save(outputFile, NSDataWritingOptions.FileProtectionCompleteUntilFirstUserAuthentication, out error);
				Debug.WriteLine($"Wrote compressed image file to '{outputFile}' (Size: {new FileInfo(outputFile).Length / 1024} KB)");

				return error;
			}); // TODO: maybe use configure context here
			return error == null;
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
