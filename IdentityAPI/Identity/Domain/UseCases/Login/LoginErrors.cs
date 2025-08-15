using FluentResults;

namespace IdentityAPI.Identity.Domain.UseCases.Login;

/// <summary>
///     The base class for all login errors
/// </summary>
public class LoginError : Error
{
}

/// <summary>
///     Wrong username or password provided
/// </summary>
public class InvalidCredentials : LoginError
{
}