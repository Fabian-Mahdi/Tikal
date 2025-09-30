using Identity.Application.Core.Errors;
using Identity.Application.Core.Messaging;
using Identity.Domain.Identity;
using OneOf;

namespace Identity.Application.Identity.Commands.RegisterCommand;

/// <summary>
///     The Command used to register a new <see cref="User" /> with a given username and password
/// </summary>
/// <param name="username">The username of the user to register</param>
/// <param name="password">The password of the user to register</param>
public sealed record RegisterCommand(string username, string password)
    : Command<OneOf<User, ValidationFailed, DuplicateUsername>>;