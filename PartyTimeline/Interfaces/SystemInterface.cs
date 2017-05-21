using System.IO;
using System.Threading.Tasks;

public interface SystemInterface
{
	void Close();
	bool DeleteFile(string path);
	string GetApplicationDataFolder();
	Task<bool> CompressImage(Stream fileStream, string inputFile, string outputFile);
	byte[] ReadFile(string path);
}