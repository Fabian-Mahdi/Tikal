using IdentityAPI.ErrorHandling;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Controllers.Register.Errors;

public class RoleAssignmentFailedException : ProblemException
{
    public RoleAssignmentFailedException(IEnumerable<IdentityError> errors)
        : base
        (
            "Failed to assign role to created user",
            500,
            [.. errors.Select(error => new ProblemError() { Detail = error.Description })]
        )
    {
    }
}