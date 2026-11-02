using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class GalaxyExtensionTests : GalaxyTestFixture
{
    [Test]
    public void EnsureObjectCreated_ExistingMatchingInstance_ShouldReturnExistingWithoutCreating()
    {
        var existing = GetInstance(TestInstance);
        var definition = new GalaxyObject
        {
            TagName = TestInstance,
            Template = existing.DerivedFrom,
            BasedOn = existing.basedOn
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeTrue();
        created.Should().BeFalse();
        result.Value.Tagname.Should().Be(TestInstance);
    }

    [Test]
    public void EnsureObjectCreated_ExistingObjectWithDifferentTemplate_ShouldFail()
    {
        var definition = new GalaxyObject
        {
            TagName = TestInstance,
            Template = "$SomeOtherTemplate"
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeFalse();
        created.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ErrorCode.ObjectNotFound);
    }

    [Test]
    public void EnsureObjectCreated_TemplateDoesNotExist_ShouldFailWithObjectNotFound()
    {
        var definition = new GalaxyObject
        {
            TagName = "TempEnsureMissingTemplate",
            Template = "$DoesNotExist"
        };

        var result = Galaxy.EnsureObjectCreated(definition, out var created);

        result.Success.Should().BeFalse();
        created.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == ErrorCode.ObjectNotFound);
        result.Errors.Should().Contain(e => e.Message.Contains("$DoesNotExist"));
    }

    [Test]
    public void EnsureObjectCreated_NewInstanceFromExistingTemplate_ShouldCreateObject()
    {
        const string tagName = "TempEnsureCreatedInstance";
        var definition = new GalaxyObject { TagName = tagName, Template = TestTemplate };

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

    [Test]
    public void EnsureObjectCreated_NewTemplateFromExistingTemplate_ShouldCreateTemplate()
    {
        const string tagName = "$TempEnsureCreatedTemplate";
        var definition = new GalaxyObject { TagName = tagName, Template = TestTemplate };

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

    [Test]
    public void EnsureObjectCreated_CalledTwice_ShouldOnlyCreateOnce()
    {
        const string tagName = "TempEnsureTwice";
        var definition = new GalaxyObject { TagName = tagName, Template = TestTemplate };

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