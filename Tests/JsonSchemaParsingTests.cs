using System.Threading.Tasks;

using BlkHost.Pages;

using CodeGenerator;

using Common;

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
			var expectedContent = await FileUtils.GetFileContent("data\\selectionMatrix.txt");
			generatedContent.ShouldBe(expectedContent);
		}
	}
}
