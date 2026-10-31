using System;
using System.IO;
using System.Reflection;
using ArchestrA.Visualization.GraphicAccess;
using NUnit.Framework;

namespace StarGate.Galaxy.Test;

[TestFixture]
public class GraphicAccessInspectionTests : GalaxyTestFixture
{
    [OneTimeSetUp]
    public void Init()
    {
        const string binFolder = @"C:\Program Files (x86)\ArchestrA\Framework\Bin";
        const string editorsFolder = @"C:\Program Files (x86)\ArchestrA\SEditorsCommon";
        const string commonFolder = @"C:\Program Files (x86)\Common Files\ArchestrA";
        const string modernEditors = @"C:\Program Files (x86)\ArchestrA\Framework\Bin\ModernEditors";
        const string viewAppFramework = @"C:\Program Files (x86)\ArchestrA\Framework\Bin\ViewAppFramework";

        // Explicitly preload Microsoft.Extensions.Logging.Abstractions so it is already resident in the AppDomain
        var loggingDll = Path.Combine(modernEditors, "Microsoft.Extensions.Logging.Abstractions.dll");
        if (File.Exists(loggingDll)) Assembly.LoadFrom(loggingDll);

        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";
            var folders = new[] { binFolder, editorsFolder, commonFolder, modernEditors, viewAppFramework };

            foreach (var folder in folders)
            {
                var fullPath = Path.Combine(folder, assemblyName);
                if (!File.Exists(fullPath)) continue;
                Console.WriteLine($"[AssemblyResolve] Resolved: {args.Name} -> {fullPath}");
                return Assembly.LoadFrom(fullPath);
            }

            if (!args.Name.Contains(".resources") &&
                !args.Name.StartsWith("System.") &&
                !args.Name.StartsWith("Microsoft."))
                Console.WriteLine($"[AssemblyResolve FAILED] Could not find dependency: '{args.Name}'");

            return null;
        };
    }

    [Test]
    [TestCase("TestArc")]
    [TestCase("TestButton")]
    [TestCase("TestCheckBox")]
    [TestCase("TestChord")]
    [TestCase("TestClosedCurve")]
    [TestCase("TestConnector")]
    [TestCase("TestCurve")]
    [TestCase("TestEditBox")]
    [TestCase("TestEllipse")]
    [TestCase("TestGraphic")]
    [TestCase("TestGroup")]
    [TestCase("TestImage")]
    [TestCase("TestLine")]
    [TestCase("TestLineHV")]
    [TestCase("TestPath")]
    [TestCase("TestPie")]
    [TestCase("TestPolygon")]
    [TestCase("TestPolyline")]
    [TestCase("TestRectangle")]
    [TestCase("TestStatus")]
    [TestCase("TestText")]
    public void ExportGraphics(string name)
    {
        var access = new GraphicAccess();
        access.ExportGraphicToXml(Galaxy, name, $"Graphics/{name}.xml");
        Console.WriteLine(access.CommandResult.Text);
    }

    [Test]
    [TestCase("TestArc")]
    [TestCase("TestButton")]
    [TestCase("TestCheckBox")]
    [TestCase("TestChord")]
    [TestCase("TestClosedCurve")]
    [TestCase("TestConnector")]
    [TestCase("TestCurve")]
    [TestCase("TestEditBox")]
    [TestCase("TestEllipse")]
    [TestCase("TestGraphic")]
    [TestCase("TestGroup")]
    [TestCase("TestImage")]
    [TestCase("TestLine")]
    [TestCase("TestLineHV")]
    [TestCase("TestPath")]
    [TestCase("TestPie")]
    [TestCase("TestPolygon")]
    [TestCase("TestPolyline")]
    [TestCase("TestRectangle")]
    [TestCase("TestStatus")]
    [TestCase("TestText")]
    public void ImportGraphics(string name)
    {
        var access = new GraphicAccess();
        access.ImportGraphicFromXml(Galaxy, name, $"Graphics/{name}.xml", true);
        Console.WriteLine(access.CommandResult.Text);
    }
}