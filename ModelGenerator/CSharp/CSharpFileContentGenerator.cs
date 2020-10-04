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
	}
}
