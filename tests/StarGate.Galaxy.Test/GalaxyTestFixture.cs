using System;
using ArchestrA.GRAccess;

// ReSharper disable SuspiciousTypeConversion.Global

namespace StarGate.Galaxy.Test;

/// <summary>
/// Represents a base test fixture for test classes that interact with a Galaxy instance.
/// Provides centralized access to the Galaxy object and supports common initialization
/// requirements for derived test classes.
/// </summary>
/// <remarks>
/// This abstract test fixture depends on the <see cref="GalaxyTestSetup" /> to ensure
/// that the Galaxy environment is properly initialized and available during test execution.
/// The <see cref="Galaxy" /> property exposes the Galaxy instance for use in test scenarios.
/// </remarks>
/// <example>
/// Intended to be inherited by test classes that require direct interaction with Galaxy components.
/// Provides a shared foundation for Galaxy-dependent test logic and reusable test setup procedures.
/// </example>
public abstract class GalaxyTestFixture
{
    /// <summary>
    /// Represents the Galaxy instance that serves as the central element for interacting with the ArchestrA environment in
    /// test scenarios.
    /// </summary>
    /// <remarks>
    /// This property provides access to the shared Galaxy object, which is initialized and managed by the
    /// <see cref="GalaxyTestSetup" /> class.
    /// It supports operations that require interaction with the underlying ArchestrA system.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the Galaxy environment is not properly initialized before attempting to access this property.
    /// Ensure that the <see cref="GalaxyTestSetup.GlobalSetup" /> method has been executed before usage.
    /// </exception>
    protected static IGalaxy Galaxy =>
        GalaxyTestSetup.Galaxy ?? throw new InvalidOperationException("Galaxy is not initialized.");

    protected static string EmptyInstance => "EmptyInstance";
    protected static string TestTemplate => "$TestTemplate";
    protected static string TestInstance => "TestInstance";

    /// <summary>
    /// Retrieves a template object from the Galaxy environment based on the specified tag name.
    /// </summary>
    /// <param name="tagname">The name of the template object to retrieve.</param>
    /// <returns>An instance of <see cref="ITemplate" /> corresponding to the specified tag name.</returns>
    protected IgObject GetTemplate(string tagname)
    {
        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsTemplate;
        var results = Galaxy.QueryObjectsByName(option, [tagname]);
        return results[tagname];
    }

    /// <summary>
    /// Retrieves an instance object from the Galaxy environment based on the specified tag name.
    /// </summary>
    /// <param name="tagname">The name of the instance object to retrieve.</param>
    /// <returns>An instance of <see cref="IInstance" /> corresponding to the specified tag name.</returns>
    protected static IgObject GetInstance(string tagname)
    {
        const EgObjectIsTemplateOrInstance option = EgObjectIsTemplateOrInstance.gObjectIsInstance;
        var results = Galaxy.QueryObjectsByName(option, [tagname]);
        return results[tagname];
    }
}