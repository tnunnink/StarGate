using System.Diagnostics;
using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
[UsesVerify]
public partial class GalaxyRepositoryTests : GalaxyFixture
{
    [TestMethod]
    public void GetObjects_SingleInstanceObject_ShouldReturnSuccess()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("TestInstance");
        stopwatch.Stop();

        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    [TestMethod]
    public void GetObjects_SingleTemplateObject_ShouldReturnSuccess()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("$TestTemplate");
        stopwatch.Stop();

        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    [TestMethod]
    public void GetObjects_InstanceObjectAndSymbol_ShouldReturnSuccess()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("TestInstance", "TestGraphic");
        stopwatch.Stop();

        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    [TestMethod]
    public Task GetObjects_KnownInstance_ShouldHaveExpectedFormat()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("TestInstance");
        stopwatch.Stop();

        result.Success.Should().BeTrue();

        var serializer = new GalaxySerializer();
        var yaml = serializer.Serialize(result.Value);
        return Verify(yaml);
    }

    [TestMethod]
    public Task GetObjects_KnownTemplate_ShouldBeVerified()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("$TestTemplate");
        stopwatch.Stop();

        result.Success.Should().BeTrue();

        var serializer = new GalaxySerializer();
        var yaml = serializer.Serialize(result.Value);
        return Verify(yaml);
    }
}