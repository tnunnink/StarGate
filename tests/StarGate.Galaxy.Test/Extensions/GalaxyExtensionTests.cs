using System;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading;
using ArchestrA.Configuration;
using ArchestrA.GRAccess;
using FluentAssertions;
using NUnit.Framework;
using StarGate.Core;
using Namespace = ArchestrA.Core.Namespace;

namespace StarGate.Galaxy.Tests;

[TestFixture]
[SingleThreaded]
public class GalaxyExtensionTests : GalaxyTestFixture
{
    [Test]
    public void Diagnose_SelfContainedSession()
    {
        var access = new GRAccessAppClass();
        var galaxy = access.QueryGalaxies(Environment.MachineName)["TestGalaxy"];
        galaxy.Login(string.Empty, string.Empty);

        try
        {
            Console.WriteLine($"Domain: {AppDomain.CurrentDomain.FriendlyName} (id {AppDomain.CurrentDomain.Id}), " +
                              $"default: {AppDomain.CurrentDomain.IsDefaultAppDomain()}");
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}, " +
                              $"apartment {Thread.CurrentThread.GetApartmentState()}");

            var configuration = (IGalaxyConfiguration)galaxy.GetGalaxyConfiguration();
            Console.WriteLine($"configuration is proxy: {RemotingServices.IsTransparentProxy(configuration)}");

            var tree = configuration.GetFolderTree(ArchestrA.Core.FolderType.Visual, out var treeStatus, out var treeMessage);
            Console.WriteLine($"GetFolderTree status={treeStatus} message={treeMessage}");
            Console.WriteLine($"folder tree is proxy: {RemotingServices.IsTransparentProxy(tree)}");

            try
            {
                var folderId = tree.GetFolderId("Test", out var status, out var message);
                Console.WriteLine($"folderId={folderId} status={status} message={message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"GetFolderId threw {e.GetType().Name}: {e.Message}");
            }
        }
        finally
        {
            galaxy.Logout();
        }
    }
    
    [Test]
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

    [Test]
    public void TestingGalaxyConfiguration()
    {
        var result = Galaxy.GetGalaxyConfiguration();

        if (result is IGalaxyConfiguration configuration)
        {
            var info = configuration.GetGObjectInfoFromName("TestGraphic", Namespace.VisualElement);
            
            var folderTree = configuration.GetFolderTree(ArchestrA.Core.FolderType.Visual, out _, out _);
            var folderId = folderTree.GetFolderId("Test", out var status, out var message);
            Console.WriteLine($"GetFolderId Status: {status}; {message}");

            configuration.MoveObjectsToFolder([info.Id], folderId, out var failed);
            Console.WriteLine("FUCK YEAH");
            return;
        }
        
        Console.WriteLine("You stupid FUCK!!!");
    }
    
    [Test]
    public void CheckAppDomain()
    {
        Console.WriteLine($"Current AppDomain: {AppDomain.CurrentDomain.FriendlyName}");
        Console.WriteLine($"IsDefaultAppDomain: {AppDomain.CurrentDomain.IsDefaultAppDomain()}");
    }

    [Test]
    public void GetToolSets()
    {
        var toolsets = Galaxy.QueryToolsetsEx(FolderType.Visual);

        foreach (IToolset toolset in toolsets)
        {
            Console.WriteLine(toolset.Name);
        }
    }

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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

    [Test]
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