using FluentResults;

namespace IdentityAPI.Identity.Domain.UseCases.Refresh;

/// <summary>
///     The base class for all refresh errors
/// </summary>
public class RefreshError : Error
{
}

/// <summary>
///     The provided token is not valid
/// </summary>
/// <remarks>Can have a lot of causes, maybe interesting to distinguish between them in the future</remarks>
public class InvalidToken : RefreshError
{
}