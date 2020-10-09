using System.IO;
using System.Threading.Tasks;

using Common;

using DotLiquid;

using Models;

namespace ModelGenerator.CSharp
{
	public class CSharpFileContentGenerator : IModelGenerator
	{
		public CSharpFileContentGenerator()
		{
		}

		public async Task<string> GenerateModelContentAsync(CsData modelData)
		{
			var updated = await TransfomTextUtils.GenerateModelContentAsync(modelData, "Templates/CSharp/classCs.liquid");

			return updated;
		}

		public async Task SaveModelAsync(CsData modelData, string generateModleFileTo)
		{
			var generatedCode = await TransfomTextUtils.GenerateModelContentAsync(modelData, "Templates/CSharp/classCs.liquid");
			string filepath = Path.Combine(generateModleFileTo, $"{modelData.api.endpoint.model}.cs");

			await FileUtils.CreateFileForContent(filepath, generatedCode);
		}
	}
}
