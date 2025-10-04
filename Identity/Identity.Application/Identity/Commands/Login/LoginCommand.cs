using Identity.Application.Core.Errors;
using Identity.Application.Core.Messaging;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.Login;

/// <summary>
///     The Command used to log in with a pair of credentials
/// </summary>
/// <param name="username">The username of the credentials</param>
/// <param name="password">The password of the credentials</param>
public sealed record LoginCommand(string username, string password)
    : Command<OneOf<TokenPair, ValidationFailed, InvalidCredentials>>;