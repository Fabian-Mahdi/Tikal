using Tikal.Application.Core.Messaging;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Accounts.Queries.GetAccount;

/// <summary>
///     Queries for an <see cref="Account" /> with a given id
/// </summary>
/// <param name="id">The id of the <see cref="Account" /> to query for</param>
public sealed record GetAccountQuery(int id)
    : Query<Account?>;