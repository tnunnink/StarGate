/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StarGate.Galaxy.Common;

public static class ArchestrAAssemblyResolver
{
    private static readonly HashSet<string> ProbeDirectories = new(StringComparer.OrdinalIgnoreCase);
    private static readonly object Lock = new();
    private static bool _isRegistered;

    /// <summary>
    /// Registers the assembly resolver with default and optional custom search directories.
    /// </summary>
    public static void Initialize(IEnumerable<string>? customDirectories = null)
    {
        lock (Lock)
        {
            if (customDirectories != null)
            {
                foreach (var dir in customDirectories.Where(Directory.Exists))
                    ProbeDirectories.Add(dir);
            }

            // Always add detected and default paths
            DiscoverInstalledDirectories();

            if (!_isRegistered)
            {
                AppDomain.CurrentDomain.AssemblyResolve += ResolveArchestrAAssembly;
                _isRegistered = true;
            }
        }
    }

    private static Assembly? ResolveArchestrAAssembly(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name).Name + ".dll";

        lock (Lock)
        {
            foreach (var directory in ProbeDirectories)
            {
                var fullPath = Path.Combine(directory, assemblyName);
                if (File.Exists(fullPath))
                {
                    return Assembly.LoadFrom(fullPath);
                }
            }
        }

        return null;
    }

    private static void DiscoverInstalledDirectories()
    {
        // 1. Check Registry
        AddFromRegistry(@"SOFTWARE\ArchestrA\Framework", "InstallPath");
        AddFromRegistry(@"SOFTWARE\ArchestrA\SEditorsCommon", "InstallPath");

        // 2. Standard Program Files (x86) Fallbacks
        var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        var commonFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86);

        AddDirectory(Path.Combine(programFilesX86, @"ArchestrA\Framework\Bin"));
        AddDirectory(Path.Combine(programFilesX86, @"ArchestrA\SEditorsCommon"));
        AddDirectory(Path.Combine(commonFilesX86, @"ArchestrA"));
    }

    private static void AddFromRegistry(string subKey, string valueName)
    {
        try
        {
            using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            using var key = baseKey.OpenSubKey(subKey);
            var path = key?.GetValue(valueName) as string;
            if (!string.IsNullOrWhiteSpace(path))
            {
                AddDirectory(path);
            }
        }
        catch
        {
            // Suppress registry access permissions errors
        }
    }

    private static void AddDirectory(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            ProbeDirectories.Add(path);
        }
    }
}*/

