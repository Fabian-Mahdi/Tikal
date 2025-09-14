using OneOf;
using Shared.Abstractions.Messaging;
using TikalBackend.PlayerAccount.Domain.Models;

namespace TikalBackend.PlayerAccount.Domain.Commands.CreateAccount;

public sealed record CreateAccountCommand(string id, string name)
    : Command<OneOf<Account, DuplicateAccountId>>;