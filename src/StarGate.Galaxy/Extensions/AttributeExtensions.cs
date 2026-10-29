using ArchestrA.GRAccess;
using StarGate.Core;
using StarGate.Core.Objects;

namespace StarGate.Galaxy.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IAttribute"/> objects to evaluate and manipulate
/// attributes within the system.
/// </summary>
public static class AttributeExtensions
{
    /// <param name="attribute">The <see cref="IAttribute"/> to convert to a configuration object.</param>
    extension(IAttribute attribute)
    {
        /// <summary>
        /// Gets a value indicating whether the parent object of the current attribute is locked.
        /// </summary>
        /// <remarks>
        /// This property determines if the attribute's locked state is inherited from its parent object.
        /// A value of <c>true</c> indicates that the attribute is locked due to the parent object's lock status,
        /// while <c>false</c> indicates that the lock state is independent of the parent.
        /// </remarks>
        public bool InheritsLock => attribute.Locked == MxPropertyLockedEnum.MxLockedInParent;

        /// <summary>
        /// Retrieves the value of the specified <see cref="IAttribute"/> by accessing its underlying <see cref="IMxValue"/> instance.
        /// </summary>
        /// <returns>The value of the attribute as a .NET object. If the attribute's value is an array, returns an object array containing the array elements.
        /// Returns null if the attribute's value is uninitialized, unassigned, or empty.</returns>
        public object? GetValue()
        {
            return attribute.value.GetValue();
        }

        /// <summary>
        /// Retrieves the length of the specified <see cref="IAttribute"/> instance if it is an array.
        /// </summary>
        /// <returns>
        /// The length of the array as a nullable short.
        /// Returns null if the attribute is not an array or if the upper bound dimension is not greater than zero.
        /// </returns>
        public short? GetLength()
        {
            return attribute.UpperBoundDim1 > 0 ? attribute.UpperBoundDim1 : null;
        }

        /// <summary>
        /// Determines the writeability of the specified <see cref="IAttribute"/> based on its <see cref="MxAttributeCategory"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="Writeability"/> value representing the writeability category of the attribute,
        /// or null if the attribute belongs to the <see cref="MxAttributeCategory.MxCategoryWriteable_USC_Lockable"/> category.
        /// </returns>
        public Writeability? GetWriteability()
        {
            return attribute.AttributeCategory != MxAttributeCategory.MxCategoryWriteable_USC_Lockable
                ? attribute.AttributeCategory.ToCoreType()
                : null;
        }

        /// <summary>
        /// Sets the value of the specified <see cref="IAttribute"/> instance using the provided value.
        /// </summary>
        /// <param name="value">The value to set, which can be of any type compatible with the <see cref="IMxValue"/> instance.</param>
        public GalaxyResult SetValue(object? value)
        {
            if (value is null) return GalaxyResult.Ok();

            attribute.value.Clone(out var clone);
            clone.PutValue(value);
            attribute.SetValue(clone);

            return attribute.CommandResult.ToResult();
        }

        /// <summary>
        /// Sets the locked state of the specified <see cref="IAttribute"/> instance.
        /// </summary>
        /// <param name="locked">A nullable boolean indicating whether the attribute should be locked.
        /// If true, the attribute is locked; if false, it is unlocked. If null, no changes are made to the locked state.
        /// </param>
        public GalaxyResult SetLocked(bool? locked)
        {
            if (!locked.HasValue)
                return GalaxyResult.Ok();

            var setting = locked.Value ? MxPropertyLockedEnum.MxLockedInMe : MxPropertyLockedEnum.MxUnLocked;
            attribute.SetLocked(setting);

            return attribute.CommandResult.ToResult();
        }

        /// <summary>
        /// Sets the security classification for the specified <see cref="IAttribute"/> based on the provided security level.
        /// </summary>
        /// <param name="security">
        /// The <see cref="Security"/> level to assign to the attribute.
        /// If null, the security classification will remain unchanged.
        /// </param>
        public GalaxyResult SetSecurity(Security? security)
        {
            if (!security.HasValue)
                return GalaxyResult.Ok();

            var setting = security.Value.ToMxType();
            attribute.SetSecurityClassification(setting);

            return attribute.CommandResult.ToResult();
        }

        /// <summary>
        /// Determines whether a specified <see cref="IAttribute"/> is configurable within the system.
        /// </summary>
        /// <returns><c>true</c> if the specified attribute is configurable based on its category and naming conventions; otherwise, <c>false</c>.</returns>
        public bool IsConfigurable()
        {
            // We will not map back any internal system attributes that start with an underscore.
            if (attribute.Name.StartsWith("_") || attribute.Name.Contains("._"))
                return false;

            // We can't set locked attributes, so filter them as well.
            if (attribute.Locked == MxPropertyLockedEnum.MxLockedInParent)
                return false;

            // Based on documentation and review, these categories represent attributes that can be set from the IDE.
            return attribute.AttributeCategory switch
            {
                MxAttributeCategory.MxCategoryPackageOnly => true,
                MxAttributeCategory.MxCategoryPackageOnly_Lockable => true,
                MxAttributeCategory.MxCategoryWriteable_UC => true,
                MxAttributeCategory.MxCategoryWriteable_USC => true,
                MxAttributeCategory.MxCategoryWriteable_UC_Lockable => true,
                MxAttributeCategory.MxCategoryWriteable_USC_Lockable => true,
                MxAttributeCategory.MxCategoryWriteable_C_Lockable => true,
                _ => false
            };
        }
    }
}