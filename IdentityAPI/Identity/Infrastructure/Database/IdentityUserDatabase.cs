using FluentResults;
using IdentityAPI.Identity.Domain.DataAccess.Users;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Infrastructure.Entities;
using IdentityAPI.Identity.Infrastructure.Mappers;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Identity.Infrastructure.Database;

/// <summary>
///     The implementation of the <see cref="UserDataAccess" /> which uses the identity framework
/// </summary>
public class IdentityUserDatabase : UserDataAccess
{
    private readonly UserManager<ApplicationUser> userManager;

    private readonly SignInManager<ApplicationUser> signInManager;

    private readonly UserMapper userMapper;

    public IdentityUserDatabase(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        UserMapper userMapper)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.userMapper = userMapper;
    }

    public async Task<Result> CreateUser(User user, string password)
    {
        ApplicationUser appUser = userMapper.ToEntity(user);

        IdentityResult userCreation = await userManager.CreateAsync(appUser, password);

        if (!userCreation.Succeeded)
        {
            return Result.Fail(new UsernameUniqueConstraint(user.Username));
        }

        IEnumerable<string> roles = user.Roles.Select(role => role.ToString());

        await userManager.AddToRolesAsync(appUser, roles);

        return Result.Ok();
    }

    public async Task<bool> ValidatePassword(User user, string password)
    {
        ApplicationUser? appUser = await userManager.FindByNameAsync(user.Username);

        if (appUser == null)
        {
            return false;
        }

        SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, password, false);

        return result.Succeeded;
    }

    public async Task<User?> FindByName(string username)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            return null;
        }

        return await userMapper.FromEntity(user);
    }
}