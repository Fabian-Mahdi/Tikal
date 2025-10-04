using FluentValidation.TestHelper;
using Identity.Application.Identity.Commands.Login;

namespace Identity.Application.Tests.Identity.Commands.Login;

public class LoginCommandValidatorTests
{
    // under test
    private LoginCommandValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator = new LoginCommandValidator();
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.ValidTestCases)
    )]
    public void Given_Valid_Command_When_Validate_Then_Has_No_Errors(LoginCommand command)
    {
        // when
        TestValidationResult<LoginCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCaseSource(
        typeof(LoginCommandTestCasesSource),
        nameof(LoginCommandTestCasesSource.InvalidTestCases)
    )]
    public void Given_Invalid_Command_When_Validate_Then_Has_Errors(LoginCommand command)
    {
        // when
        TestValidationResult<LoginCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldHaveValidationErrors();
    }
}