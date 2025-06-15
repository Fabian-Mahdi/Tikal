using IdentityAPI.Dtos;
using IdentityAPI.Extensions;
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

        TokenPair tokenPair = tokenService.GenerateTokenPair(user);

        HttpContext.Response.Cookies.AddToken("refreshToken", tokenPair.RefreshToken);

        return new TokenDto()
        {
            AccessToken = tokenPair.AccessToken,
        };
    }
}