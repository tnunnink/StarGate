using FluentAssertions;

namespace StarGate.Core.Tests.Validation;

[TestClass]
public class LineValidationTests
{
    [TestMethod]
    public void Validation_WithValidLine_Passes()
    {
        var line = new LineElement
        {
            Name = "ValidLine",
            Start = new GraphicPoint(0, 0),
            End = new GraphicPoint(100, 100)
        };

        var result = line.Validate();

        result.Success.Should().BeTrue();
    }
}
