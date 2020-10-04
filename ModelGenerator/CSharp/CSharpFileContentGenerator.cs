using System.Linq;
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

		public Task<string> GenerateDbModelContentAsync(CsData modelData)
		{
			CsData dbData = new CsData();

			modelData.classes.ToList().ForEach(c =>
			{
				var aClass = dbData.AddNewClass(c.name);

				for (int i = 0; i < c.props.Count; i++)
				{
					if (c.props[i].type.Equals("object", System.StringComparison.InvariantCultureIgnoreCase))
					{
						aClass.AddNewProp(c.props[i].name, "string");
						continue;
					}

					aClass.AddNewProp(c.name, c.props[i].type);
				}

				if (!c.props.Any(p => p.name.EndsWith("id", System.StringComparison.InvariantCultureIgnoreCase)))
				{
					aClass.AddNewProp("id", "Guid");
				}
			});

			return GenerateModelContentAsync(dbData);
		}

		public async Task<string> GenerateModelContentAsync(CsData modelData)
		{
			Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));
			var updated = template.Render(Hash.FromAnonymousObject(new { csData = modelData }));

			return updated;
		}
	}
}
