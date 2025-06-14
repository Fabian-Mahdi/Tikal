using IdentityAPI.Models;
using IdentityAPI.Services.TokenService;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController
{
    private readonly ITokenService tokenService;

    public LoginController(ITokenService tokenService)
    {
        this.tokenService = tokenService;
    }

    [HttpPost]
    public TokenPair Post()
    {
        return tokenService.GenerateTokenPair(Guid.NewGuid(), "username");
    }
}