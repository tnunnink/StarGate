using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class RectangleValidationTests
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(35)]
    [DataRow(70)]
    public void Validation_WithValidRadius_Passes(int? radius)
    {
        var element = new RectangleElement
        {
            Name = "ValidRadiusRect",
            Radius = radius
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(-10)]
    [DataRow(71)]
    [DataRow(100)]
    public void Validation_WithInvalidRadius_Fails(int? radius)
    {
        var element = new RectangleElement
        {
            Name = "InvalidRadiusRect",
            Radius = radius
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Corner radius must be between 0 and 70.");
    }
}