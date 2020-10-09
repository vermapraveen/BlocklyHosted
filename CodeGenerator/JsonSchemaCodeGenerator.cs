
using ModelGenerator.CSharp;
using ModelGenerator;
using ApiGenerator;
using Models;

namespace CodeGenerator
{
	public class JsonSchemaCodeGenerator
	{
		public static async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{
			// Schema --> Parsed Object --> C# class
			IJsonSchemaInMemoryModelCreator jsonSchemaInMemoryModelCreator = new JsonSchemaInMemoryModelCreator();
			CsData inputSchemaModel = jsonSchemaInMemoryModelCreator.GetJsonModel(jsonSchema);


			inputSchemaModel.appname = "StockApp";
			inputSchemaModel.@namespace = $"{inputSchemaModel.appname}Ns";

			inputSchemaModel.db = new CsData.Db
			{
				connstring = "this is mt conn string"
			};

			IEfModelContentGenerator dbModelGenerator = new EfModelContentGenerator();
			var updatedModel = dbModelGenerator.GetSchemaInputModelDataWithDbProps(inputSchemaModel);

			var api = new ApiProjectGenerator<CsData>(updatedModel);
			await api.GenerateProjectFor("CSharp");

			IModelGenerator modelGenerator = new CSharpFileContentGenerator();
			var classCode = await modelGenerator.GenerateModelContentAsync(inputSchemaModel);


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