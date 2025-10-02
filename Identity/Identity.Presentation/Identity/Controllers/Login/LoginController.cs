using Identity.Application.Core.Errors;
using Identity.Application.Identity.Commands.LoginCommand;
using Identity.Domain.Identity;
using Identity.Presentation.Core;
using Identity.Presentation.Extensions;
using Identity.Presentation.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Identity.Presentation.Identity.Controllers.Login;

[Route("login")]
public partial class LoginController : ApiController
{
    public LoginController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        LoginCommand command = new(dto.username, dto.password);

        OneOf<TokenPair, ValidationFailed, InvalidCredentials> result = await sender.Send(command, cancellationToken);

        return result.Match(
            handleSuccess,
            handleValidationFailed,
            handleInvalidCredentials
        );
    }

    private OkObjectResult handleSuccess(TokenPair tokenPair)
    {
        Response.Cookies.AddRefreshToken(tokenPair.RefreshToken);

        TokenDto dto = new(tokenPair.AccessToken);

        return Ok(dto);
    }

    private IActionResult handleInvalidCredentials(InvalidCredentials invalidCredentials)
    {
        return InvalidCredentials();
    }
}