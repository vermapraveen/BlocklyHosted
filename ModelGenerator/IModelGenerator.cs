using System.Threading.Tasks;

using ModelGenerator.CSharp;

using Models;

namespace ModelGenerator
{
	public interface IModelGenerator
	{
		Task<string> GenerateModelContentAsync(CsData modelData);
		Task SaveModelAsync(CsData modelData, string generateModleFileTo);
	}
}
