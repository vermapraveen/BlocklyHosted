using System.Linq;
using System.Threading.Tasks;

using BlkHost.Pages;

using CodeGenerator;

using Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Shouldly;

using Xunit;

namespace Tests
{
	public class JsonSchemaParsingTests
	{
		[Fact]
		public async Task GivenValidJsonSchema_ShouldAbleToCreateCsClassAsync()
		{
			const string jsonSchema = "{\"$schema\":\"http://json-schema.org/draft-06/schema#\",\"$ref\":\"#/definitions/SelectionMatrix\",\"definitions\":{\"SelectionMatrix\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"items\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/SelectionMatrixItem\"}},\"connections\":{\"type\":\"null\"},\"multiplierQuantity\":{\"type\":\"integer\"},\"isStaging\":{\"type\":\"boolean\"},\"relationshipContext\":{\"type\":\"null\"},\"sellerContext\":{\"$ref\":\"#/definitions/SellerContext\"}},\"required\":[\"connections\",\"isStaging\",\"items\",\"multiplierQuantity\",\"relationshipContext\",\"sellerContext\"],\"title\":\"SelectionMatrix\"},\"SelectionMatrixItem\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"id\":{\"type\":\"string\"},\"instanceId\":{\"type\":\"string\",\"format\":\"uuid\"},\"deltaSelections\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/DeltaSelection\"}},\"items\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/ItemItem\"}}},\"required\":[\"deltaSelections\",\"id\",\"instanceId\",\"items\"],\"title\":\"SelectionMatrixItem\"},\"DeltaSelection\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"actions\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/Action\"}},\"id\":{\"type\":\"string\"},\"instanceId\":{\"anyOf\":[{\"type\":\"null\"},{\"type\":\"string\",\"format\":\"uuid\"}]},\"path\":{\"type\":\"string\"}},\"required\":[\"actions\",\"id\",\"instanceId\",\"path\"],\"title\":\"DeltaSelection\"},\"Action\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"type\":{\"type\":\"string\"},\"value\":{\"type\":\"integer\"}},\"required\":[\"type\",\"value\"],\"title\":\"Action\"},\"ItemItem\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"id\":{\"type\":\"string\"},\"instanceId\":{\"type\":\"string\",\"format\":\"uuid\"},\"deltaSelections\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/DeltaSelection\"}},\"options\":{\"$ref\":\"#/definitions/Options\"},\"path\":{\"type\":\"string\"}},\"required\":[\"deltaSelections\",\"id\",\"instanceId\",\"options\",\"path\"],\"title\":\"ItemItem\"},\"Options\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"subscriptionUpgrade\":{\"type\":\"string\",\"format\":\"boolean\"}},\"required\":[\"subscriptionUpgrade\"],\"title\":\"Options\"},\"SellerContext\":{\"type\":\"object\",\"additionalProperties\":false,\"properties\":{\"region\":{\"type\":\"string\"},\"country\":{\"type\":\"string\"},\"language\":{\"type\":\"string\"},\"currency\":{\"type\":\"string\"},\"customerSet\":{\"type\":\"string\"},\"segment\":{\"type\":\"string\"},\"sourceApplicationName\":{\"type\":\"string\"},\"companyNumber\":{\"type\":\"string\",\"format\":\"integer\"},\"businessUnitId\":{\"type\":\"string\",\"format\":\"integer\"}},\"required\":[\"businessUnitId\",\"companyNumber\",\"country\",\"currency\",\"customerSet\",\"language\",\"region\",\"segment\",\"sourceApplicationName\"],\"title\":\"SellerContext\"}}}";


			var gen = new JsonSchemaCodeGenerator();
			var generatedContent = await gen.GenerateAsync(jsonSchema);
			var expectedContent = await FileUtils.GetFileContent("data/selectionMatrix.txt");
			generatedContent.ShouldBe(expectedContent);
		}

