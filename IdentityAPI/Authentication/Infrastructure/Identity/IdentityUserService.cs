using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;
using IdentityAPI.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Authentication.Infrastructure.Identity;

public class IdentityUserService : UserDataAccess
{
    private readonly UserManager<ApplicationUser> userManager;

    private readonly IUserMapper userMapper;

    public IdentityUserService(UserManager<ApplicationUser> userManager, IUserMapper userMapper)
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

        IEnumerable<string> roles = user.Roles.Select(role => role.Type.ToString());

        IdentityResult roleAssignment = await userManager.AddToRolesAsync(appUser, roles);

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