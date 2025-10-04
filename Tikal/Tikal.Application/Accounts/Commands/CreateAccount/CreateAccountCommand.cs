using OneOf;
using Tikal.Application.Core.Errors;
using Tikal.Application.Core.Messaging;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

/// <summary>
///     The Command used to create an <see cref="Account" /> with a given id and name
/// </summary>
/// <param name="id">The id of the <see cref="Account" /> to create</param>
/// <param name="name">The name of the <see cref="Account" /> to create</param>
public sealed record CreateAccountCommand(int id, string name)
    : Command<OneOf<Account, ValidationFailed, DuplicateAccountId>>;