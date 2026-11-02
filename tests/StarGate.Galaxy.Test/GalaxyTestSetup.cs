using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using ArchestrA.GRAccess;
using NUnit.Framework;

// ReSharper disable SuspiciousTypeConversion.Global

// Disables parallel execution across ALL tests in this assembly
[assembly: NonParallelizable]

// Ensures all tests run in Single-Threaded Apartment (STA), which is critical for GRAccess COM
[assembly: Apartment(ApartmentState.STA)]

namespace StarGate.Galaxy.Tests;

[SetUpFixture]
public class GalaxyTestSetup
{
    /// <summary>
    /// Represents an instance of GRAccessAppClass used to interact with the Galaxy system.
    /// Provides methods for querying, logging in, and managing Galaxy environments
    /// within the context of automated tests. This variable is critical for establishing
    /// communication with the Galaxy server and managing related operations.
    /// </summary>
    private static GRAccessAppClass? _grAccess;

    /// <summary>
    /// Represents the identifier for the Galaxy instance being used for testing.
    /// This property provides the unique name of the Galaxy environment targeted
    /// by the test setup. The value is used to query and interact with the specified Galaxy
    /// within a testing context.
    /// </summary>
    private static string GalaxyIdentifier => "TestGalaxy";

    /// <summary>
    /// Represents the Galaxy instance being used in the test environment.
    /// This property provides access to the specific Galaxy database available for
    /// the test lifecycle. It is initialized during the setup phase of the testing
    /// process and is used to execute operations or queries against the Galaxy system.
    /// </summary>
    public static IGalaxy? Galaxy { get; private set; }

    /// <summary>
    /// Initializes and sets up the test environment for interacting with the Galaxy system.
    /// This method is executed once before any tests are run in the test suite.
    /// Establishes a connection to the Galaxy server, logs in, and optionally provisions baseline test objects.
    /// </summary>
    [OneTimeSetUp]
    public void GlobalSetup()
    {
        _grAccess = new GRAccessAppClass();

        Galaxy = _grAccess.QueryGalaxies(Environment.MachineName)[GalaxyIdentifier];
        Galaxy.Login(string.Empty, string.Empty);

        ImportResources(Galaxy);
    }

    /// <summary>
    /// Cleans up and tears down the test environment after all tests in the suite have completed execution.
    /// Logs out from the Galaxy system and releases any associated resources. Ensures that COM references
    /// are properly released to prevent resource leaks.
    /// </summary>
    [OneTimeTearDown]
    public void GlobalTearDown()
    {
        try
        {
            Galaxy?.Logout();
        }
        finally
        {
            if (_grAccess != null)
                // Release COM references cleanly
                Marshal.FinalReleaseComObject(_grAccess);
        }
    }

    /// <summary>
    /// Provisions test objects required for the Galaxy test environment.
    /// Ensures that baseline objects are created or configured within the Galaxy system
    /// to facilitate consistent and repeatable testing scenarios.
    /// </summary>
    private static void ImportResources(IGalaxy galaxy)
    {
        var packagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "TestObjects.aaPKG");

        galaxy.ImportObjectsEx(packagePath,
            E_RESOLVE_VERSION_CONFLICT_ACTION.eResolveVersionConflictAction_OverwriteObjectInDatabase,
            E_RESOLVE_NAME_CONFLICT_ACTION.eResolveNameConflictAction_SkipObject,
            string.Empty
        );

        Console.WriteLine($"Test objects import result: {galaxy.CommandResult.Text}");
    }
}