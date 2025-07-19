using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Entities;
using IdentityAPI.Authentication.Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Authentication.Infrastructure.Identity;

public class IdentityUserService : UserDataAccess
{
    private readonly UserManager<ApplicationUser> userManager;

    private readonly UserMapper userMapper;

    public IdentityUserService(UserManager<ApplicationUser> userManager, UserMapper userMapper)
    {
        this.userManager = userManager;
        this.userMapper = userMapper;
    }

    public async Task<UserCreationResult> CreateUser(User user, string password)
    {
        ApplicationUser appUser = new()
        {
            UserName = user.Username
        };

        IdentityResult userCreation = await userManager.CreateAsync(appUser, password);

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

    public async Task<User?> FindByName(string name)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(name);

        if (user == null)
        {
            return null;
        }

        return await userMapper.FromEntity(user);
    }
}