using System.Threading.Tasks;

using DbGeneration;

using Shouldly;

using Xunit;

namespace Tests
{
	public class DbGeneratedCodeTester
	{
		private const string connStr = "Host=localhost;Port=5433;Database=user1;Username=postgres;Password=postgres";


		[Fact]
		public async Task ShouldAbleToPerformCRUDOps()
		{
			var updateFirstNameTo = "master";
			DataAccessLayer dal = new DataAccessLayer(connStr);
			await dal.DeleteAll();

			var count = await dal.GetCount();
			count.ShouldBe(0);

			await dal.Add();
			count = await dal.GetCount();
			count.ShouldBe(1);

			var frstName = await dal.Get();

			await dal.UpdateFirstName(frstName, updateFirstNameTo);

			var updatedName = await dal.Get();
			updatedName.ShouldBe(updateFirstNameTo);
			await dal.DeleteAll();

			count = await dal.GetCount();
			count.ShouldBe(0);
		}
	}
}
