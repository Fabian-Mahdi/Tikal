using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityAPI.Tests;

internal static class MockHelpers
{
    public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        Mock<IUserStore<TUser>> store = new();
#pragma warning disable CS8625
        Mock<UserManager<TUser>> mgr = new(store.Object, null, null, null, null, null, null, null, null);
#pragma warning restore CS8625
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
        return mgr;
    }

    public static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
    {
        return new Mock<SignInManager<TUser>>(
            MockUserManager<TUser>().Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<TUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<ILogger<SignInManager<TUser>>>().Object,
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<TUser>>().Object
        );
    }
}