using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BlkHost.Pages
{
	[IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost([FromBody] CodeContext codeConext)
        {
            Console.Out.WriteLine($"{codeConext.Language} for {codeConext.Code}");
			var gen = new CsGenerator();
			gen.Generate(codeConext.Code);
        }
    }

    public class CodeContext
    {
        public string Code { get; set; }
        public string Language { get; set; }
    }
}
