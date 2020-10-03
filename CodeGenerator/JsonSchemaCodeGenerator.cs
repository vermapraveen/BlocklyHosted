using System.Collections.Generic;
using System.Linq;

using ModelGenerator.CSharp;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ModelGenerator;

namespace CodeGenerator
{
	public class JsonSchemaCodeGenerator
	{
		public async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{

			// Schema --> Parsed Object --> C# class
			var root = (JObject)JsonConvert.DeserializeObject(jsonSchema);
			var parentNode = root["definitions"] as JObject;

			if (null == parentNode)
			{
				parentNode = root["properties"] as JObject;
			}

			CsData classStructure = new CsData();

			foreach (var aChild in parentNode.Children())
			{
				string childName = ((JProperty)aChild).Name;
				var aClass = classStructure.AddNewClass(childName);
				var node = root["definitions"][childName];

				var propStructCollection = GetPropertyNode(node);

				foreach (var propStruct in propStructCollection)
				{
					aClass.AddNewProp(propStruct.Name, CsTypeChecker.CheckType(propStruct.Type));
				}
			}

			IModelGenerator modelGenerator = new CSharpFileContentGenerator();

			//Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));
			//var updated = template.Render(Hash.FromAnonymousObject(new { csData = classStructure }));

			return await modelGenerator.GenerateModelContentAsync(classStructure);
		}

		public static IEnumerable<PropertyStructure> GetPropertyNode(JToken node)
		{
			List<PropertyStructure> props = new List<PropertyStructure>();

			foreach (JProperty propNode in node["properties"])
			{
				var propStruct = FindNonLeafLevelPropType(node["properties"][propNode.Name]);
				propStruct.Name = propNode.Name;
				props.Add(propStruct);
			}

			return props;
		}

		private static PropertyStructure FindNonLeafLevelPropType(JToken aProp)
		{
			PropertyStructure propStruct;

			if (IsArrayNode(aProp))
			{
				aProp = aProp["items"];
				propStruct = FindLeafLevelPropType(aProp);
				propStruct.IsCollection = true;
			}
			else if (IsAnyOfNode(aProp))
			{
				propStruct = FindLeafLevelPropType(aProp);
				List<PropertyStructure> subProps = new List<PropertyStructure>();
				foreach (var aChildOfAnyType in aProp["anyOf"])
				{
					var subProp = FindLeafLevelPropType(aChildOfAnyType);
					subProps.Add(subProp);
				}

				propStruct.IsNullable = subProps.Any(x => x.IsNullable);
				propStruct.Type = subProps.First(x => !string.IsNullOrEmpty(x.Type)).Type;
			}
			else
			{
				propStruct = FindLeafLevelPropType(aProp);
			}

			return propStruct;
		}

		private static PropertyStructure FindLeafLevelPropType(JToken propToken)
		{
			PropertyStructure propStruct = new PropertyStructure();

			if (IsNullTypeNode(propToken))
			{
				propStruct = new PropertyStructure
				{
					IsNullable = true,
					Type = ""
				};
			}
			else if (IsTypeNode(propToken))
			{
				if (IsStringTypeNodeHasFormatNode(propToken))
				{
					propStruct.Type = propToken["format"].ToString();
				}
				else
				{
					propStruct.Type = propToken["type"].ToString();
				}
			}
			else if (IsComplexNode(propToken))
			{
				propStruct.Type = propToken["$ref"].ToString().Substring("#/definitions/".Length);
			}

			return propStruct;
		}

		private static bool IsTypeNode(JToken node)
		{
			return node["type"] != null;
		}

		private static bool IsAnyOfNode(JToken node)
		{
			return node["anyOf"] != null;
		}

		private static bool IsNullTypeNode(JToken node)
		{
			return node["type"] != null && (node["type"].ToString() == JsonTypeStrings.Null || string.IsNullOrEmpty(node["type"].ToString()));
		}

		private static bool IsArrayNode(JToken node)
		{
			return node["type"] != null && node["type"].ToString() == JsonTypeStrings.Array;
		}

		private static bool IsComplexNode(JToken node)
		{
			return node["$ref"] != null;
		}

		private static bool IsStringTypeNodeHasFormatNode(JToken node)
		{
			return node["type"].ToString() == JsonTypeStrings.String
				&& node["format"] != null;
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