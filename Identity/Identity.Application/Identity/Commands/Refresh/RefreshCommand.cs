using Identity.Application.Core.Messaging;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.Refresh;

/// <summary>
///     The Command used to refresh a given token with a new pair,
///     the refreshed <see cref="TokenPair" /> will have the same claims as the given token
/// </summary>
/// <param name="token">The token which should be refreshed</param>
public sealed record RefreshCommand(string token)
    : Command<OneOf<TokenPair, InvalidToken>>;