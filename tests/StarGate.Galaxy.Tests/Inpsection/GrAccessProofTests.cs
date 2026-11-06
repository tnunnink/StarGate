using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using ArchestrA.Configuration;
using ArchestrA.Core;
using ArchestrA.GRAccess;
using FluentAssertions;
using FolderType = ArchestrA.Core.FolderType;
using MxAttributeCategory = ArchestrA.GRAccess.MxAttributeCategory;
using MxDataType = ArchestrA.GRAccess.MxDataType;
using MxValueClass = ArchestrA.GRAccess.MxValueClass;
using Namespace = ArchestrA.Core.Namespace;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class GrAccessProofTests : GalaxyFixture
{
    [TestMethod]
    public void GettingAnAttributeThatDoesNotExistDoesWhat()
    {
        var results = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);

        var obj = results[TestInstance];

        var fake = obj.Attributes["FakeAttribute"];

        fake.Should().BeNull();
    }

    [TestMethod]
    public void PassingNullToTheAttributeIndexerDoesWhat()
    {
        var results = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);

        var obj = results[TestInstance];

        FluentActions.Invoking(() => obj.Attributes[null]).Should().Throw<COMException>();
    }

    [TestMethod]
    public void ValidateWhichAttributeCategoriesActuallyWorkToAddUda()
    {
        var results = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);
        var obj = results[TestInstance];

        obj.CheckOut();

        var names = Enum.GetNames(typeof(MxAttributeCategory));

        foreach (var name in names)
        {
            var category = (MxAttributeCategory)Enum.Parse(typeof(MxAttributeCategory), name);
            obj.AddUDA(
                "Temp",
                MxDataType.MxInteger,
                category
            );
            Console.WriteLine($"Category: {category}; Result: {obj.CommandResult.ID}");

            if (!obj.CommandResult.Successful) continue;
            obj.DeleteUDA("Temp");
        }

        obj.UndoCheckOut();
    }

    [TestMethod]
    public void PrintConfigurableExtensionAttributes()
    {
        var results = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, [TestInstance]);
        var obj = results[TestInstance];

        foreach (var attr in obj.Attributes.AsEnumerable().Where(a => a.IsConfigurable()))
        {
            // Filter out engine metadata
            if (attr.Name.StartsWith("_") || attr.Name.Contains("._"))
                continue;

            Console.WriteLine(
                $"{attr.Name,-45} | Category: {(int)attr.AttributeCategory} ({attr.AttributeCategory}) | Type: {attr.DataType}");
        }
    }

    [TestMethod]
    [DataRow("alarmextension", "TempBool")]
    [DataRow("badvaluealarmextension", "TempFloat")]
    [DataRow("historyextension", "TempFloat")]
    [DataRow("inputextension", "TempFloat")]
    [DataRow("inputoutputextension", "TempFloat")]
    [DataRow("logdatachangeeventextension", "TempFloat")]
    [DataRow("outputextension", "TempFloat")]
    public void EnableBasicExtensionToExtractConfigurableAttributeList(string extension, string attribute)
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        var baseline = new HashSet<string>(obj.Attributes
            .AsEnumerable()
            .Where(a => a.IsConfigurable())
            .Select(a => a.Name)
        );

        obj.CheckOut();
        obj.AddExtensionPrimitive(extension, attribute, false);
        Console.WriteLine($"Extension Add Result: {obj.CommandResult.Text}");
        DumpAttributeDifferences(obj, baseline);

        //Clean up the edit, save, and check back in.
        obj.DeleteExtensionPrimitive(extension, "Temp");
        Console.WriteLine($"Extension Delete Result: {obj.CommandResult.Text}");
        obj.Save();
        Console.WriteLine($"Save Result: {obj.CommandResult.Text}");
        obj.CheckIn("Testing attribute extensions");
        Console.WriteLine($"Check in: {obj.CommandResult.Text}");
    }

    [TestMethod]
    [DataRow("analogextension", "LevelAlarmed")]
    [DataRow("analogextension", "ROCAlarmed")]
    [DataRow("analogextension", "DeviationAlarmed")]
    [DataRow("analogextension", "HasStatistics")]
    public void EnableAnalogExtensionToExtractConfigurableAttributeList(string extension, string attribute)
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        var baseline = new HashSet<string>(obj.Attributes
            .AsEnumerable()
            .Where(a => a.IsConfigurable())
            .Select(a => a.Name)
        );

        //Add the extension
        obj.CheckOut();
        obj.AddExtensionPrimitive(extension, "TempFloat", false);
        Console.WriteLine($"Extension Add Result: {obj.CommandResult.ID}");
        baseline = DumpAttributeDifferences(obj, baseline);

        //Configure the extension property to add the corresponding extension attributes
        var value = new MxValueClass();
        value.PutBoolean(true);
        obj.Attributes[$"TempFloat.{attribute}"]?.SetValue(value);
        Console.WriteLine($"Configure Extension Result: {obj.CommandResult.ID}");
        DumpAttributeDifferences(obj, baseline);

        //Clean up and check back in
        obj.DeleteExtensionPrimitive(extension, "TempFloat");
        obj.Save();
        Console.WriteLine($"Save Result: {obj.CommandResult.ID}");
        obj.CheckIn("Testing attribute extensions");
        Console.WriteLine($"Check in: {obj.CommandResult.ID}");
    }

    [TestMethod]
    [DataRow("booleanextension", "HasStatistics")]
    public void EnableBooleanExtensionToExtractConfigurableAttributeList(string extension, string attribute)
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        var baseline = new HashSet<string>(obj.Attributes
            .AsEnumerable()
            .Where(a => a.IsConfigurable())
            .Select(a => a.Name)
        );

        //Add the extension
        obj.CheckOut();
        obj.AddExtensionPrimitive(extension, "TempBool", false);
        Console.WriteLine($"Extension Add Result: {obj.CommandResult.ID}");
        baseline = DumpAttributeDifferences(obj, baseline);

        //Configure the extension property to add the corresponding extension attributes
        var value = new MxValueClass();
        value.PutBoolean(true);
        obj.Attributes[$"TempBool.{attribute}"]?.SetValue(value);
        Console.WriteLine($"Configure Extension Result: {obj.CommandResult.ID}");
        DumpAttributeDifferences(obj, baseline);

        //Clean up and check back in
        obj.DeleteExtensionPrimitive(extension, "TempBool");
        obj.Save();
        Console.WriteLine($"Save Result: {obj.CommandResult.ID}");
        obj.CheckIn("Testing attribute extensions");
        Console.WriteLine($"Check in: {obj.CommandResult.ID}");
    }

    [TestMethod]
    public void WhatHappensIfWeTryToAddAnExtensionToAttributeThatAlreadyHasItConfigured()
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        //Add the extension
        obj.CheckOut();
        obj.AddExtensionPrimitive("alarmextension", "TempBool", false);
        Console.WriteLine($"Extension Add Result: {obj.CommandResult.ID}");

        obj.Save();
        Console.WriteLine($"Save Result: {obj.CommandResult.ID}");
        obj.CheckIn("Testing attribute extensions");
        Console.WriteLine($"Check in: {obj.CommandResult.ID}");

        //Add the extension
        obj.CheckOut();
        obj.AddExtensionPrimitive("alarmextension", "TempBool", false);
        obj.CommandResult.Successful.Should().BeFalse();

        //Clean up and check back in
        obj.DeleteExtensionPrimitive("alarmextension", "TempBool");
        obj.Save();
        Console.WriteLine($"Save Result: {obj.CommandResult.ID}");
        obj.CheckIn("Testing attribute extensions");
        Console.WriteLine($"Check in: {obj.CommandResult.ID}");
    }

    [TestMethod]
    public void WhatHappensIfWeTryToDeleteAnExtensionThatDoesNotExist()
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        //Add the extension
        obj.CheckOut();
        obj.DeleteExtensionPrimitive("alarmextension", "TempBool");
        Console.WriteLine($"Save Result: {obj.CommandResult.ID}");
        obj.CommandResult.Successful.Should().BeFalse();
        obj.UndoCheckOut();
    }

    [TestMethod]
    public void WhatHappensIfWeTryToUpdateAnIOExtension()
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [EmptyInstance]
        )[EmptyInstance];

        //Add the extension
        obj.CheckOut();
        obj.AddExtensionPrimitive("inputoutputextension", "TempBool");
        Console.WriteLine($"Add extension result: {obj.CommandResult.ID}");

        //Configure the input source
        var value = new MxValueClass();
        value.PutString("Me.TempBool");
        obj.Attributes["TempBool.InputSource"].SetValue(value);
        Console.WriteLine($"Set input source result: {obj.CommandResult.ID}");

        //Add inputextension to change to readonly (this will probably fail)
        obj.AddExtensionPrimitive("inputextension", "TempBool");
        Console.WriteLine($"Update extension result: {obj.CommandResult.ID}");
        obj.CommandResult.Successful.Should().BeFalse();

        obj.UndoCheckOut();
    }

    [TestMethod]
    public void DeleteAndRecreateUda_WithScriptReference_DoesItSucceedOnSave()
    {
        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [TestInstance]
        )[TestInstance];

        obj.CheckOut();

        // 1. Ensure a UDA exists
        obj.AddUDA("TestUdaForRecreate", MxDataType.MxInteger, MxAttributeCategory.MxCategoryWriteable_USC_Lockable);

        // 2. Add a script that references this UDA
        obj.AddExtensionPrimitive("ScriptExtension", "TestScriptRef", false);
        var scriptValue = new MxValueClass();
        scriptValue.PutString("Me.TestUdaForRecreate = 10;");
        obj.Attributes["TestScriptRef.ExecuteText"]?.SetValue(scriptValue);

        obj.Save();
        Console.WriteLine($"Baseline Setup Result: {obj.CommandResult.ID} - {obj.CommandResult.Text}");

        // 3. NOW TEST THE THEORY: Delete the UDA and immediately re-add it in the same checkout session
        obj.DeleteUDA("TestUdaForRecreate");
        Console.WriteLine($"Delete Result: {obj.CommandResult.ID} - {obj.CommandResult.Text}");

        obj.AddUDA("TestUdaForRecreate", MxDataType.MxInteger, MxAttributeCategory.MxCategoryWriteable_USC_Lockable);
        Console.WriteLine($"Re-add Result: {obj.CommandResult.ID} - {obj.CommandResult.Text}");

        // 4. Call Save() - does ArchestrA accept it, or complain about the script reference / ID mismatch?
        obj.Save();
        Console.WriteLine($"Save After Re-add Result: {obj.CommandResult.ID} - {obj.CommandResult.Text}");

        var saveSuccessful = obj.CommandResult.Successful;

        // Clean up
        obj.DeleteExtensionPrimitive("ScriptExtension", "TestScriptRef");
        obj.DeleteUDA("TestUdaForRecreate");
        obj.Save();
        obj.CheckIn("Cleaned up recreate test");

        saveSuccessful.Should()
            .BeTrue("ArchestrA should allow deleting and re-adding a referenced UDA in the same session");
    }

    [TestMethod]
    public void Benchmark_UdaRecreate_Vs_InPlaceUpdate_Performance()
    {
        const int attributeCount = 20;
        var attributes = Enumerable.Range(1, attributeCount)
            .Select(i => $"PerfUda_{i:D2}")
            .ToArray();

        var obj = Galaxy.QueryObjectsByName(
            EgObjectIsTemplateOrInstance.gObjectIsInstance,
            [TestInstance]
        )[TestInstance];

        // ==========================================
        // 1. Initial Setup: Create the 20 UDAs
        // ==========================================
        obj.CheckOut();
        foreach (var name in attributes)
            obj.AddUDA(name, MxDataType.MxInteger, MxAttributeCategory.MxCategoryWriteable_USC_Lockable);

        obj.Save();
        obj.CheckIn("Baseline setup of performance UDAs");

        Console.WriteLine($"--- Starting Performance Benchmark ({attributeCount} UDAs) ---");

        // ==========================================
        // 2. Measure Strategy A: Delete & Re-add All
        // ==========================================
        var swRecreate = Stopwatch.StartNew();

        obj.CheckOut();
        var swRecreateOperations = Stopwatch.StartNew();

        foreach (var name in attributes)
        {
            obj.DeleteUDA(name);
            obj.AddUDA(name, MxDataType.MxInteger, MxAttributeCategory.MxCategoryWriteable_USC_Lockable);

            var val = new MxValueClass();
            val.PutInteger(100);
            obj.Attributes[name]?.SetValue(val);
        }

        swRecreateOperations.Stop();

        var swRecreateSave = Stopwatch.StartNew();
        obj.Save();
        swRecreateSave.Stop();

        obj.CheckIn("Save after recreate");
        swRecreate.Stop();

        Console.WriteLine("[Strategy A: Recreate]");
        Console.WriteLine($"  Delete & Add & Set loop : {swRecreateOperations.ElapsedMilliseconds} ms");
        Console.WriteLine($"  Save()                  : {swRecreateSave.ElapsedMilliseconds} ms");
        Console.WriteLine($"  Total Checkout-to-Checkin: {swRecreate.ElapsedMilliseconds} ms");

        // ==========================================
        // 3. Measure Strategy B: In-Place Update Only
        // ==========================================
        var swInPlace = Stopwatch.StartNew();

        obj.CheckOut();
        var swInPlaceOperations = Stopwatch.StartNew();

        foreach (var name in attributes)
        {
            var val = new MxValueClass();
            val.PutInteger(200);
            obj.Attributes[name]?.SetValue(val);
        }

        swInPlaceOperations.Stop();

        var swInPlaceSave = Stopwatch.StartNew();
        obj.Save();
        swInPlaceSave.Stop();

        obj.CheckIn("Save after in-place update");
        swInPlace.Stop();

        Console.WriteLine("[Strategy B: In-Place Update]");
        Console.WriteLine($"  SetValue loop           : {swInPlaceOperations.ElapsedMilliseconds} ms");
        Console.WriteLine($"  Save()                  : {swInPlaceSave.ElapsedMilliseconds} ms");
        Console.WriteLine($"  Total Checkout-to-Checkin: {swInPlace.ElapsedMilliseconds} ms");

        // ==========================================
        // Cleanup: Remove benchmark UDAs
        // ==========================================
        obj.CheckOut();
        foreach (var name in attributes) obj.DeleteUDA(name);

        obj.Save();
        obj.CheckIn("Cleaned up performance test UDAs");

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine(
            $"Recreate was {swRecreate.ElapsedMilliseconds / (double)Math.Max(1, swInPlace.ElapsedMilliseconds):F2}x the time of in-place update.");
    }

    //GRAccess does not give us a way to modify wizard options, so we have to go through the back door.
    [TestMethod]
    public void GetWizardOptionDefinitionFromTemplateToInspect()
    {
        var queried = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ["$TestTemplate"]);
        var obj = queried["$TestTemplate"];

        var definition = obj.Attributes["_WizardDefinition_1"];

        var dataType = definition.value.GetDataType();
        var content = definition.value.GetString();
        content.Should().NotBeEmpty();
        Console.WriteLine(dataType);
        Console.WriteLine(content);
    }

    //GRAccess does not give us a way to modify wizard options, so we have to go through the back door.
    [TestMethod]
    public void GetWizardOptionDefinitionFromInstanceToInspect()
    {
        var queried = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ["TestInstance"]);
        var obj = queried["TestInstance"];

        var definition = obj.Attributes["_WizardDefinition_2"];

        var dataType = definition.value.GetDataType();
        var content = definition.value.GetString();
        content.Should().NotBeEmpty();
        Console.WriteLine(dataType);
        Console.WriteLine(content);
    }

    //GRAccess does not give us a way to modify wizard options, so we have to go through the back door.
    [TestMethod]
    public void CanWeConfigureWizardOptionInternalAttributeAndHaveItCompiled()
    {
        const string xml = """
                           <WizardState Version="2" 
                                        xmlns="clr-namespace:ArchestrA.EditorFramework.ViewModel.Wizard.Serialization;assembly=AttributesTab" 
                                        xmlns:aaefvmw="clr-namespace:ArchestrA.EditorFramework.ViewModel.Wizard;assembly=AttributesTab" 
                                        xmlns:aaefvmwc="clr-namespace:ArchestrA.EditorFramework.ViewModel.Wizard.Collections;assembly=AttributesTab">
                               <WizardState.Definition>
                                   <aaefvmwc:ChoiceGroupCollection>
                                       <aaefvmw:ChoiceGroup CheckedChoiceName="Choice2" Name="bd2b4816-4801-40cf-91b4-348edef1ae96" Prompt="ChoiceGroup" Comment="this is an updated test">
                                           <aaefvmw:ChoiceGroup.Choices>
                                               <aaefvmwc:ChoiceCollection>
                                                   <aaefvmw:Choice Name="Choice1" Prompt="Choice #1">
                                                       <aaefvmw:Choice.Associations>
                                                           <aaefvmwc:AssociationCollection />
                                                       </aaefvmw:Choice.Associations>
                                                   </aaefvmw:Choice>
                                                   <aaefvmw:Choice Name="Choice2" Prompt="Choice #2">
                                                       <aaefvmw:Choice.Associations>
                                                           <aaefvmw:AttributeAssociation Name="ParentBool">
                                                               <aaefvmw:AttributeAssociation.Settings>
                                                                   <aaefvmwc:SettingCollection />
                                                               </aaefvmw:AttributeAssociation.Settings>
                                                           </aaefvmw:AttributeAssociation>
                                                       </aaefvmw:Choice.Associations>
                                                   </aaefvmw:Choice>
                                               </aaefvmwc:ChoiceCollection>
                                           </aaefvmw:ChoiceGroup.Choices>
                                       </aaefvmw:ChoiceGroup>
                                       <aaefvmw:ChoiceGroup CheckedChoiceName="True" IsOption="True" Name="c526b8b1-9296-46b4-ac97-9a4123251dac" Prompt="Option" Comment="this is a test" IsVisibleExpression="ChoiceGroup::Choice #2">
                                           <aaefvmw:ChoiceGroup.Choices>
                                               <aaefvmwc:ChoiceCollection>
                                                   <aaefvmw:Choice Name="True" Prompt="True">
                                                       <aaefvmw:Choice.Associations>
                                                           <aaefvmw:AttributeAssociation Name="ParentInt">
                                                               <aaefvmw:AttributeAssociation.Settings>
                                                                   <aaefvmwc:SettingCollection />
                                                               </aaefvmw:AttributeAssociation.Settings>
                                                           </aaefvmw:AttributeAssociation>
                                                       </aaefvmw:Choice.Associations>
                                                   </aaefvmw:Choice>
                                                   <aaefvmw:Choice Name="False" Prompt="False">
                                                       <aaefvmw:Choice.Associations>
                                                           <aaefvmwc:AssociationCollection />
                                                       </aaefvmw:Choice.Associations>
                                                   </aaefvmw:Choice>
                                               </aaefvmwc:ChoiceCollection>
                                           </aaefvmw:ChoiceGroup.Choices>
                                       </aaefvmw:ChoiceGroup>
                                   </aaefvmwc:ChoiceGroupCollection>
                               </WizardState.Definition>
                           </WizardState>
                           """;
        var queried = Galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ["$TestTemplate"]);
        var obj = queried["$TestTemplate"];
        obj.CheckOut();

        var definition = obj.Attributes["_WizardDefinition_1"];
        var value = new MxValueClass();
        value.PutString(xml);
        definition.SetValue(value);
        Console.WriteLine(definition.CommandResult.Text);

        obj.Save();
        Console.WriteLine(definition.CommandResult.Text);
        obj.CheckIn("This is an update test for the wizard option definition XML");
        Console.WriteLine(definition.CommandResult.Text);
    }

    [TestMethod]
    public void FolderTree_GetFolderId_ShouldNotThrowRemotingException()
    {
        var configuration = (IGalaxyConfiguration)Galaxy.GetGalaxyConfiguration();

        var tree = configuration.GetFolderTree(FolderType.Visual, out var treeStatus, out _);
        Console.WriteLine($"GetFolderTree: {treeStatus}; proxy: {RemotingServices.IsTransparentProxy(tree)}");

        var folderId = tree.GetFolderId("Test", out var status, out var message);
        Console.WriteLine($"GetFolderId: id={folderId} status={status} message={message}");

        folderId.Should().BeGreaterThan(0);
    }

    [TestMethod]
    public void InspectGObjectInfoPropertiesToSeeWhatWeCanHarvest()
    {
        // this is a much cleaner API since it lets you specify namespace. I wonder if it will be performant for many tag names.
        var info = Galaxy.Config.GetGObjectInfoFromName(TestInstance, Namespace.AutomationObject);

        info.Should().NotBeNull();

        foreach (var propertyInfo in typeof(IGObjectInfo).GetProperties())
        {
            Console.WriteLine($"{propertyInfo.Name} [{propertyInfo.PropertyType}] = {propertyInfo.GetValue(info)}");
        }
    }

    [TestMethod]
    public void GetFolderPathForGivenGraphicName()
    {
        var info = Galaxy.Config.GetGObjectInfoFromName("SA_Meters", Namespace.VisualElement);
        var tree = Galaxy.Config.GetFolderTree(FolderType.Visual, out _, out _);
        var path = tree.GetPath(info.FolderId);

        path.Should().NotBeEmpty();
        path.Replace("$", "/").Should().Be("Situational Awareness Library/Meters");
        Console.WriteLine(path);
    }

    private static HashSet<string> DumpAttributeDifferences(IgObject obj, HashSet<string> baseline)
    {
        //Get updated attribute list
        var updated = new HashSet<string>(obj.Attributes
            .AsEnumerable()
            .Where(a => a.IsConfigurable())
            .Select(a => a.Name)
        );

        // Then compute the differences to see which attributes get added (that are "configurable").
        var differences = updated.Except(baseline);
        differences.ToList().ForEach(Console.WriteLine);

        return updated;
    }
}