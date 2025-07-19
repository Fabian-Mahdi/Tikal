using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Controllers.Login.Dtos;
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
            (RefreshToken, AccessToken) tokenPair = await loginUser.Login(dto.Username, dto.Password);

            return new TokenDto
            {
                RefreshToken = tokenPair.Item1.Value,
                AccessToken = tokenPair.Item2.Value
            };
        }
        catch (InvalidCredentials)
        {
            throw new Errors.InvalidCredentials();
        }
    }
}