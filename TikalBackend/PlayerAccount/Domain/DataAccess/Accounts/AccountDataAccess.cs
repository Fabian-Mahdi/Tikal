using FluentResults;
using TikalBackend.PlayerAccount.Domain.Models;

namespace TikalBackend.PlayerAccount.Domain.DataAccess.Accounts;

public interface AccountDataAccess
{
    Task<Result<Account>> CreateAccount(Account account);
}