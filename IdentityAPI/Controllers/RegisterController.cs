using IdentityAPI.Dtos;
using IdentityAPI.Models;
using IdentityAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IUserService userService;

    public RegisterController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost]
    public async Task<UserDto> Post([FromBody] RegisterDto registerDto)
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Password = registerDto.Password,
        };

        User createdUser = await userService.CreateUser(user);

        return new UserDto()
        {
            Id = createdUser.Id,
            Username = createdUser.Username,
        };
    }
}