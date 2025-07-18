using System.Net;
using IdentityAPI.ErrorHandling;

namespace IdentityAPI.Controllers.Register.Errors;

public class RegistrationFailed : ProblemException
{
    public RegistrationFailed(IEnumerable<string> errors)
        : base(
            "User registration failed",
            HttpStatusCode.BadRequest,
            errors.Select(error => new ProblemError { Detail = error })
        )
    {
    }
}