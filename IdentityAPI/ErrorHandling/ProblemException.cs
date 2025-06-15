using System.Net;

namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    public HttpStatusCode Status { get; }

    public ProblemException(HttpStatusCode status)
    {
        Status = status;
    }
}