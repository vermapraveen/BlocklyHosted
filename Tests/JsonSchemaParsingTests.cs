using System.Linq;

using CodeGenerator;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Shouldly;

using Xunit;

namespace Tests
{
	public class JsonSchemaParsingTests
	{
		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsSimpleType_ShouldAbleGetProperty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"firstName\":{\"type\":\"string\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

			props.Count().ShouldBe(3);
			props.First().IsNullable.ShouldBeFalse();
			props.First().IsCollection.ShouldBeFalse();
		}

		[Fact]
		public void GivenObjectNodeHasSingleProperty_WhenPropertyIsComplex_ShouldAbleGetProperty()
		{
			const string aPropJsonSchema = "{\"properties\":{\"sellerContext\":{\"$ref\":\"#/definitions/SellerContext\"}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

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
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

			props.ShouldHaveSingleItem();
			props.First().Name.ShouldBe("instanceId");
			props.First().Type.ShouldBe(JsonTypeStrings.Integer);
			props.First().IsNullable.ShouldBeTrue();
			props.First().IsCollection.ShouldBeFalse();
		}



		[Fact]
		public void GivenObjectNodeHasMultipleTypeOfProperties_ShouldAbleToGetAllProps()
		{
			const string aPropJsonSchema = "{\"properties\":{\"firstName\":{\"type\":\"string\"},\"lastName\":{\"type\":\"null\"},\"sellerContext\":{\"$ref\":\"#/definitions/SellerContext\"},\"age\":{\"type\":\"integer\"},\"instanceId\":{\"anyOf\":[{\"type\":\"null\"},{\"type\":\"integer\"}]},\"deltaSelections\":{\"type\":\"array\",\"items\":{\"$ref\":\"#/definitions/DeltaSelection\"}}}}";
			var aPropObject = (JObject)JsonConvert.DeserializeObject(aPropJsonSchema);
			var props = JsonSchemaInMemoryModelCreator.GetPropertyNode(aPropObject);

			props.Count().ShouldBe(6);
			props.Any(x => x.Name == "firstName" && x.Type == JsonTypeStrings.String && !x.IsNullable && !x.IsCollection).ShouldBeTrue();
			props.Any(x => x.Name == "lastName" && x.Type == "" && x.IsNullable && !x.IsCollection).ShouldBeTrue();
			props.Any(x => x.Name == "sellerContext" && x.Type == "SellerContext" && !x.IsNullable && !x.IsCollection).ShouldBeTrue();
			props.Any(x => x.Name == "age" && x.Type == JsonTypeStrings.Integer && !x.IsNullable && !x.IsCollection).ShouldBeTrue();
			props.Any(x => x.Name == "instanceId" && x.Type == JsonTypeStrings.Integer && x.IsNullable && !x.IsCollection).ShouldBeTrue();
			props.Any(x => x.Name == "deltaSelections" && x.Type == "DeltaSelection" && !x.IsNullable && x.IsCollection).ShouldBeTrue();
		}
	}

}
