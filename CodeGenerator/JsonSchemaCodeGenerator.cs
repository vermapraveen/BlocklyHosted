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
		public static async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{
			// Schema --> Parsed Object --> C# class
			IJsonSchemaInMemoryModelCreator jsonSchemaInMemoryModelCreator = new JsonSchemaInMemoryModelCreator();
			CsData classStructure = jsonSchemaInMemoryModelCreator.GetJsonModel(jsonSchema);

			IModelGenerator modelGenerator = new CSharpFileContentGenerator();
			var classCode = await modelGenerator.GenerateModelContentAsync(classStructure);

			IEfModelContentGenerator dbModelGenerator = new EfModelContentGenerator();
			_ = await dbModelGenerator.GenerateDbModelContentAsync(classStructure);

			return classCode;
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