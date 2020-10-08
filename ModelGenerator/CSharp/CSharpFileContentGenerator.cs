using System.IO;
using System.Threading.Tasks;

using Common;

using DotLiquid;

namespace ModelGenerator.CSharp
{
	public class CSharpFileContentGenerator : IModelGenerator
	{
		public CSharpFileContentGenerator()
		{
		}

		public async Task<string> GenerateModelContentAsync(CsData modelData)
		{
			Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));
			var updated = template.Render(Hash.FromAnonymousObject(new { csData = modelData }));

			return updated;
		}

		public async Task SaveModelAsync(CsData modelData, string generateModleFileTo)
		{
			Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));

			try
			{
				var generatedCode = template.Render(Hash.FromAnonymousObject(new { csData = modelData }));
				string filepath = Path.Combine(generateModleFileTo, $"{modelData.api.endpoint.model}.cs");

				await FileUtils.CreateFileForContent(filepath, generatedCode);
			}
			catch (System.Exception ex)
			{

				throw;
			}

		}
	}
}
