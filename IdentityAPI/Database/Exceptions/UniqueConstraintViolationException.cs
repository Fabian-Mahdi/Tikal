using IdentityAPI.ErrorHandling;
using System.Net;

namespace IdentityAPI.Database.Exceptions;

public class UniqueConstraintViolationException : ProblemException
{
    public UniqueConstraintViolationException()
        : base("unique constraint violation", HttpStatusCode.Conflict)
    {
    }
}