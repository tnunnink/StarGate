namespace StarGate.Core.Wizards;

/// <summary>
/// Represents the types of associations that a wizard can be linked to.
/// </summary>
public enum WizardAssociation
{
    /// <summary>
    /// Represents an association of a wizard with an attribute,
    /// typically used to define or manipulate metadata or characteristics
    /// tied to a specific element or object in the system.
    /// </summary>
    Attribute,

    /// <summary>
    /// Represents an association of a wizard with a property,
    /// typically used to manage or configure specific attributes
    /// or behaviors of an object within the system.
    /// </summary>
    Property,

    /// <summary>
    /// Represents an association of a wizard with a script, typically used
    /// to define or manage executable functionalities or logic tied to a specific context.
    /// </summary>
    Script,

    /// <summary>
    /// Represents an association of a wizard with a symbol,
    /// often used to denote or reference a specific identifier
    /// or construct within the system.
    /// </summary>
    Symbol,

    /// <summary>
    /// Represents an association of a wizard with an element,
    /// often used to define or interact with a fundamental piece
    /// or component within the system.
    /// </summary>
    Element,
}