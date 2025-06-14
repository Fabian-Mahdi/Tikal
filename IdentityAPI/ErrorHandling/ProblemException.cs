using System.Net;

namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    public string Error { get; }
    public HttpStatusCode Status { get; }

    public ProblemException(string error, HttpStatusCode status) : base(error)
    {
        Error = error;
        Status = status;
    }
}