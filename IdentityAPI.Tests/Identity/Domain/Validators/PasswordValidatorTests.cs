using FluentValidation.Results;
using IdentityAPI.Identity.Domain.Validators;
using IdentityAPI.Tests.Data.Other;

namespace IdentityAPI.Tests.Identity.Domain.Validators;

public class PasswordValidatorTests
{
    // under tests
    private PasswordValidator passwordValidator;

    [SetUp]
    public void Setup()
    {
        passwordValidator = new PasswordValidator();
    }

    [TestCaseSource(typeof(ValidPasswordSource), nameof(ValidPasswordSource.TestCases))]
    public async Task Given_Valid_Password_When_Validate_Then_Return_Success(string password)
    {
        // when
        ValidationResult validationResult = await passwordValidator.ValidateAsync(password);

        // then
        Assert.That(validationResult.IsValid);
    }

    [TestCaseSource(typeof(InvalidPasswordSource), nameof(InvalidPasswordSource.TestCases))]
    public async Task Given_Invalid_Password_When_Validate_Then_Return_Error(string password)
    {
        // when
        ValidationResult validationResult = await passwordValidator.ValidateAsync(password);

        // then
        Assert.That(validationResult.IsValid, Is.False);
    }
}