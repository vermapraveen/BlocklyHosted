using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace ApiTester.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SelectionMatrixController : ControllerBase
	{
		private const string connStr = "Host=localhost;Port=5433;Database=user1;Username=postgres;Password=postgres";

		[HttpGet]
		public async Task<string> GetAsync()
		{

			DataAccessLayer dal = new DataAccessLayer(connStr);
			return await dal.Get();
		}

		[HttpPost]
		public async Task PostAsync()
		{

			DataAccessLayer dal = new DataAccessLayer(connStr);
			await dal.Add();
		}
	}
}
