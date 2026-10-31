namespace StarGate.Core;

/// <summary>
/// Defines the error and result codes returned by Galaxy repository operations.
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// An unspecified or unknown error occurred during the operation.
    /// </summary>
    UnknownError = -1,

    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Success = 0,

    /// <summary>
    /// The current user does not have enough permissions to perform the requested operation.
    /// </summary>
    InsufficientPermissions = 101,

    /// <summary>
    /// The specified Galaxy Repository node could not be found or reached.
    /// </summary>
    NoSuchGalaxyNode = 102,

    /// <summary>
    /// The specified user account does not exist in the Galaxy security configuration.
    /// </summary>
    NoSuchUser = 103,

    /// <summary>
    /// The provided password for user authentication is incorrect.
    /// </summary>
    InvalidPassword = 104,

    /// <summary>
    /// A valid license is not available to execute the requested operation.
    /// </summary>
    LicenseUnavailable = 105,

    /// <summary>
    /// The specified file could not be found.
    /// </summary>
    NoSuchFile = 201,

    /// <summary>
    /// The system could not create the specified file.
    /// </summary>
    CouldNotCreateFile = 202,

    /// <summary>
    /// The target object is currently checked out.
    /// </summary>
    ObjectIsCheckedOut = 301,

    /// <summary>
    /// The target object is currently checked out to another user.
    /// </summary>
    ObjectIsCheckedOutToSomeoneElse = 302,

    /// <summary>
    /// The target object is not checked out to the current user.
    /// </summary>
    ObjectNotCheckedOutToMe = 303,

    /// <summary>
    /// The target object cannot be overwritten.
    /// </summary>
    ObjectCannotBeOverwritten = 401,

    /// <summary>
    /// The specified template is currently in use and cannot be modified or deleted.
    /// </summary>
    TemplateInUse = 402,

    /// <summary>
    /// The target object is a container object and cannot perform the requested operation.
    /// </summary>
    ObjectIsAContainer = 403,

    /// <summary>
    /// The host object associated with the target could not be found.
    /// </summary>
    ObjectHostNotFound = 404,

    /// <summary>
    /// The instance object is configured as a host.
    /// </summary>
    InstanceIsHost = 405,

    /// <summary>
    /// The target object is required by the system and cannot be deleted or unassigned.
    /// </summary>
    ObjectIsRequired = 406,

    /// <summary>
    /// The specified object is not a valid automation object.
    /// </summary>
    ObjectNotAnAutomationObject = 407,

    /// <summary>
    /// The target object is in an invalid or corrupted state.
    /// </summary>
    ObjectInBadState = 408,

    /// <summary>
    /// A custom configuration error occurred on the target object.
    /// </summary>
    CustomConfigurationError = 409,

    /// <summary>
    /// The target object is in read-only mode and cannot be modified.
    /// </summary>
    ObjectInReadOnlyMode = 410,

    /// <summary>
    /// The host object for the target is not currently deployed.
    /// </summary>
    ObjectHostNotDeployed = 501,

    /// <summary>
    /// The instance object is currently deployed.
    /// </summary>
    InstanceIsDeployed = 502,

    /// <summary>
    /// An invalid load mode was specified for the Galaxy repository.
    /// </summary>
    InvalidGalaxyLoadMode = 503,

    /// <summary>
    /// The OEM version is incompatible with the current Galaxy repository.
    /// </summary>
    IncompatibleOemVersion = 504,

    /// <summary>
    /// The client synchronization failed and must be retried.
    /// </summary>
    RetryClientSync = 505,

    /// <summary>
    /// A synchronization semaphore conflict occurred.
    /// </summary>
    SyncSemaphore = 506,

    /// <summary>
    /// The client or object is out of synchronization with the Galaxy repository.
    /// </summary>
    OutOfSync = 507,

    /// <summary>
    /// The operation was denied due to instance protection settings.
    /// </summary>
    InstanceProtectionDenied = 508,

    // --- Model & Validation Errors (1000+) ---
    // These are custom application-defined error codes.

    /// <summary>
    /// The model or configuration object failed general validation.
    /// </summary>
    ValidationFailed = 1000,

    /// <summary>
    /// Indicates that a required attribute is missing from an object or entity,
    /// preventing the operation from proceeding.
    /// </summary>
    MissingAttribute = 1001,

    /// <summary>
    /// The data type for an attribute was not specified or could not be determined.
    /// </summary>
    MissingDataType = 1002,

    /// <summary>
    /// An invalid or unsupported data type was provided.
    /// </summary>
    InvalidDataType = 1003,

    /// <summary>
    /// An attribute configuration value is invalid or out of bounds.
    /// </summary>
    InvalidAttributeValue = 1004,

    /// <summary>
    /// An invalid security classification or writeability level was provided.
    /// </summary>
    InvalidSecurityClassification = 1005,

    /// <summary>
    /// The specified string length or array dimension is invalid for the data type.
    /// </summary>
    InvalidLength = 1006,

    /// <summary>
    /// Indicates that the specified object could not be found.
    /// </summary>
    ObjectNotFound = 1100,
    InvalidExtension
}