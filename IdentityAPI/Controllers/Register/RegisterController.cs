using IdentityAPI.Controllers.Register.Errors;
using IdentityAPI.Dtos;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Register;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly UserManager<User> userManager;

    public RegisterController(UserManager<User> userManager)
    {
        this.userManager = userManager;
    }

    [HttpPost]
    public async Task Register(RegisterDto registerDto)
    {
        User user = new()
        {
            UserName = registerDto.Username
        };

        IdentityResult userCreation = await userManager.CreateAsync(user, registerDto.Password);

        if (!userCreation.Succeeded)
        {
            throw new UserCreationFailedException(userCreation.Errors);
        }

        IdentityResult roleAssignment = await userManager.AddToRoleAsync(user, "User");

        if (!roleAssignment.Succeeded)
        {
            throw new RoleAssignmentFailedException(roleAssignment.Errors);
        }
    }
}