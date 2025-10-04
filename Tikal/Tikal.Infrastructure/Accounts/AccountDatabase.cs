using Microsoft.EntityFrameworkCore;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Domain.Accounts;
using Tikal.Infrastructure.Accounts.Mappers;
using Tikal.Infrastructure.Database;
using Tikal.Infrastructure.Entities;

namespace Tikal.Infrastructure.Accounts;

public class AccountDatabase : AccountRepository
{
    private readonly AccountMapper accountMapper;

    private readonly ApplicationDbContext dbContext;

    public AccountDatabase(ApplicationDbContext dbContext, AccountMapper accountMapper)
    {
        this.dbContext = dbContext;
        this.accountMapper = accountMapper;
    }

    public async Task<Account> CreateAccount(Account account, CancellationToken cancellationToken)
    {
        AccountEntity accountEntity = accountMapper.FromAccount(account);

        await dbContext.Accounts.AddAsync(accountEntity, cancellationToken);

        Account createdAccount = accountMapper.ToAccount(accountEntity);

        return createdAccount;
    }

    public async Task<Account?> GetAccountById(int id, CancellationToken cancellationToken)
    {
        AccountEntity? accountEntity =
            await dbContext.Accounts.FirstOrDefaultAsync(account => account.Id == id, cancellationToken);

        if (accountEntity is null)
        {
            return null;
        }

        Account account = accountMapper.ToAccount(accountEntity);

        return account;
    }
}