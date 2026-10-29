using System;
using StarGate.Core.Objects;

namespace StarGate.Core.Attributes;

/// <summary>
/// Specifies metadata for a property to indicate its name and security level within the StarGate system.
/// This attribute is intended to annotate properties, associating them with a specific descriptor
/// and a default security classification.
/// </summary>
/// <remarks>
/// The <see cref="GalaxyAttributeAttribute"/> is primarily used in StarGate.Core to configure attribute
/// metadata such as identifier names and security levels for access control. This helps define operational
/// behaviors and security strategies for annotated properties.
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class GalaxyAttributeAttribute(string name, Security defaultSecurity = Security.Undefined) : Attribute
{
    /// <summary>
    /// Gets the name of the galaxy attribute. This name uniquely identifies the attribute
    /// within the context of the Galaxy system.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the default security classification level assigned to this galaxy attribute.
    /// This determines the authorization requirements for accessing and modifying the attribute.
    /// </summary>
    public Security DefaultSecurity { get; } = defaultSecurity;
}