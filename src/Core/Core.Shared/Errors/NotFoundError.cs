namespace Core.Shared.Errors;

/// <summary>
/// Represents the not found error.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NotFoundError"/> class.
/// </remarks>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public sealed class NotFoundError(Error error) : Error(error.Code, error.Message) { }
