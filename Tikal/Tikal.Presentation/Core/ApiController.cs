using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tikal.Presentation.Core;

[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ISender sender;

    public ApiController(ISender sender)
    {
        this.sender = sender;
    }

    protected string GetCurrentUserId()
    {
        return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}