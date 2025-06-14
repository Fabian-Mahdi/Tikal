using IdentityAPI.Dtos;
using IdentityAPI.Models;
using IdentityAPI.Services.TokenService;
using IdentityAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ITokenService tokenService;
    private readonly IUserService userService;

    public LoginController(ITokenService tokenService, IUserService userService)
    {
        this.tokenService = tokenService;
        this.userService = userService;
    }

    [HttpPost]
    public async Task<TokenDto> Post([FromBody] LoginDto loginDto)
    {
        User user = await userService.GetUser(loginDto.Username, loginDto.Password);

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