		//[Fact]
		//public async Task GivenValidJsonSchema_WhenOnlyRootLevel_NonComplex_Props_ShouldAbleToCreateCsClassAsync()
		//{
		//	const string jsonSchema = "{\"$id\":\"https://example.com/person.schema.json\",\"$schema\":\"http://json-schema.org/draft-07/schema#\",\"title\":\"Person\",\"type\":\"object\",\"properties\":{\"firstName\":{\"type\":\"string\",\"description\":\"The person's first name.\"},\"lastName\":{\"type\":\"string\",\"description\":\"The person's last name.\"},\"age\":{\"description\":\"Age in years which must be equal to or greater than zero.\",\"type\":\"integer\",\"minimum\":0}}}";


		//	var gen = new JsonSchemaCodeGenerator();
		//	var generatedContent = await gen.GenerateAsync(jsonSchema);
		//	var expectedContent = await FileUtils.GetFileContent("data/OnlyRootLevel_NonComplex.txt");
		//	generatedContent.ShouldBe(expectedContent);
		//}

		[Fact]
		public async Task GivenRootNode_WhenTypeIsObject_ShouldAbleGetPropertiesOfObject()
		{
			const string jsonSchema = "{\"$id\":\"https://example.com/person.schema.json\",\"$schema\":\"http://json-schema.org/draft-07/schema#\",\"title\":\"Person\",\"type\":\"object\",\"properties\":{\"firstName\":{\"type\":\"string\",\"description\":\"The person's first name.\"},\"lastName\":{\"type\":\"string\",\"description\":\"The person's last name.\"},\"age\":{\"description\":\"Age in years which must be equal to or greater than zero.\",\"type\":\"integer\",\"minimum\":0}}}";


		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsSimpleType_ShouldAbleGetProperty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("firstName");
			props.First().Type.ShouldBe("string");
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeFalse();
		}


		[Fact]
		public void GivenObjectNodeHasMultipleProperties_WhenPropertyIsSimpleType_ShouldAbleGetProperty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"firstName\":{\"type\":\"string\"},\"lastName\":{\"type\":\"string\"},\"age\":{\"type\":\"integer\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.Count().ShouldBe(3);
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeFalse();
		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsComplex_ShouldAbleGetProperty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"sellerContext\":{\"$ref\":\"#/definitions/SellerContext\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("sellerContext");
			props.First().Type.ShouldBe("SellerContext");
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeFalse();
		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsNull_ShouldAbleGetPropertyAsEmpty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"connections\":{\"type\":null}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("connections");
			props.First().Type.ShouldBe("");
			props.First().IsNullable.ShouldBeTrue();
			props.First().IsCollection.ShouldBeFalse();
		}


		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsNullString_ShouldAbleGetPropertyAsEmpty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"connections\":{\"type\":\"null\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("connections");
			props.First().Type.ShouldBe("");
			props.First().IsNullable.ShouldBeTrue();
			props.First().IsCollection.ShouldBeFalse();
		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsArrayOfSimpleType_ShouldAbleGetPropertyAsEmpty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"connectionsStrings\":{\"type\":\"array\",\"items\":{\"type\":\"string\"}}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("connectionsStrings");
			props.First().Type.ShouldBe("string");
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeTrue();
		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsArrayOfComplexType_ShouldAbleGetPropertyAsEmpty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"Selections\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/SelectionItem\"}}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("Selections");
			props.First().Type.ShouldBe("SelectionItem");
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeTrue();
		}

		[Fact]
		public void GivenObjectNodeHasSingleStringTypeProperty_WhenPropertyHasFormatNodeValueUuid_ShouldSetPropertyTypeAsUuid()
		{
			const string aPropJsonSchema = "{\"properties\":{\"instanceId\":{\"type\":\"string\",\"format\":\"uuid\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("instanceId");
			props.First().Type.ShouldBe("uuid");
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeFalse();
		}

		[Fact]
		public void GivenObjectNodeHasPropertyWithAnyOf_WhenPropertyHasNullTypeAndInteger_ShouldSetPropertyTypeAsUuid()
		{
			const string aPropJsonSchema = "{\"properties\":{\"instanceId\":{\"anyOf\":[{\"type\":\"null\"},{\"type\":\"integer\"}]}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaCodeGenerator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("instanceId");
			props.First().Type.ShouldBe("integer");
			props.First().IsNullable.ShouldBeTrue();
			props.First().IsCollection.ShouldBeFalse();
		}
	}

}
