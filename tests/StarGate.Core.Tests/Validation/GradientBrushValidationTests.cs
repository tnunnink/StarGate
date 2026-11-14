using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class GradientBrushValidationTests
{
    [TestMethod]
    public void Validation_WithValidConfiguration_Passes()
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White, GraphicColor.Black],
            Angle = 90.0,
            Center = 0.5f,
            Falloff = 1.0f,
            FocusScale = 0.0f
        };

        var result = brush.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    public void Validation_WithEmptyStops_Fails()
    {
        var brush = new GradientBrush
        {
            Stops = []
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Gradient stops must contain at least 1 color.");
    }

    [TestMethod]
    public void Validation_WithMoreThanThreeStops_Fails()
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White, GraphicColor.Black, GraphicColor.White, GraphicColor.Black]
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Gradient stops cannot contain more than 3 colors.");
    }

    [TestMethod]
    [DataRow(-0.1)]
    [DataRow(360.1)]
    public void Validation_WithInvalidAngle_Fails(double angle)
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White],
            Angle = angle
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Angle must be between 0.0 and 360.0.");
    }

    [TestMethod]
    [DataRow(-0.1f)]
    [DataRow(1.1f)]
    public void Validation_WithInvalidCenter_Fails(float center)
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White],
            Center = center
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Center must be between 0.0 and 1.0.");
    }

    [TestMethod]
    [DataRow(-0.1f)]
    [DataRow(1.1f)]
    public void Validation_WithInvalidFalloff_Fails(float falloff)
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White],
            Falloff = falloff
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Falloff must be between 0.0 and 1.0.");
    }

    [TestMethod]
    [DataRow(-0.1f)]
    [DataRow(1.1f)]
    public void Validation_WithInvalidFocusScale_Fails(float focusScale)
    {
        var brush = new GradientBrush
        {
            Stops = [GraphicColor.White],
            FocusScale = focusScale
        };

        var result = brush.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "FocusScale must be between 0.0 and 1.0.");
    }
}
