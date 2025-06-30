using IdentityAPI.Controllers.Login.Errors;
using IdentityAPI.Dtos;
using IdentityAPI.Extensions;
using IdentityAPI.Models;
using IdentityAPI.Services.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.Controllers.Login;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserManager<User> userManager;

    private readonly SignInManager<User> signInManager;

    private readonly ITokenService tokenService;

    public LoginController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService
        )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.tokenService = tokenService;
    }

    [HttpPost]
    public async Task<TokenDto> Login(LoginDto loginDto)
    {
        User user = await userManager.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.Username)
            ?? throw new InvalidCredentialsException();

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            throw new InvalidCredentialsException();
        }

        TokenPair tokenPair = tokenService.GenerateTokenPair(user);

        HttpContext.Response.Cookies.AddRefreshToken(tokenPair.RefreshToken);

        return new TokenDto()
        {
            AccessToken = tokenPair.AccessToken,
        };
    }
}