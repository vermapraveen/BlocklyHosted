
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CodeGenerator.CSharp;

using Common;

using DotLiquid;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodeGenerator
{
	public class JsonSchemaCodeGenerator
	{
		public async System.Threading.Tasks.Task<string> GenerateAsync(string jsonSchema)
		{

			// Schema --> Parsed Object --> C# class
			var root = (JObject)JsonConvert.DeserializeObject(jsonSchema);
			var parentNode = root["definitions"] as JObject;
			var schemaVersion = (string)root["$schema"];
			var title = (string)root["title"];

			//GetLeafNode(root);

			if (null == parentNode)
			{
				parentNode = root["properties"] as JObject;
			}

			var parentNodeTitle = (string)root["title"];
			if (null == parentNodeTitle)
			{
				parentNodeTitle = ((string)root["$ref"]).Substring("#/definitions/".Length);
			}


			CsData classStructure = new CsData();

			int counter = 0;
			foreach (var aChild in parentNode.Children())
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

					aClass.AddNewProp(aPropName, isComplexProp ? propTypeName : CsTypeChecker.CheckType(propTypeName));

					counter++;
				}
			}

			try
			{
				Template template = Template.Parse(await FileUtils.GetFileContent("CSharp/Templates/classCs.liquid"));
				var updated = template.Render(Hash.FromAnonymousObject(new { csData = classStructure }));

				return updated;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public static IEnumerable<PropertyStructure> GetPropertyNode(JObject node)
		{
			List<PropertyStructure> props = new List<PropertyStructure>();

			foreach (JProperty propNode in node["properties"])
			{
				var aProp = node["properties"][propNode.Name];

				var propStruct = FindNonLeafLevelPropType(aProp);
				propStruct.Name = propNode.Name;
				props.Add(propStruct);
			}

			return props;
		}

		private static PropertyStructure FindNonLeafLevelPropType(JToken aProp)
		{
			PropertyStructure propStruct = new PropertyStructure();

			string prop;

			if (IsArrayNode(aProp))
			{
				propStruct.IsCollection = true;
				aProp = aProp["items"];

				prop = FindLeafLevelPropType(aProp);
			}
			else if (IsAnyOfNode(aProp))
			{
				List<PropertyStructure> subProps = new List<PropertyStructure>();
				foreach (var aChildOfAnyType in aProp["anyOf"])
				{
					PropertyStructure subPropStruct = new PropertyStructure();
					var subProp = FindLeafLevelPropType(aChildOfAnyType);

					subProps.Add(subPropStruct);
				}
			}
			else if (IsNullTypeNode(aProp))
			{
				prop = "";
				propStruct.IsNullable = true;
			}
			else
			{
				prop = FindLeafLevelPropType(aProp);
			}

			propStruct.Type = prop;
			return propStruct;
		}

		private static string FindLeafLevelPropType(JToken propToken)
		{
			string prop = null;
			if (IsTypeNode(propToken))
			{
				if (IsStringTypeNodeHasFormatNode(propToken))
				{
					prop = propToken["format"].ToString();
				}
				else
				{
					prop = propToken["type"].ToString();
				}
			}
			else if (IsComplexNode(propToken))
			{
				prop = propToken["$ref"].ToString().Substring("#/definitions/".Length);
			}

			return prop;
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
		public IEnumerable<PropertyStructure> AnyOfProperty { get; set; }
	}
}