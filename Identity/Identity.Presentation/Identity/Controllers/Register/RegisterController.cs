using Identity.Application.Core.Errors;
using Identity.Application.Identity.Commands.RegisterCommand;
using Identity.Domain.Identity;
using Identity.Presentation.Core;
using Identity.Presentation.Identity.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Identity.Presentation.Identity.Controllers.Register;

[Route("register")]
public partial class RegisterController : ApiController
{
    public RegisterController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
    {
        RegisterCommand command = new(dto.username, dto.password);

        OneOf<User, ValidationFailed, DuplicateUsername> result = await sender.Send(command, cancellationToken);

        return result.Match(
            handleSuccess,
            handleValidationFailed,
            handleDuplicateUsername
        );
    }

    private OkObjectResult handleSuccess(User user)
    {
        UserDto dto = new(user.Id, user.Username);

        return Ok(dto);
    }

    private IActionResult handleDuplicateUsername(DuplicateUsername duplicateUsername)
    {
        return UsernameExists(duplicateUsername.Username);
    }
}