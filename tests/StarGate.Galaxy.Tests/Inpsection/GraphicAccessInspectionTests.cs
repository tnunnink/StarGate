using System.Reflection;
using ArchestrA.Visualization.GraphicAccess;

namespace StarGate.Galaxy.Tests;

[TestClass]
public class GraphicAccessInspectionTests : GalaxyFixture
{
    [ClassInitialize]
    public static void Init(TestContext _)
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

    [TestMethod]
    [DataRow("TestArc")]
    [DataRow("TestButton")]
    [DataRow("TestCheckBox")]
    [DataRow("TestChord")]
    [DataRow("TestClosedCurve")]
    [DataRow("TestConnector")]
    [DataRow("TestCurve")]
    [DataRow("TestEditBox")]
    [DataRow("TestEllipse")]
    [DataRow("TestGraphic")]
    [DataRow("TestGroup")]
    [DataRow("TestImage")]
    [DataRow("TestLine")]
    [DataRow("TestLineHV")]
    [DataRow("TestPath")]
    [DataRow("TestPie")]
    [DataRow("TestPolygon")]
    [DataRow("TestPolyline")]
    [DataRow("TestRectangle")]
    [DataRow("TestStatus")]
    [DataRow("TestText")]
    public void ExportGraphics(string name)
    {
        var access = new GraphicAccess();
        access.ExportGraphicToXml(Galaxy, name, $"Graphics/{name}.xml");
        Console.WriteLine(access.CommandResult.CustomMessage);
    }

    [TestMethod]
    [DataRow("TestArc")]
    [DataRow("TestButton")]
    [DataRow("TestCalendar")]
    [DataRow("TestCheckBox")]
    [DataRow("TestChord")]
    [DataRow("TestClosedCurve")]
    [DataRow("TestConnector")]
    [DataRow("TestCurve")]
    [DataRow("TestEditBox")]
    [DataRow("TestEllipse")]
    [DataRow("TestGraphic")]
    [DataRow("TestGroup")]
    [DataRow("TestImage")]
    [DataRow("TestLine")]
    [DataRow("TestLineHV")]
    [DataRow("TestPath")]
    [DataRow("TestPie")]
    [DataRow("TestPolygon")]
    [DataRow("TestPolyline")]
    [DataRow("TestRectangle")]
    [DataRow("TestStatus")]
    [DataRow("TestText")]
    public void ImportGraphics(string name)
    {
        var access = new GraphicAccess();
        access.ImportGraphicFromXml(Galaxy, $"Test\\{name}", $"Graphics/{name}.xml", true);
        Console.WriteLine(access.CommandResult.CustomMessage);
    }
}