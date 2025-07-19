using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Controllers.Login.Dtos;
using IdentityAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Login;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginUser loginUser;

    public LoginController(LoginUser loginUser)
    {
        this.loginUser = loginUser;
    }

    [HttpPost]
    public async Task<TokenDto> Login(LoginDto dto)
    {
        try
        {
            (RefreshToken refreshToken, AccessToken accessToken) = await loginUser.Login(dto.Username, dto.Password);

            Response.Cookies.AddRefreshToken(refreshToken.Value);

            return new TokenDto
            {
                AccessToken = accessToken.Value
            };
        }
        catch (InvalidCredentials)
        {
            throw new Errors.InvalidCredentials();
        }
    }
}