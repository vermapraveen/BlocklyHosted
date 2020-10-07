using System.Threading.Tasks;

using ApiGenerator;

using Xunit;

namespace Tests
{
	public class ApiProjectGeneratorTests
	{
		[Fact]
		public async Task ShouldGenerateProjectFiles()
		{
			ApiProjectGenerator api = new ApiProjectGenerator();
			await api.GenerateProjectFor("CSharp");
		}
	}
}
