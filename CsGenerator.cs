
using System;
using System.IO;
using System.Text;

using DotLiquid;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlkHost.Pages
{
	public class CsGenerator
	{
		public void Generate(string jsonSchema)
		{

			// Schema --> Parsed Object --> C# class
			var root = (JObject)JsonConvert.DeserializeObject(jsonSchema);
			var definitionsToken = root["definitions"] as JObject;
			var mainType = ((string)root["$ref"]).Substring("#/definitions/".Length);

			CsData classStructure = new CsData();

			int counter = 0;
			foreach (var aChild in definitionsToken.Children())
			{
				string childName = ((JProperty)aChild).Name;
				var aClass = classStructure.AddNewClass(childName);
				foreach (var aProp in root["definitions"][childName]["properties"].Children())
				{
					string aPropName = ((JProperty)aProp).Name;
					string isPropNullable = "";
					string propTypeName = "";
					bool isComplexProp = false;

					var prop = (JObject)root["definitions"][childName]["properties"][aPropName];

					if (prop.ContainsKey("anyOf"))
					{
						foreach (var aChildOfAnyType in (prop["anyOf"] as JArray).Children())
						{
							var aType = aChildOfAnyType["type"];
							if (aType.ToString() == JsonTypeStrings.Null)
							{
								isPropNullable = "Nullable";
							}
							else if (aType.ToString() == JsonTypeStrings.Array)
							{
								var arrayType = prop["anyOf"]["items"]["$ref"];
								propTypeName = $"{((string)arrayType).Substring("#/definitions/".Length)}";
								isComplexProp = true;
							}

							else
							{
								var format = aChildOfAnyType["format"];
								propTypeName = string.IsNullOrEmpty((string)format) ? (string)aType : $"{format}";
							}
						}
					}

					else if (prop.ContainsKey("$ref"))
					{
						var refType = root["definitions"][childName]["properties"][aPropName]["$ref"];
						propTypeName = $"{((string)refType).Substring("#/definitions/".Length)}";
						isComplexProp = true;
					}
					else if (prop.ContainsKey("type"))
					{
						var type = root["definitions"][childName]["properties"][aPropName]["type"];

						if (type.ToString() == JsonTypeStrings.Null)
						{
							isPropNullable = "Nullable";
						}
						else if (type.ToString() == JsonTypeStrings.Array)
						{
							var arrayType = root["definitions"][childName]["properties"][aPropName]["items"]["$ref"];
							propTypeName = $"{((string)arrayType).Substring("#/definitions/".Length)}";
							isComplexProp = true;
						}
						else
						{
							var format = root["definitions"][childName]["properties"][aPropName]["format"];
							propTypeName = string.IsNullOrEmpty((string)format) ? (string)type : $"{format}";
						}
					}

					//Console.WriteLine($"{childName}.{aPropName} [{propTypeName} {isPropNullable}]");
					aClass.AddNewProp(aPropName, isComplexProp ? propTypeName : CsTypeChecker.CheckType(propTypeName));

					counter++;
				}
			}

			Console.WriteLine($"Total props added {counter}");

			Template template = Template.Parse(File.ReadAllText("classCs.liquid"));
			var updated = template.Render(Hash.FromAnonymousObject(new { csData = classStructure })); // Renders the output => "hi tobi"
			Console.WriteLine(updated);

			using (FileStream fs = File.Create($"{mainType}.cs"))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(updated);
				fs.Write(info, 0, info.Length);
			}
		}
	}
}