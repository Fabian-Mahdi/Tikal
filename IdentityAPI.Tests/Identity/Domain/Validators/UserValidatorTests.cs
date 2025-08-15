using FluentValidation.Results;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.Validators;
using IdentityAPI.Tests.Data.Models;

namespace IdentityAPI.Tests.Identity.Domain.Validators;

public class UserValidatorTests
{
    // under tests
    private UserValidator userValidator;

    [SetUp]
    public void Setup()
    {
        userValidator = new UserValidator();
    }

    [TestCaseSource(typeof(ValidUserSource), nameof(ValidUserSource.TestCases))]
    public async Task Given_Valid_User_When_Validate_Then_Return_Success(User user)
    {
        // when
        ValidationResult validationResult = await userValidator.ValidateAsync(user);

        // then
        Assert.That(validationResult.IsValid);
    }

    [TestCaseSource(typeof(InvalidUserSources), nameof(InvalidUserSources.TestCases))]
    public async Task Given_Invalid_User_When_Validate_Then_Return_Failure(User user)
    {
        // when
        ValidationResult validationResult = await userValidator.ValidateAsync(user);

        // then
        Assert.That(validationResult.IsValid, Is.False);
    }
}