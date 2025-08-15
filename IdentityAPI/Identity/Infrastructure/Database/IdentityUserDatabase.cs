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

    private readonly UserMapper userMapper;

    public IdentityUserDatabase(UserManager<ApplicationUser> userManager, UserMapper userMapper)
    {
        this.userManager = userManager;
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
}