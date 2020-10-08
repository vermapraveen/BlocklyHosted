using System.IO;
using System.Threading.Tasks;

using Common;

using ModelGenerator;
using ModelGenerator.CSharp;

namespace ApiGenerator
{
	public class ApiProjectGenerator<T> where T : CsData
	{
		private readonly T model;

		IModelGenerator modelGenerator = new CSharpFileContentGenerator();

		public ApiProjectGenerator(T model)
		{
			this.model = model;
		}

		public async Task GenerateProjectFor(string projectType)
		{
			var allFiles = Directory.GetFiles(projectType, "*.liquid", SearchOption.AllDirectories);
			string generateFilePath = Path.Combine(Path.GetFullPath(@"..\..\..\..\"), $"artifacts\\api_project\\{projectType}");

			foreach (var f in allFiles)
			{
				var transformedText = await TransfomTextUtils.GenerateModelContentAsync(model, f);

				Directory.CreateDirectory(generateFilePath);
				string filepath = Path.Combine(generateFilePath, Path.GetFileNameWithoutExtension(f));
				await FileUtils.CreateFileForContent(filepath, transformedText);
			}

			await modelGenerator.SaveModelAsync(model, generateFilePath);

		}
	}
}
