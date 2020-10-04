using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbGeneration;

using Shouldly;

using Xunit;

namespace Tests
{
	public class DbGeneratedCodeTester
	{
		private const string connStr = "";

		[Fact]
		public async Task ShouldAbleToCreateData()
		{
			DataAccessLayer dal = new DataAccessLayer(connStr);
			await dal.Add();
		}


		[Fact]
		public async Task ShouldAbleToReadData()
		{
			DataAccessLayer dal = new DataAccessLayer(connStr);
			var frstName = await dal.Get();
			frstName.ShouldBe("John1");
		}


		[Fact]
		public async Task ShouldAbleToUpdateData()
		{
		}


		[Fact]
		public async Task ShouldAbleToDeleteData()
		{
		}
	}
}
