using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core.Serialization;
using VerifyNUnit;

namespace StarGate.Galaxy.Test;

[TestFixture]
public class GalaxyRepositoryTests : GalaxyTestFixture
{
    [Test]
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

    [Test]
    public void GetObjects_SingleSymbolObject_ShouldReturnSuccess()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("TestGraphic");
        stopwatch.Stop();

        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    }

    [Test]
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

    [Test]
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

    [Test]
    public Task GetObjects_KnownInstance_ShouldHaveExpectedFormat()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("TestInstance");
        stopwatch.Stop();

        result.Success.Should().BeTrue();

        var serializer = new GalaxySerializer();
        var yaml = serializer.Serialize(result.Value);
        return Verifier.Verify(yaml);
    }
    
    [Test]
    public Task GetObjects_KnownTemplate_ShouldBeVerified()
    {
        var repo = new GalaxyRepository(Galaxy);

        var stopwatch = Stopwatch.StartNew();
        var result = repo.GetObjects("$TestTemplate");
        stopwatch.Stop();

        result.Success.Should().BeTrue();

        var serializer = new GalaxySerializer();
        var yaml = serializer.Serialize(result.Value);
        return Verifier.Verify(yaml);
    }
}