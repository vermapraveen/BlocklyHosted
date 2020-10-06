using System.Threading.Tasks;

using ApiTester.Models;

using Microsoft.AspNetCore.Mvc;

namespace ApiTester.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SelectionMatrixController : ControllerBase
	{
		private const string connStr = "Host=localhost;Port=5433;Database=user1;Username=postgres;Password=postgres";

		[HttpGet]
		public async Task<SelectionMatrix> GetAsync()
		{

			DataAccessLayer dal = new DataAccessLayer(connStr);
			var firstName = await dal.Get();
			return new SelectionMatrix { firstName = firstName };
		}

		[HttpPost]
		public async Task PostAsync(SelectionMatrix selectionMatrix)
		{
			DataAccessLayer dal = new DataAccessLayer(connStr);
			await dal.Add();
		}
	}
}
