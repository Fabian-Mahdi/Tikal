using System.Net;
using IdentityAPI.ErrorHandling;

namespace IdentityAPI.Controllers.Refresh.Errors;

public class InvalidRefreshTokenException : ProblemException
{
    public InvalidRefreshTokenException()
        : base(
            "Invalid refresh token",
            HttpStatusCode.Unauthorized,
            []
        )
    {
    }
}