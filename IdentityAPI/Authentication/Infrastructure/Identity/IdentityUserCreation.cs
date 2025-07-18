using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Authentication.Infrastructure.Identity;

public class IdentityUserCreation : UserCreationDataAccess
{
    private readonly UserManager<ApplicationUser> userManager;

    public IdentityUserCreation(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<UserCreationResult> CreateUser(User user)
    {
        ApplicationUser appUser = new()
        {
            UserName = user.Username
        };

        IdentityResult userCreation = await userManager.CreateAsync(appUser, user.Password);

        if (!userCreation.Succeeded)
        {
            return UserCreationResult.Failed(userCreation.Errors.Select(error => error.Description));
        }

        IdentityResult roleAssignment =
            await userManager.AddToRolesAsync(appUser, user.Roles.Select(role => role.Type.ToString()));

        if (!roleAssignment.Succeeded)
        {
            return UserCreationResult.Failed(roleAssignment.Errors.Select(error => error.Description));
        }

        return UserCreationResult.Success();
    }
}