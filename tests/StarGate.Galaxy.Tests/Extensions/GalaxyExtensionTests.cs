using FluentAssertions;
using StarGate.Core;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class GalaxyExtensionTests : GalaxyFixture
{
    [TestMethod]
    public void GetSymbols_ValidSymbolName_ShouldHaveExpectedCount()
    {
        var symbol = Galaxy.GetSymbols("TestGraphic").First();

        Console.WriteLine($"Tagname: {symbol.Tagname}");
        Console.WriteLine($"HierarchicalName: {symbol.HierarchicalName}");
        Console.WriteLine($"_InternalName: {symbol.Attributes["_InternalName"]?.GetValue()}");
        Console.WriteLine($"_ExternalName: {symbol.Attributes["_ExternalName"]?.GetValue()}");
        Console.WriteLine($"ContainedName: {symbol.ContainedName}");
        Console.WriteLine($"Container: {symbol.Container}");
    }

    [TestMethod]
    public void EnsureObjectCreated_ExistingMatchingInstance_ShouldReturnExistingWithoutCreating()
    {
        var existing = GetInstance(TestInstance);
        var definition = new GalaxyObject
        {
            TagName = TestInstance,
            DerivedFrom = existing.DerivedFrom,
            BasedOn = existing.basedOn
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeTrue();
        created.Should().BeFalse();
        result.Value.Tagname.Should().Be(TestInstance);
    }

    [TestMethod]
    public void EnsureObjectCreated_ExistingObjectWithDifferentTemplate_ShouldFail()
    {
        var definition = new GalaxyObject
        {
            TagName = TestInstance,
            DerivedFrom = "$SomeOtherTemplate"
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeFalse();
        created.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.ObjectNotFound);
    }

    [TestMethod]
    public void EnsureObjectCreated_TemplateDoesNotExist_ShouldFailWithObjectNotFound()
    {
        var definition = new GalaxyObject
        {
            TagName = "TempEnsureMissingTemplate",
            DerivedFrom = "$DoesNotExist"
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeFalse();
        created.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.ObjectNotFound);
        result.Errors.Should().Contain(e => e.Message.Contains("$DoesNotExist"));
    }

    [TestMethod]
    public void EnsureObjectCreated_NewInstanceFromExistingTemplate_ShouldCreateObject()
    {
        const string tagName = "TempEnsureCreatedInstance";
        var definition = new GalaxyObject { TagName = tagName, DerivedFrom = TestTemplate };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        try
        {
            result.Success.Should().BeTrue();
            created.Should().BeTrue();
            result.Value.Tagname.Should().Be(tagName);
            result.Value.IsInstance.Should().BeTrue();
        }
        finally
        {
            GetInstance(tagName).Delete();
        }
    }

    [TestMethod]
    public void EnsureObjectCreated_NewTemplateFromExistingTemplate_ShouldCreateTemplate()
    {
        const string tagName = "$TempEnsureCreatedTemplate";
        var definition = new GalaxyObject { TagName = tagName, DerivedFrom = TestTemplate };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        try
        {
            result.Success.Should().BeTrue();
            created.Should().BeTrue();
            result.Value.IsTemplate.Should().BeTrue();
        }
        finally
        {
            GetTemplate(tagName).Delete();
        }
    }

    [TestMethod]
    public void EnsureObjectCreated_CalledTwice_ShouldOnlyCreateOnce()
    {
        const string tagName = "TempEnsureTwice";
        var definition = new GalaxyObject { TagName = tagName, DerivedFrom = TestTemplate };

        try
        {
            Galaxy.EnsureObjectCreated(definition, out var first);
            var result = Galaxy.EnsureObjectCreated(definition, out var second);

            first.Should().BeTrue();
            second.Should().BeFalse();
            result.Success.Should().BeTrue();
        }
        finally
        {
            GetInstance(tagName).Delete();
        }
    }
}