using System;
using System.Collections.Generic;
using System.Linq;

using Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FastApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class JsonApiController : ControllerBase
	{
		private static readonly string[] SampleJsons = new[]
		{
			"{\"name\":\"John\", \"age\":31, \"city\":\"New York\"}","{\"employee\":{\"name\":\"sonoo\",\"salary\":56000,\"married\":true}}","{\"menu\":{\"id\":\"file\",\"value\":\"File\",\"popup\":{\"menuitem\":[{\"value\":\"New\",\"onclick\":\"CreateDoc()\"},{\"value\":\"Open\",\"onclick\":\"OpenDoc()\"},{\"value\":\"Save\",\"onclick\":\"SaveDoc()\"}]}}}",
		};

		private readonly ILogger<JsonApiController> _logger;

		// The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
		static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

		public JsonApiController(ILogger<JsonApiController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public string Get(ulong Id)
		{
			var rng = new Random();
			return SampleJsons[rng.Next(SampleJsons.Length)];
		}
	}
}