
using ModelGenerator.CSharp;
using ModelGenerator;
using ApiGenerator;

namespace CodeGenerator
{
	public class JsonSchemaCodeGenerator
	{
		public static async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{
			// Schema --> Parsed Object --> C# class
			IJsonSchemaInMemoryModelCreator jsonSchemaInMemoryModelCreator = new JsonSchemaInMemoryModelCreator();
			CsData jsonModel = jsonSchemaInMemoryModelCreator.GetJsonModel(jsonSchema);


			jsonModel.appname = "StockApp";
			jsonModel.@namespace = $"{jsonModel.appname}Ns";

			jsonModel.db = new CsData.Db
			{
				connstring = "this is mt conn string"
			};

			IEfModelContentGenerator dbModelGenerator = new EfModelContentGenerator();
			var updatedModel = dbModelGenerator.GetModelDataWithDbProps(jsonModel);

			var api = new ApiProjectGenerator<CsData>(updatedModel);
			await api.GenerateProjectFor("CSharp");

			IModelGenerator modelGenerator = new CSharpFileContentGenerator();
			var classCode = await modelGenerator.GenerateModelContentAsync(jsonModel);


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