using IdentityAPI.ErrorHandling;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace IdentityAPI.Controllers.Register.Errors;

public class RoleAssignmentFailedException : ProblemException
{
    public RoleAssignmentFailedException(IEnumerable<IdentityError> errors)
        : base
        (
            "Failed to assign role to created user",
            HttpStatusCode.InternalServerError,
            [.. errors.Select(error => new ProblemError() { Detail = error.Description })]
        )
    {
    }
}