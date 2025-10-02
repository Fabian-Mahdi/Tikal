using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Identity.Controllers.Refresh;

public partial class RefreshController
{
    private ObjectResult InvalidRefreshToken()
    {
        return Problem(
            title: "Invalid refresh token",
            statusCode: StatusCodes.Status401Unauthorized
        );
    }
}