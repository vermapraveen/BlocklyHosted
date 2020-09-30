
using CodeGenerator;

using Microsoft.AspNetCore.Mvc;

using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlkHost.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CodeGeneratorController : ControllerBase
	{
		// POST api/<CodeGeneratorController>
		[HttpPost]
		public string Post([FromBody] CodeContext codeConext)
		{
			var gen = new JsonSchemaCodeGenerator();
			return gen.GenerateAsync(codeConext.Code).Result;
		}
	}
}
