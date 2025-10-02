using Identity.Application.Identity.Commands.RefreshCommand;
using Identity.Domain.Identity;
using Identity.Presentation.Core;
using Identity.Presentation.Extensions;
using Identity.Presentation.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Identity.Presentation.Identity.Controllers.Refresh;

[Route("refresh")]
public partial class RefreshController : ApiController
{
    public RefreshController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        string? refreshToken = Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return InvalidRefreshToken();
        }

        RefreshCommand command = new(refreshToken);

        OneOf<TokenPair, InvalidToken> result = await sender.Send(command, cancellationToken);

        return result.Match(
            handleSuccess,
            handleInvalidToken
        );
    }

    private OkObjectResult handleSuccess(TokenPair tokenPair)
    {
        Response.Cookies.AddRefreshToken(tokenPair.RefreshToken);

        TokenDto dto = new(tokenPair.AccessToken);

        return Ok(dto);
    }

    private IActionResult handleInvalidToken(InvalidToken invalidToken)
    {
        return InvalidRefreshToken();
    }
}