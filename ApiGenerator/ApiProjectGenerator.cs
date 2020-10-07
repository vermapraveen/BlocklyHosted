using System.IO;
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

			foreach (var f in allFiles)
			{
				var transformedText = await TransfomTextUtils.GenerateModelContentAsync(model, f);
				await FileUtils.CreateFileForContent(Path.GetFileNameWithoutExtension(f), transformedText);
			}
		}
	}
}
