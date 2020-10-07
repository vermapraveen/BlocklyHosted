using System.Threading.Tasks;

using DotLiquid;

namespace Common
{
	public static class TransfomTextUtils
	{
		public static async Task<string> GenerateModelContentAsync<TData>(TData modelData, string filePath)
		{
			Template template = Template.Parse(await FileUtils.GetFileContent(filePath));
			var updated = template.Render(Hash.FromAnonymousObject(new { csData = modelData }));

			return updated;
		}
	}
}
