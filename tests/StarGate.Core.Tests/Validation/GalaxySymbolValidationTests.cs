using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class GalaxySymbolValidationTests
{
    [TestMethod]
    public void Validation_WithValidTagName_Passes()
    {
        var symbol = new GalaxySymbol
        {
            TagName = "Symbol_001"
        };

        var result = symbol.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Validation_WithEmptyTagName_Fails()
    {
        var symbol = new GalaxySymbol
        {
            TagName = string.Empty
        };

        var result = symbol.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "The TagName field is required.");
    }
}
