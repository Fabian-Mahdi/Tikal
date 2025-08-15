using FluentResults;
using IdentityAPI.Core.Exceptions;
using IdentityAPI.Extensions;
using IdentityAPI.Identity.Domain.Models;
using IdentityAPI.Identity.Domain.UseCases.Refresh;
using IdentityAPI.Identity.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Refresh;

[ApiController]
[Route("[controller]")]
public partial class RefreshController : ControllerBase
{
    private readonly RefreshTokens refreshTokens;

    public RefreshController(RefreshTokens refreshTokens)
    {
        this.refreshTokens = refreshTokens;
    }

    [HttpPost]
    public async Task<ActionResult<TokenDto>> Refresh()
    {
        string? refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return InvalidRefreshToken();
        }

        Result<TokenPair> result = await refreshTokens.Refresh(refreshToken);

        if (result.IsFailed)
        {
            return handleRefreshError(result);
        }

        TokenPair tokenPair = result.Value;

        Response.Cookies.AddRefreshToken(tokenPair.RefreshToken);

        TokenDto tokenDto = new()
        {
            AccessToken = tokenPair.AccessToken
        };

        return Ok(tokenDto);
    }

    private ObjectResult handleRefreshError(Result<TokenPair> result)
    {
        IError? error = result.Errors.OfType<RefreshError>().FirstOrDefault();

        switch (error)
        {
            case InvalidToken _:
                return InvalidRefreshToken();
            default:
                throw new UnknownErrorException();
        }
    }
}