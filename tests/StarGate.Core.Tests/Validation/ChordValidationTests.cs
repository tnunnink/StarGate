using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class ChordValidationTests
{
    [TestMethod]
    [DataRow(-360.0)]
    [DataRow(-180.0)]
    [DataRow(0.0)]
    [DataRow(180.0)]
    [DataRow(360.0)]
    public void Validation_WithValidAngles_Passes(double angle)
    {
        var element = new ChordElement
        {
            Name = "ValidChord",
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
        var element = new ChordElement
        {
            Name = "InvalidChord",
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
        var element = new ChordElement
        {
            Name = "InvalidChord",
            StartAngle = 0.0,
            SweepAngle = angle
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "SweepAngle must be between -360.0 and 360.0 degrees.");
    }
}
