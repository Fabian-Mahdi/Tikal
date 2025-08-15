using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Identity.Presentation.Controllers.Refresh;

/// <summary>
///     Contains all error definitions for the refresh endpoint
/// </summary>
public partial class RefreshController
{
    /// <summary>
    ///     The provided refresh token is invalid
    /// </summary>
    /// <returns>StatusCode: 401</returns>
    private ObjectResult InvalidRefreshToken()
    {
        return Problem(
            title: "Invalid refresh token",
            statusCode: StatusCodes.Status401Unauthorized
        );
    }
}