using IdentityAPI.ErrorHandling;
using System.Net;

namespace IdentityAPI.Controllers.Login.Errors;

public class InvalidCredentialsException : ProblemException
{
    public InvalidCredentialsException()
        : base
        (
            "Invalid username or password provided",
            HttpStatusCode.Unauthorized,
            []
        )
    {
    }
}