using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace FastApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FastApiController : ControllerBase
	{
		public FastApiController(DataAccess dataAccessLayer)
		{
			_dataAccessLayer = dataAccessLayer;
		}

		private readonly DataAccess _dataAccessLayer;

		[HttpGet]
		public async Task<ActionResult<string>> Get(long Id)
		{
			var response = await _dataAccessLayer.Get(Id);

			if (string.IsNullOrEmpty(response))
				return NotFound();

			return response;
		}


		[HttpPost]
		public async Task<long> Post(string response)
		{
			return await _dataAccessLayer.Add(response);
		}
	}
}