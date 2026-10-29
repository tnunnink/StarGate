using System.Linq;
using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Galaxy.Extensions;

namespace StarGate.Galaxy.Test.Extensions;

[TestFixture]
public class CollectionExtensionTests : GalaxyTestFixture
{
    [Test]
    public void AsEnumerable_Objects_ShouldReturnAllItems()
    {
        var objects = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [TestInstance, EmptyInstance]);

        var result = objects.AsEnumerable().ToList();

        result.Should().HaveCount(2);
        result.Select(x => x.Tagname).Should().BeEquivalentTo(TestInstance, EmptyInstance);
    }

    [Test]
    public void AsEnumerable_Attributes_ShouldReturnAllItems()
    {
        var instance = GetInstance(TestInstance);

        var result = instance.Attributes.AsEnumerable().ToList();

        result.Should().NotBeEmpty();
        result.Count.Should().Be(instance.Attributes.count);
        result.Select(x => x.Name).Should().Contain(["TestInt", "TestString", "TestArray"]);
    }

    [Test]
    public void AsEnumerable_Attributes_ShouldBeEnumerableMultipleTimes()
    {
        var instance = GetInstance(TestInstance);
        var attributes = instance.Attributes.AsEnumerable();

        attributes.Count().Should().Be(attributes.Count());
    }
}
