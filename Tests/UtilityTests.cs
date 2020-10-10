using System;

using Common;

using Shouldly;

using Xunit;

namespace Tests
{
	public class UtilityTests
	{
		[Fact]
		public void ShoudAbleGenerateRandomNumbers()
		{
			var rnd = new Random();
			var ranNum1 = rnd.GetNextLong();

			var rnd2= new Random();
			var ranNum2 = rnd2.GetNextLong();

			ranNum2.ShouldNotBe(ranNum1);
		}
	}
}
