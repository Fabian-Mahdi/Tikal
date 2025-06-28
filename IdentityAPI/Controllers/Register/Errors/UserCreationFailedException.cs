using IdentityAPI.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Controllers.Register.Errors;

public class UserCreationFailedException : ProblemException
{
    public UserCreationFailedException(IEnumerable<IdentityError> errors)
        : base
        (
            "User registration failed",
            400,
            [.. errors.Select(error => new ProblemError() { Detail = error.Description })]
        )
    {
    }
}