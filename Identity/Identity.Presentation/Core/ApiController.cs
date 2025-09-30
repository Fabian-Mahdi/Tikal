using Identity.Application.Core.Errors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Core;

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
    ///     Validation of the current command or query has failed
    /// </summary>
    /// <param name="validationFailed">
    ///     The <see cref="ValidationFailed" /> which contains the errors responsible for the
    ///     failure
    /// </param>
    /// <returns>StatusCode: 400</returns>
    protected IActionResult handleValidationFailed(ValidationFailed validationFailed)
    {
        Dictionary<string, IEnumerable<string>> errors = validationFailed.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage)
            );

        return Problem(
            title: "One or more validation errors occurred.",
            statusCode: StatusCodes.Status400BadRequest,
            extensions: new Dictionary<string, object?>
            {
                {
                    "errors", errors
                }
            }
        );
    }
}