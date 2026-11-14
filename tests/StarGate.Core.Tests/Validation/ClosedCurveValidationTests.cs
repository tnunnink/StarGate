using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class ClosedCurveValidationTests
{
    [TestMethod]
    [DataRow(0.0)]
    [DataRow(0.5)]
    [DataRow(1.0)]
    [DataRow(2.0)]
    public void Validation_WithValidTension_Passes(double tension)
    {
        var element = new ClosedCurveElement
        {
            Name = "ValidClosedCurve",
            Points = [new GraphicPoint(0, 0), new GraphicPoint(10, 10)],
            Tension = tension
        };

        var result = element.Validate();

        result.Success.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(-0.1)]
    [DataRow(2.1)]
    public void Validation_WithInvalidTension_Fails(double tension)
    {
        var element = new ClosedCurveElement
        {
            Name = "InvalidClosedCurve",
            Points = [new GraphicPoint(0, 0), new GraphicPoint(10, 10)],
            Tension = tension
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Tension must be between 0.0 and 2.0.");
    }

    [TestMethod]
    public void Validation_WithFewerThanTwoPoints_Fails()
    {
        var element = new ClosedCurveElement
        {
            Name = "TooFewPointsClosedCurve",
            Points = [new GraphicPoint(0, 0)]
        };

        var result = element.Validate();

        result.Success.Should().BeFalse();
        result.Errors.Should().ContainSingle(r => r.Message == "Closed curve requires at least 2 points.");
    }
}
