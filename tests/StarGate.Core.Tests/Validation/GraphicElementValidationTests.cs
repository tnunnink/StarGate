using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class GraphicElementValidationTests
{
    [TestMethod]
    public void Validation_WithValidNameLength_Passes()
    {
        var element = new RectangleElement
        {
            Name = new string('A', 373)
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Validation_WithNameExceedingMaxLength_Fails()
    {
        var element = new RectangleElement
        {
            Name = new string('A', 374)
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Element name cannot exceed 373 characters.");
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(180)]
    [DataRow(360)]
    public void Validation_WithValidAngle_Passes(int angle)
    {
        var element = new RectangleElement
        {
            Name = "Rect",
            Angle = angle
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(360)]
    public void Validation_WithInvalidAngle_Fails(int angle)
    {
        var element = new RectangleElement
        {
            Name = "Rect",
            Angle = angle
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Angle must be between 0.0 and 360.0 degrees.");
    }

    [TestMethod]
    public void Validation_WithNegativeTabOrder_Fails()
    {
        var element = new RectangleElement
        {
            Name = "Rect",
            TabOrder = -1
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "TabOrder must be a non-negative integer.");
    }
}
