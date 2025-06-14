using IdentityAPI.ErrorHandling;
using System.Net;

namespace IdentityAPI.Services.UserService.Exceptions;

public class InvalidCredentialsException : ProblemException
{
    public InvalidCredentialsException()
        : base("Invalid Credentials", HttpStatusCode.Unauthorized)
    {
    }
}