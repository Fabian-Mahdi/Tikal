using System.Net;
using IdentityAPI.ErrorHandling;

namespace IdentityAPI.Controllers.Login.Errors;

public class InvalidCredentialsException : ProblemException
{
    public InvalidCredentialsException()
        : base(
            "Invalid username or password provided",
            HttpStatusCode.Unauthorized,
            []
        )
    {
    }
}