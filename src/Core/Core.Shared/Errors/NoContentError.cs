namespace Core.Shared.Errors;

/// <summary>
/// Represents the no content error.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NoContentError"/> class.
/// </remarks>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public sealed class NoContentError(Error error) : Error(error.Code, error.Message) { }