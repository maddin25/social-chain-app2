
public interface SystemInterface
{
	void Close();
	bool DeleteFile(string path);
	string GetApplicationDataFolder();
}