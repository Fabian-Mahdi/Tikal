using Moq;
using OneOf;
using OneOf.Types;
using Tikal.Application.Accounts.Commands.CreateAccount;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Core.DataAccess;
using Tikal.Application.Core.Errors;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Tests.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandlerTests
{
    // dummies
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    // dependencies
    private Mock<AccountRepository> accountRepository;

    private Mock<UnitOfWork> unitOfWork;

    // under test
    private CreateAccountCommandHandler commandHandler;

    [SetUp]
    public void SetUp()
    {
        accountRepository = new Mock<AccountRepository>(MockBehavior.Strict);
        unitOfWork = new Mock<UnitOfWork>(MockBehavior.Strict);

        commandHandler = new CreateAccountCommandHandler(accountRepository.Object, unitOfWork.Object);
    }

    private void SetupMocks(CreateAccountCommand command)
    {
        Account account = new(command.id, command.name);

        accountRepository
            .Setup(r => r.GetAccountById(command.id, cancellationToken))
            .ReturnsAsync((Account?)null);

        accountRepository
            .Setup(r => r.CreateAccount(It.IsAny<Account>(), cancellationToken))
            .ReturnsAsync(account);

        unitOfWork
            .Setup(u => u.SaveChanges(cancellationToken))
            .ReturnsAsync(new None());
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_GetAccountById(CreateAccountCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        accountRepository.Verify(r => r.GetAccountById(command.id, cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_CreateAccount(CreateAccountCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        accountRepository.Verify(r => r.CreateAccount(It.IsAny<Account>(), cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_SaveChanges(CreateAccountCommand command)
    {
        // given
        SetupMocks(command);

        // when
        await commandHandler.Handle(command, cancellationToken);

        // then
        unitOfWork.Verify(u => u.SaveChanges(cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Existing_AccountId_When_Handle_Then_Returns_DuplicateAccountId(
        CreateAccountCommand command
    )
    {
        // given
        SetupMocks(command);

        Account existingAccount = new(command.id, command.name);

        accountRepository
            .Setup(r => r.GetAccountById(command.id, cancellationToken))
            .ReturnsAsync(existingAccount);

        // when
        OneOf<Account, ValidationFailed, DuplicateAccountId> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<DuplicateAccountId>());
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public async Task Given_DatabaseUpdateError_When_Persisting_Changes_Then_Returns_DuplicateAccountId(
        CreateAccountCommand command
    )
    {
        // given
        SetupMocks(command);

        unitOfWork
            .Setup(u => u.SaveChanges(cancellationToken))
            .ReturnsAsync(new DatabaseUpdateError());

        // when
        OneOf<Account, ValidationFailed, DuplicateAccountId> result =
            await commandHandler.Handle(command, cancellationToken);

        // then
        Assert.That(result.Value, Is.InstanceOf<DuplicateAccountId>());
    }
}