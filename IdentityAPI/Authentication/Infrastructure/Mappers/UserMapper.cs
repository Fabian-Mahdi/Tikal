using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;
using IdentityAPI.Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Authentication.Infrastructure.Mappers;

public class UserMapper : IUserMapper
{
    private readonly UserManager<ApplicationUser> userManager;

    public UserMapper(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<User> FromEntity(ApplicationUser entity)
    {
        User user = new(entity.Id, entity.UserName ?? string.Empty);

        IEnumerable<string> roles = await userManager.GetRolesAsync(entity);

        foreach (string role in roles)
        {
            RoleType roleType = Enum.Parse<RoleType>(role);

            user.AddRole(roleType);
        }

        return user;
    }
}