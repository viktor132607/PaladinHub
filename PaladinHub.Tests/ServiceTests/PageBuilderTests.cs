using NUnit.Framework;
using PaladinHub.Services.PageBuilder;

namespace PaladinHub.Tests.ServiceTests;

[TestFixture]
public class PageBuilderTests
{
	[Test]
	public void JsonLayoutValidator_Throws_For_Invalid_Json()
	{
		IJsonLayoutValidator validator = new JsonLayoutValidator();
		Assert.Throws<JsonLayoutValidationException>(() => validator.ValidateOrThrow("{ invalid json }"));
	}

	[Test]
	public void JsonLayoutValidator_Allows_Valid_Empty_Array()
	{
		IJsonLayoutValidator validator = new JsonLayoutValidator();
		Assert.DoesNotThrow(() => validator.ValidateOrThrow("[]"));
	}
}
