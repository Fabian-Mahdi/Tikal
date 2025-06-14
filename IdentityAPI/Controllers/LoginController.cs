using IdentityAPI.Dtos;
using IdentityAPI.Models;
using IdentityAPI.Services.TokenService;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ITokenService tokenService;

    public LoginController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpPost]
    public TokenDto Post()
    {
        TokenPair tokenPair = tokenService.GenerateTokenPair(Guid.NewGuid(), "username");

        HttpContext.Response.Cookies.Append("refreshToken", tokenPair.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });

        return new TokenDto()
        {
            AccessToken = tokenPair.AccessToken,
        };
    }
}