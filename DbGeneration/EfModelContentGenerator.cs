using System.Linq;
using System.Threading.Tasks;

using Common;

using DotLiquid;

namespace ModelGenerator.CSharp
{
	public interface IEfModelContentGenerator
	{
		Task<string> GenerateDbModelContentAsync(CsData modelData);
		CsData GetModelDataWithDbProps(CsData modelData);
	}

	public class EfModelContentGenerator : IEfModelContentGenerator
	{
		public EfModelContentGenerator()
		{
		}

		public Task<string> GenerateDbModelContentAsync(CsData modelData)
		{
			CsData dbData = GetModelDataWithDbProps(modelData);

			return GenerateModelContentAsync(dbData);
		}

		public CsData GetModelDataWithDbProps(CsData modelData)
		{
			var clonedModelData = CopyUtils.GerDeepCloneOf(modelData);
			clonedModelData.classes.ToList().ForEach(c =>
			{
				for (int i = 0; i < c.props.Count; i++)
				{
					if (c.props[i].type.Equals("object", System.StringComparison.InvariantCultureIgnoreCase))
					{
						c.props[i].type = "string";
						continue;
					}
				}

				if (!c.props.Any(p => p.name.Equals("id", System.StringComparison.InvariantCultureIgnoreCase)))
				{
					c.AddIdProp();
				}
			});

			return clonedModelData;
		}

		public static async Task<string> GenerateModelContentAsync(CsData modelData)
		{
			return await TransfomTextUtils.GenerateModelContentAsync(modelData, "PostgreSql/Templates/efContext.liquid");
		}
	}
}
