namespace StarGate.Core;

[TestClass]
public class FIllStyleTests : VerifyBase
{
    [TestMethod]
    public Task SerializeDefaultShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle();

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }
    
    [TestMethod]
    public Task SerializeWithBrushShouldHaveVerifiedOutput()
    {
        var serializer = new GalaxySerializer();
        var fill = new FillStyle
        {
            Brush = "123 223 231"
        };

        var yaml = serializer.Serialize(fill);

        return Verify(yaml);
    }
}