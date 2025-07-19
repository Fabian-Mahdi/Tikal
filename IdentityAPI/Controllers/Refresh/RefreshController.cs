using IdentityAPI.Authentication.Domain.Errors;
using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Domain.UseCases;
using IdentityAPI.Controllers.Login.Dtos;
using IdentityAPI.Controllers.Refresh.Errors;
using IdentityAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Refresh;

[ApiController]
[Route("[controller]")]
public class RefreshController : ControllerBase
{
    private readonly RefreshTokens refreshTokens;

    public RefreshController(RefreshTokens refreshTokens)
    {
        this.refreshTokens = refreshTokens;
    }

    [HttpPost]
    public async Task<TokenDto> Refresh()
    {
        string? token = Request.Cookies["refreshToken"];

        if (token == null)
        {
            throw new InvalidRefreshTokenException();
        }

        try
        {
            (RefreshToken refreshToken, AccessToken accessToken) = await refreshTokens.Refresh(token);

            Response.Cookies.AddRefreshToken(refreshToken.Value);

            return new TokenDto
            {
                AccessToken = accessToken.Value
            };
        }
        catch (InvalidTokenException)
        {
            throw new InvalidRefreshTokenException();
        }
    }
}