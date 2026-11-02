using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StarGate.Galaxy;

/// <summary>
/// Provides a mechanism to resolve assembly dependencies during runtime.
/// This class ensures that specific assemblies are located and loaded from predefined directories
/// to handle version conflicts and resolve references that may not be automatically located
/// by the .NET runtime.
/// </summary>
internal static class GalaxyAssemblyResolver
{
    private static int _initialized;

    [ModuleInitializer]
    internal static void Initialize()
    {
        // Ensure thread-safe single execution
        if (System.Threading.Interlocked.Exchange(ref _initialized, 1) != 0)
            return;

        //todo eventually we probably want this in a config file or environment variable - something we can read from.
        const string binFolder = @"C:\Program Files (x86)\ArchestrA\Framework\Bin";
        const string editorsFolder = @"C:\Program Files (x86)\ArchestrA\SEditorsCommon";
        const string commonFolder = @"C:\Program Files (x86)\Common Files\ArchestrA";
        const string modernEditors = @"C:\Program Files (x86)\ArchestrA\Framework\Bin\ModernEditors";
        const string viewAppFramework = @"C:\Program Files (x86)\ArchestrA\Framework\Bin\ViewAppFramework";

        // Preload any assemblies that cause version conflicts or resolution quirks
        var loggingDll = Path.Combine(modernEditors, "Microsoft.Extensions.Logging.Abstractions.dll");
        if (File.Exists(loggingDll))
        {
            try
            {
                Assembly.LoadFrom(loggingDll);
            }
            catch
            {
                /* ignore */
            }
        }

        var folders = new[] { binFolder, editorsFolder, commonFolder, modernEditors, viewAppFramework };

        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
        {
            var requestedName = new AssemblyName(args.Name).Name;
            var assemblyFileName = requestedName + ".dll";

            foreach (var folder in folders)
            {
                var fullPath = Path.Combine(folder, assemblyFileName);
                if (File.Exists(fullPath))
                {
                    return Assembly.LoadFrom(fullPath);
                }
            }

            return null;
        };
    }
}