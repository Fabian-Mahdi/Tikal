using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tikal.Presentation.Core;

/// <summary>
///     The base class for all api controllers
/// </summary>
[ApiController]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    ///     Used to send commands and queries
    /// </summary>
    protected readonly ISender sender;

    protected ApiController(ISender sender)
    {
        this.sender = sender;
    }

    /// <summary>
    ///     Gets the Id of the currently authenticated user
    /// </summary>
    /// <returns>The id of the currently authenticated user</returns>
    protected string GetCurrentUserId()
    {
        return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}