using System.ComponentModel.DataAnnotations;

namespace StarGate.Core;

/// <summary>
/// Represents a two-way data binding connecting a control value to a Galaxy tag or attribute reference.
/// </summary>
public class ValueBinding
{
    /// <summary>
    /// Gets the Galaxy tag or attribute reference to read and write.
    /// </summary>
    [Required]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Gets an optional boolean expression that triggers submitting the value.
    /// When specified, value changes are deferred until this trigger fires.
    /// </summary>
    public string? Trigger { get; set; }

    /// <summary>
    /// Implicitly converts a tag reference string into a <see cref="ValueBinding" /> with immediate submission.
    /// </summary>
    public static implicit operator ValueBinding(string reference) => new() { Reference = reference };
}