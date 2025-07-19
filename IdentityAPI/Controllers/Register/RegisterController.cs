using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Controllers.Register.Dtos;
using IdentityAPI.Controllers.Register.Errors;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Register;

[ApiController]
[Route("[controller]")]
public class RegisterController : ControllerBase
{
    private readonly RegisterUser registerUser;

    public RegisterController(RegisterUser registerUser)
    {
        this.registerUser = registerUser;
    }

    [HttpPost]
    public async Task Register(RegisterDto registerDto)
    {
        User user = new(registerDto.Username);

        RegisterResult result = await registerUser.Register(user, registerDto.Password);

        if (!result.Succeeded)
        {
            throw new RegistrationFailedException(result.Errors);
        }
    }
}