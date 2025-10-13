using Moq;
using Tikal.Application.Accounts.DataAccess;
using Tikal.Application.Accounts.Queries.GetAccount;
using Tikal.Domain.Accounts;

namespace Tikal.Application.Tests.Accounts.Queries.GetAccount;

public class GetAccountQueryHandlerTests
{
    // dummies
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    // dependencies
    private Mock<AccountRepository> accountRepository;

    // under test
    private GetAccountQueryHandler queryHandler;

    [SetUp]
    public void Setup()
    {
        accountRepository = new Mock<AccountRepository>();

        queryHandler = new GetAccountQueryHandler(accountRepository.Object);
    }

    private void SetupMocks(GetAccountQuery query)
    {
        Account testAccount = new(query.id, "username");

        accountRepository
            .Setup(r => r.GetAccountById(query.id, cancellationToken))
            .ReturnsAsync(testAccount);
    }

    [TestCaseSource(
        typeof(GetAccountQueryTestCasesSource),
        nameof(GetAccountQueryTestCasesSource.ValidTestCases)
    )]
    public async Task Given_Valid_Command_When_Handle_Then_Calls_GetAccountById(GetAccountQuery query)
    {
        // given
        SetupMocks(query);

        // when
        await queryHandler.Handle(query, cancellationToken);

        // then
        accountRepository.Verify(r => r.GetAccountById(query.id, cancellationToken), Times.Once);
    }

    [TestCaseSource(
        typeof(GetAccountQueryTestCasesSource),
        nameof(GetAccountQueryTestCasesSource.ValidTestCases)
    )]
    public async Task Given_NonExistent_Account_With_Id_When_Handle_Then_Returns_Null(GetAccountQuery query)
    {
        // given
        accountRepository
            .Setup(r => r.GetAccountById(query.id, cancellationToken))
            .ReturnsAsync((Account?)null);

        // when
        Account? account = await queryHandler.Handle(query, cancellationToken);

        // then
        Assert.That(account, Is.Null);
    }
}