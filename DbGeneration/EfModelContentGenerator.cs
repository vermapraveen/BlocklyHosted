using System.Linq;
using System.Threading.Tasks;

using Common;

using DotLiquid;

namespace ModelGenerator.CSharp
{
	public interface IEfModelContentGenerator
	{

		Task<string> GenerateDbModelContentAsync(CsData modelData);
	}
	public class EfModelContentGenerator : IEfModelContentGenerator
	{
		public EfModelContentGenerator()
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

					aClass.AddNewProp(c.props[i].name, c.props[i].type);
				}

				if (!c.props.Any(p => p.name.Equals("id", System.StringComparison.InvariantCultureIgnoreCase)))
				{
					aClass.AddIdProp();
				}
			});

			return GenerateModelContentAsync(dbData);
		}

		public static async Task<string> GenerateModelContentAsync(CsData modelData)
		{
			return await TransfomTextUtils.GenerateModelContentAsync(modelData, "PostgreSql/Templates/efContext.liquid");
		}
	}
}
