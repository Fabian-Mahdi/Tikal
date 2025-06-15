using IdentityAPI.ErrorHandling;
using System.Net;

namespace IdentityAPI.Services.UserService.Exceptions;

public class DuplicateUsernameException : ProblemException
{
    public DuplicateUsernameException() : base(HttpStatusCode.Conflict)
    {
    }
}