using FluentResults;
using IdentityAPI.Core.Exceptions;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Register;
using IdentityAPI.Identity.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Register;

[ApiController]
[Route("[controller]")]
public partial class RegisterController : ControllerBase
{
    private readonly RegisterUser registerUser;

    public RegisterController(RegisterUser registerUser)
    {
        this.registerUser = registerUser;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        User user = new(registerDto.Username);

        Result result = await registerUser.Register(user, registerDto.Password);

        if (result.IsFailed)
        {
            return handleRegisterError(result);
        }

        return Ok();
    }

    private ObjectResult handleRegisterError(Result result)
    {
        IError? error = result.Errors.OfType<RegisterError>().FirstOrDefault();

        switch (error)
        {
            case InvalidUsername invalidUsername:
                return InvalidUsername(invalidUsername.Username);
            case DuplicateUserName duplicateUserName:
                return UsernameConflict(duplicateUserName.Username);
            case InvalidPassword _:
                return InvalidPassword();
            default:
                throw new UnknownErrorException();
        }
    }
}