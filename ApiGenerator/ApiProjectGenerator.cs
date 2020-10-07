using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Common;

namespace ApiGenerator
{
	public class ApiProjectGenerator<T>
	{
		private readonly T model;

		public ApiProjectGenerator(T model)
		{
			this.model = model;
		}

		public async Task GenerateProjectFor(string projectType)
		{
			var allFiles = Directory.GetFiles(projectType, "*.liquid", SearchOption.AllDirectories);
			var rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
			foreach (var f in allFiles)
			{
				var transformedText = await TransfomTextUtils.GenerateModelContentAsync(model, f);
				string currentFilePath = Path.GetFullPath(f);
				string generateFilePath = Path.Combine(Path.GetFullPath(@"..\..\..\..\"), $"artifacts\\api_project\\{projectType}");

				Directory.CreateDirectory(generateFilePath);
				string filepath = Path.Combine(generateFilePath, Path.GetFileNameWithoutExtension(f));
				await FileUtils.CreateFileForContent(filepath, transformedText);
			}
		}
	}
}
