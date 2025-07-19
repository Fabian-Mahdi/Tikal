using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Entities;
using IdentityAPI.Authentication.Infrastructure.Identity;
using IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;
using IdentityAPI.Tests.Data.Models;
using IdentityAPI.Tests.MockHelpers;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IdentityAPI.Tests.Authentication.Infrastructure.Identity;

public class IdentityUserServiceTests
{
    // constants
    private readonly IdentityResult successfulResult = IdentityResult.Success;

    private readonly IdentityResult failedResult = IdentityResult.Failed(new IdentityError());

    private const string testPassword = "password";

    // dependencies
    private Mock<UserManager<ApplicationUser>> userManager;

    private Mock<IUserMapper> userMapper;

    // under test
    private IdentityUserService identityUserService;

    [SetUp]
    public void Setup()
    {
        userManager = IdentityMockHelpers.MockUserManager<ApplicationUser>();

        userManager
            .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(successfulResult);

        userManager
            .Setup(u => u.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(successfulResult);

        userMapper = new Mock<IUserMapper>();

        identityUserService = new IdentityUserService(userManager.Object, userMapper.Object);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_CreateUser_Then_Calls_UserManager_CreateAsync(User user)
    {
        // when
        await identityUserService.CreateUser(user, testPassword);

        // then
        userManager.Verify(u => u.CreateAsync(It.IsAny<ApplicationUser>(), testPassword), Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_User_When_CreateUser_Then_Calls_UserManager_AddToRolesAsync(User user)
    {
        // when
        await identityUserService.CreateUser(user, testPassword);

        // then
        userManager.Verify(u =>
                u.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()),
            Times.Once);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_Successful_Operations_When_CreateUser_Then_Returns_Successful_UserCreationResult(User user)
    {
        // when
        UserCreationResult result = await identityUserService.CreateUser(user, testPassword);

        // then
        Assert.That(result.Succeeded, Is.True);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_UserManager_CreateAsync_Fails_When_CreateUser_Then_Returns_Failed_UserCreationResult(
        User user
    )
    {
        // given
        userManager
            .Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), testPassword))
            .ReturnsAsync(failedResult);

        // when
        UserCreationResult result = await identityUserService.CreateUser(user, testPassword);

        // then
        Assert.That(result.Succeeded, Is.False);
    }

    [TestCaseSource(typeof(UserSource), nameof(UserSource.TestCases))]
    public async Task Given_UserManager_AddToRolesAsync_Fails_When_CreateUser_Then_Returns_Failed_UserCreationResult(
        User user
    )
    {
        // given
        userManager
            .Setup(u => u.AddToRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(failedResult);

        // when
        UserCreationResult result = await identityUserService.CreateUser(user, testPassword);

        // then
        Assert.That(result.Succeeded, Is.False);
    }
}