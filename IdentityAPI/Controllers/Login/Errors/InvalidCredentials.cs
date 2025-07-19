using System.Net;
using IdentityAPI.ErrorHandling;

namespace IdentityAPI.Controllers.Login.Errors;

public class InvalidCredentials : ProblemException
{
    public InvalidCredentials()
        : base(
            "Invalid username or password provided",
            HttpStatusCode.Unauthorized,
            []
        )
    {
    }
}