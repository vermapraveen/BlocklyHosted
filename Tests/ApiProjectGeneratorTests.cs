using System.Threading.Tasks;

using ApiGenerator;

using ModelGenerator.CSharp;

using Xunit;

namespace Tests
{
	public class ApiProjectGeneratorTests
	{
		[Fact]
		public async Task ShouldGenerateProjectFiles()
		{
			var csharpModel = new CsData()
			{
				classes = new System.Collections.Generic.List<CsData.CsClass>
				{
					new CsData.CsClass("Employee")
				},
				@namespace = "Stock",
				appname = "StockApp",
				api = new CsData.Api
				{
					endpoint = new CsData.Api.Endpoint
					{
						model = "StockData",
						name = "Stock"
					}
				},
				db = new CsData.Db
				{
					connstring = "this is mt conn string"
				}
			};

			var api = new ApiProjectGenerator<CsData>(csharpModel);
			await api.GenerateProjectFor("CSharp");
		}
	}
}
