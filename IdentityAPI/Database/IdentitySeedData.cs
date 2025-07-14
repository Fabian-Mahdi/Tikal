using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Database;

public static class IdentitySeedData
{
    private static readonly string[] roleNames = ["Admin", "User"];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (string roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            IdentityRole role = new()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };

            await roleManager.CreateAsync(role);
        }
    }
}