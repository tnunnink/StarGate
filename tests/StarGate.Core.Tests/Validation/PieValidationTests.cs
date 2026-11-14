using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class PieValidationTests
{
    [TestMethod]
    [DataRow(-360.0)]
    [DataRow(-180.0)]
    [DataRow(0.0)]
    [DataRow(180.0)]
    [DataRow(360.0)]
    public void Validation_WithValidAngles_Passes(double angle)
    {
        var element = new PieElement
        {
            Name = "ValidPie",
            StartAngle = angle,
            SweepAngle = angle
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(-361.0)]
    [DataRow(361.0)]
    public void Validation_WithInvalidStartAngle_Fails(double angle)
    {
        var element = new PieElement
        {
            Name = "InvalidPie",
            StartAngle = angle,
            SweepAngle = 0.0
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "StartAngle must be between -360.0 and 360.0 degrees.");
    }

    [TestMethod]
    [DataRow(-361.0)]
    [DataRow(361.0)]
    public void Validation_WithInvalidSweepAngle_Fails(double angle)
    {
        var element = new PieElement
        {
            Name = "InvalidPie",
            StartAngle = 0.0,
            SweepAngle = angle
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "SweepAngle must be between -360.0 and 360.0 degrees.");
    }
}
