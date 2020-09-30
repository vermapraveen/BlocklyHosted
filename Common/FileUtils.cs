using System.IO;
using System.Threading.Tasks;

namespace Common
{
	public static class FileUtils
	{
		public static async Task<string> GetFileContent(string filePath)
		{
			return await File.ReadAllTextAsync(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath));
		}

		public static async Task CreateFileForContent(string filePath, string fileContent)
		{
			await File.WriteAllTextAsync(filePath, fileContent);
		}
	}
}
