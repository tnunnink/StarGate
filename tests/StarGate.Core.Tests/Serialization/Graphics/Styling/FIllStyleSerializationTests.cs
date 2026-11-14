namespace StarGate.Core;

[TestClass]
public class FIllStyleSerializationTests : VerifyBase
{
    [TestMethod]
    public Task SerializeDefaultShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle();

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }

    [TestMethod]
    [DataRow("123 223 231")]
    [DataRow("#35A6E2")]
    [DataRow("35A6E2")]
    public Task SerializeWithDifferentColorFormatsShouldBeVerified(string colorInput)
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle
        {
            Brush = colorInput
        };

        var yaml = serializer.Serialize(fill);

        return Verify(yaml).UseParameters(colorInput);
    }

    [TestMethod]
    public Task SerializeWithTransparentDefaultColorBrushShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle
        {
            // Same default White color, but with transparency > 0 => should serialize
            Brush = new SolidBrush { Color = GraphicColor.White, Transparency = 25.0 }
        };

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeWithNonDefaultUnfilledBrushShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle
        {
            Unfilled = "#FF0000"
        };

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }

    [TestMethod]
    public Task SerializeWithPartialFillPercentagesShouldBeVerified()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle
        {
            Horizontal = 50,
            Vertical = 25,
            Direction = FillDirection.DownLeft,
            Orientation = FillOrientation.RelativeToScreen
        };

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }
}