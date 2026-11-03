using ArchestrA.Configuration;
using ArchestrA.GRAccess;

namespace StarGate.TestConsole;

public class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var access = new GRAccessAppClass();
        var galaxy = access.QueryGalaxies(Environment.MachineName)["TestGalaxy"];
        galaxy.Login(string.Empty, string.Empty);

        var result = galaxy.GetGalaxyConfiguration();

        if (result is not IGalaxyConfiguration configuration)
            return;

        var info = configuration.GetGObjectInfoFromName("TestGraphic", ArchestrA.Core.Namespace.VisualElement);
        Console.WriteLine($"GetObjectInfo TagName: {info.TagName}; Id: {info.Id};");

        var folderTree = configuration.GetFolderTree(ArchestrA.Core.FolderType.Visual, out _, out _);
        var folderId = folderTree.GetFolderId("Test", out var status, out var message);
        Console.WriteLine($"GetFolderId Id: {folderId}; Status: {status}; {message}");

        configuration.MoveObjectsToFolder([info.Id], folderId, out var failed);
        failed?.ToList().ForEach(f => Console.WriteLine(f));
        Console.WriteLine($"{info.TagName} moved to folder with id {folderId}");
    }
}