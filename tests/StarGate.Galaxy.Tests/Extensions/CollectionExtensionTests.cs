using ArchestrA.GRAccess;
using FluentAssertions;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class CollectionExtensionTests : GalaxyFixture
{
    [TestMethod]
    public void AsEnumerable_Objects_ShouldReturnAllItems()
    {
        var objects = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [TestInstance, EmptyInstance]);

        var result = objects.AsEnumerable().ToList();

        result.Should().HaveCount(2);
        result.Select(x => x.Tagname).Should().BeEquivalentTo(TestInstance, EmptyInstance);
    }

    [TestMethod]
    public void AsEnumerable_Attributes_ShouldReturnAllItems()
    {
        var instance = GetInstance(TestInstance);

        var result = instance.Attributes.AsEnumerable().ToList();

        result.Should().NotBeEmpty();
        result.Count.Should().Be(instance.Attributes.count);
        result.Select(x => x.Name).Should().Contain(["TestInt", "TestString", "TestArray"]);
    }

    [TestMethod]
    public void AsEnumerable_Attributes_ShouldBeEnumerableMultipleTimes()
    {
        var instance = GetInstance(TestInstance);
        var attributes = instance.Attributes.AsEnumerable();

        attributes.Count().Should().Be(attributes.Count());
    }
}