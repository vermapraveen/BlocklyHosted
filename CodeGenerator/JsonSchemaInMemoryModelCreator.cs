using System.Collections.Generic;
using System.Linq;

using ModelGenerator.CSharp;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CodeGenerator
{
	public interface IJsonSchemaInMemoryModelCreator
	{
		CsData GetJsonModel(string jsonSchema);
	}

	public class JsonSchemaInMemoryModelCreator : IJsonSchemaInMemoryModelCreator
	{
		public CsData GetJsonModel(string jsonSchema)
		{
			var root = (JObject)JsonConvert.DeserializeObject(jsonSchema);

			if (root["definitions"] is not JObject parentNode)
			{
				parentNode = root["properties"] as JObject;
			}

			CsData classStructure = new CsData();

			foreach (var classLevelObject in parentNode.Children())
			{
				string classLevelObjectName = ((JProperty)classLevelObject).Name;
				var aClass = classStructure.AddNewClass(classLevelObjectName);
				var node = root["definitions"][classLevelObjectName];

				var propStructCollection = GetPropertyNode(node);

				foreach (var propStruct in propStructCollection)
				{
					aClass.AddNewProp(propStruct.Name, CsTypeChecker.CheckType(propStruct.Type));
				}
			}

			return classStructure;
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
				propStruct.Type = propToken["$ref"].ToString()["#/definitions/".Length..];
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
}
