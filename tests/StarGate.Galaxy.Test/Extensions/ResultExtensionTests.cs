using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using StarGate.Galaxy;

namespace StarGate.Galaxy.Tests;

[TestFixture]
public class ResultExtensionTests : GalaxyTestFixture
{
    [Test]
    public void ToResult_NullCommandResult_ShouldBeSuccess()
    {
        ICommandResult? result = null;

        var converted = result.ToResult();

        converted.Success.Should().BeTrue();
        converted.Errors.Should().BeEmpty();
    }

    [Test]
    public void ToResultOfT_NullCommandResult_ShouldBeSuccessWithValue()
    {
        ICommandResult? result = null;

        var converted = result.ToResult("value");

        converted.Success.Should().BeTrue();
        converted.Value.Should().Be("value");
    }

    [Test]
    public void ToResult_SuccessfulCommandResult_ShouldBeSuccess()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var result = instance.CommandResult;
        instance.Rollback();

        result.Successful.Should().BeTrue();
        result.ToResult().Success.Should().BeTrue();
    }

    [Test]
    public void ToResultOfT_SuccessfulCommandResult_ShouldBeSuccessWithValue()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        var result = instance.CommandResult;
        instance.Rollback();

        var converted = result.ToResult(instance);

        converted.Success.Should().BeTrue();
        converted.Value.Should().BeSameAs(instance);
    }

    [Test]
    public void ToResult_FailedCommandResult_ShouldContainErrorWithCodeAndText()
    {
        var instance = GetInstance(TestInstance);
        instance.Save(); // not checked out so this should fail
        var result = instance.CommandResult;

        var converted = result.ToResult();

        result.Successful.Should().BeFalse();
        converted.Success.Should().BeFalse();
        converted.Errors.Should().ContainSingle();
        converted.Errors.Should().Contain(e => e.Code == result.ID.ToCoreType() && e.Message == result.Text);
    }

    [Test]
    public void ToResultOfT_FailedCommandResult_ShouldContainErrorAndValue()
    {
        var instance = GetInstance(TestInstance);
        instance.Save(); // not checked out so this should fail
        var result = instance.CommandResult;

        var converted = result.ToResult(instance);

        converted.Success.Should().BeFalse();
        converted.Value.Should().BeSameAs(instance);
        converted.Errors.Should().ContainSingle();
        converted.Errors.Should().Contain(e => e.Code == result.ID.ToCoreType() && e.Message == result.Text);
    }

    [Test]
    public void ToResult_NullCommandResults_ShouldBeSuccess()
    {
        ICommandResults? results = null;

        var converted = results.ToResult();

        converted.Success.Should().BeTrue();
        converted.Errors.Should().BeEmpty();
    }

    [Test]
    public void ToResultOfT_NullCommandResults_ShouldBeSuccessWithValue()
    {
        ICommandResults? results = null;

        var converted = results.ToResult(123);

        converted.Success.Should().BeTrue();
        converted.Value.Should().Be(123);
    }

    [Test]
    public void ToResult_CompletelySuccessfulCommandResults_ShouldBeSuccess()
    {
        var objects = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);
        objects.CheckOut();
        var results = objects.CommandResults;
        objects.UndoCheckOut();

        results.CompletelySuccessful.Should().BeTrue();
        results.ToResult().Success.Should().BeTrue();
    }

    [Test]
    public void ToResultOfT_CompletelySuccessfulCommandResults_ShouldBeSuccessWithValue()
    {
        var objects = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);

        var converted = objects.CommandResults.ToResult(objects);

        converted.Success.Should().BeTrue();
        converted.Value.Should().BeSameAs(objects);
    }

    [Test]
    public void ToResult_ErrorCodeMapping_SuccessfulCommandShouldMapToSuccessCode()
    {
        var instance = GetInstance(TestInstance);
        instance.CheckOut();
        instance.CommandResult.ID.ToCoreType().Should().BeOneOf(ErrorCode.Success, ErrorCode.UnknownError);
        instance.UndoCheckOut();
    }
}