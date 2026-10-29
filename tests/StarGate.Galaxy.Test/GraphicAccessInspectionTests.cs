using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
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
        if (File.Exists(loggingDll))
        {
            Assembly.LoadFrom(loggingDll);
        }

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
            {
                Console.WriteLine($"[AssemblyResolve FAILED] Could not find dependency: '{args.Name}'");
            }

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

    [Test]
    public void GetAllEnumerationValuesFromGraphicSchemaDefinitionFile()
    {
        const string filePath = @"C:\Program Files (x86)\ArchestrA\Framework\Bin\aaGraphics.xsd";

        if (!File.Exists(filePath))
            Assert.Fail("Schema definition file not found");

        var doc = XDocument.Load(filePath);
        XNamespace xs = "http://www.w3.org/2001/XMLSchema";

        var enums = doc.Descendants(xs + "simpleType")
            .Where(st => st.Descendants(xs + "enumeration").Any())
            .Select(st =>
            {
                // Infer name from the simpleType itself, or parent attribute / element if anonymous
                var name = st.Attribute("name")?.Value
                           ?? st.Parent?.Attribute("name")?.Value
                           ?? "UnknownEnum";

                var rawValues = st.Descendants(xs + "enumeration")
                    .Select(e => e.Attribute("value")?.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Select(v => v!)
                    .ToList();

                return new { RawName = name, RawValues = rawValues };
            })
            // Filter out types that just wrap primitives
            .Where(e =>
            {
                // Skip booleans (e.g., true/false variations)
                var isBoolean = e.RawValues.All(v => bool.TryParse(v, out _));
                if (isBoolean || string.Equals(e.RawName, "boolean", StringComparison.OrdinalIgnoreCase))
                    return false;

                // Skip purely numeric sets (e.g., TimePeriod: 1, 5, 10, 15...)
                var isNumeric = e.RawValues.All(v => double.TryParse(v, out _));
                if (isNumeric)
                    return false;

                return true;
            })
            .Select(e =>
            {
                // Normalize case duplicates (e.g. Slow/slow/SLOW -> Slow) and sanitize identifier names
                var sanitizedValues = new List<string>();
                var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var raw in e.RawValues)
                {
                    // Remove whitespace (e.g., "Windows Integrated" -> "WindowsIntegrated")
                    var clean = raw.Replace(" ", "").Trim();

                    if (clean.Length == 0 || !seen.Add(clean))
                        continue;

                    sanitizedValues.Add(clean);
                }

                return new { Name = e.RawName, Values = sanitizedValues };
            })
            .Where(e => e.Values.Count > 0)
            // Group and deduplicate enums with the same name across the XSD
            .GroupBy(e => e.Name)
            .Select(g => g.First())
            .OrderBy(e => e.Name);

        var builder = new StringBuilder();
        builder.AppendLine("// <auto-generated />");
        builder.AppendLine("namespace StarGate.Core.Model.Symbols.Enums;\n");

        foreach (var e in enums)
        {
            builder.AppendLine($"public enum {e.Name}");
            builder.AppendLine("{");
            foreach (var v in e.Values)
            {
                builder.AppendLine($"    {v},");
            }

            builder.AppendLine("}\n");
        }

        var outputPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Graphics\graphic_enums.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        File.WriteAllText(outputPath, builder.ToString());
    }
}