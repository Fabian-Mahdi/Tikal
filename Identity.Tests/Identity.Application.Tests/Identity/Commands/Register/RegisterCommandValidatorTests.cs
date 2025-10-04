using FluentValidation.TestHelper;
using Identity.Application.Identity.Commands.Register;

namespace Identity.Application.Tests.Identity.Commands.Register;

public class RegisterCommandValidatorTests
{
    // under test
    private RegisterCommandValidator validator;

    [SetUp]
    public void Setup()
    {
        validator = new RegisterCommandValidator();
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.ValidTestCases)
    )]
    public void Given_Valid_Command_When_Validate_Then_Has_No_Errors(RegisterCommand command)
    {
        // when
        TestValidationResult<RegisterCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestCaseSource(
        typeof(RegisterCommandTestCasesSource),
        nameof(RegisterCommandTestCasesSource.InvalidTestCases)
    )]
    public void Given_Invalid_Command_When_Validate_Then_Has_Errors(RegisterCommand command)
    {
        // when
        TestValidationResult<RegisterCommand>? result = validator.TestValidate(command);

        // then
        result.ShouldHaveValidationErrors();
    }
}