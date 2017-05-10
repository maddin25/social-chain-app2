using System.IO;

public interface SystemInterface
{
	void Close();
	bool DeleteFile(string path);
	string GetApplicationDataFolder();
	bool CompressImage(Stream fileStream, string inputFile, string outputFile);
}