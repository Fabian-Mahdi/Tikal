using FluentValidation.TestHelper;
using Tikal.Application.Accounts.Commands.CreateAccount;

namespace Tikal.Application.Tests.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidatorTests
{
    // under test
    private CreateAccountCommandValidator validator;

    [SetUp]
    public void Setup()
    {
        validator = new CreateAccountCommandValidator();
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.ValidTestCases)
    )]
    public void Given_Valid_Command_When_Validate_Then_Has_No_Errors(CreateAccountCommand command)
    {
        // when
        TestValidationResult<CreateAccountCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCaseSource(
        typeof(CreateAccountCommandTestCasesSource),
        nameof(CreateAccountCommandTestCasesSource.InvalidTestCases)
    )]
    public void Given_Invalid_Command_When_Validate_Then_Has_Errors(CreateAccountCommand command)
    {
        // when
        TestValidationResult<CreateAccountCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldHaveValidationErrors();
    }
}