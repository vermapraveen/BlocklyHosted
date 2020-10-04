using System.Collections.Generic;
using System.Linq;

using ModelGenerator.CSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ModelGenerator;
using Common;

namespace CodeGenerator
{
	public class JsonSchemaCodeGenerator
	{
		public async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{
			// Schema --> Parsed Object --> C# class
			IJsonSchemaInMemoryModelCreator jsonSchemaInMemoryModelCreator = new JsonSchemaInMemoryModelCreator();
			CsData classStructure = jsonSchemaInMemoryModelCreator.GetJsonModel(jsonSchema);

			IModelGenerator modelGenerator = new CSharpFileContentGenerator();

			//Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));
			//var updated = template.Render(Hash.FromAnonymousObject(new { csData = classStructure }));

			//var classCode = await modelGenerator.GenerateModelContentAsync(classStructure);

			var dbModelCode = await modelGenerator.GenerateDbModelContentAsync(classStructure);

			return dbModelCode;
		}


	}

	public struct PropertyStructure
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public bool IsCollection { get; set; }
		public bool IsNullable { get; set; }
	}
}