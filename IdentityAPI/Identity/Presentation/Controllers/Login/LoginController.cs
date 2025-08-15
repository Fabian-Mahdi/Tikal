using FluentResults;
using IdentityAPI.Core.Exceptions;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Login;
using IdentityAPI.Identity.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Login;

[ApiController]
[Route("[controller]")]
public partial class LoginController : ControllerBase
{
    private readonly LoginUser loginUser;

    public LoginController(LoginUser loginUser)
    {
        this.loginUser = loginUser;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
    {
        Result<TokenPair> result = await loginUser.Login(loginDto.Username, loginDto.Password);

        if (result.IsFailed)
        {
            return handleLoginError(result);
        }

        TokenPair tokenPair = result.Value;

        TokenDto tokenDto = new()
        {
            AccessToken = tokenPair.AccessToken,
            RefreshToken = tokenPair.RefreshToken
        };

        return Ok(tokenDto);
    }

    private ObjectResult handleLoginError(Result<TokenPair> result)
    {
        IError? error = result.Errors.OfType<LoginError>().FirstOrDefault();

        switch (error)
        {
            case InvalidCredentials _:
                return InvalidCredentials();
            default:
                throw new UnknownErrorException();
        }
    }
}