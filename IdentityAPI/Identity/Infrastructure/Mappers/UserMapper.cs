using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Identity.Infrastructure.Mappers;

/// <summary>
///     Maps between <see cref="User" /> and <see cref="ApplicationUser" />
/// </summary>
public interface UserMapper
{
    /// <summary>
    ///     Maps an <see cref="ApplicationUser" /> to a <see cref="User" />
    /// </summary>
    /// <param name="entity">The entity to be mapped</param>
    /// <returns>The resulting model</returns>
    Task<User> FromEntity(ApplicationUser entity);

    /// <summary>
    ///     Maps a <see cref="User" /> to an <see cref="ApplicationUser" />
    /// </summary>
    /// <param name="user">The model to be mapped</param>
    /// <returns>The resulting entity</returns>
    ApplicationUser ToEntity(User user);
}

/// <summary>
///     Default implementation of the <see cref="UserMapper" />
/// </summary>
public class UserMapperImpl : UserMapper
{
    private readonly UserManager<ApplicationUser> userManager;

    public UserMapperImpl(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<User> FromEntity(ApplicationUser entity)
    {
        User user = new(entity.UserName ?? string.Empty, entity.Id);

        IEnumerable<string> roles = await userManager.GetRolesAsync(entity);

        foreach (string role in roles)
        {
            Role roleType = Enum.Parse<Role>(role);

            user.Roles.Add(roleType);
        }

        return user;
    }

    public ApplicationUser ToEntity(User user)
    {
        return new ApplicationUser
        {
            Id = user.Id,
            UserName = user.Username
        };
    }
}