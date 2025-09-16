using OneOf;
using Tikal.Application.Abstractions.Messaging;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Commands.CreateAccount;

public sealed record CreateAccountCommand(string id, string name)
    : Command<OneOf<Account, DuplicateAccountId>>;