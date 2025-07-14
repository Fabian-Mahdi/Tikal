using IdentityAPI.Controllers.Register;
using IdentityAPI.Controllers.Register.Errors;
using IdentityAPI.Dtos;
using IdentityAPI.Models;
using IdentityAPI.Tests.Data.Dtos;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityAPI.Tests.Controllers.Register;

internal class RegisterControllerTests
{
    // constants
    private readonly IdentityResult successfulResult = IdentityResult.Success;

    private readonly IdentityResult failedResult = IdentityResult.Failed(new IdentityError());

    // dependencies
    private Mock<UserManager<User>> userManager;

    // under test
    private RegisterController registerController;

    [SetUp]
    public void Setup()
    {
        userManager = MockHelpers.MockUserManager<User>();

        userManager
            .Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(successfulResult);

        userManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(successfulResult);

        registerController = new RegisterController(userManager.Object);
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.TestCases))]
    public async Task Given_RegisterDto_When_Register_Then_Calls_UserManager_CreateAsync(RegisterDto registerDto)
    {
        // when
        await registerController.Register(registerDto);

        // then
        userManager.Verify(m => m.CreateAsync(It.IsAny<User>(), registerDto.Password));
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.TestCases))]
    public async Task Given_RegisterDto_When_Register_Then_Calls_UserManager_AddToRoleAsync(RegisterDto registerDto)
    {
        // when
        await registerController.Register(registerDto);

        // then
        userManager.Verify(m => m.AddToRoleAsync(It.IsAny<User>(), "User"));
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.TestCases))]
    public void Given_Failure_When_CreateAsync_Then_Throws_UserCreatedFailedException(RegisterDto registerDto)
    {
        // given
        userManager
            .Setup(m => m.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(failedResult);

        // when & then
        Assert.ThrowsAsync<UserCreationFailedException>(async () => await registerController.Register(registerDto)
        );
    }

    [TestCaseSource(typeof(RegisterDtoSource), nameof(RegisterDtoSource.TestCases))]
    public void Given_Failure_When_AddToRoleAsync_Then_Throws_RoleAssignmentFailedException(RegisterDto registerDto)
    {
        // given
        userManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(failedResult);

        // when & then
        Assert.ThrowsAsync<RoleAssignmentFailedException>(async () => await registerController.Register(registerDto)
        );
    }
}