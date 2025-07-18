using System.Net;

namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    public string Title { get; }

    public HttpStatusCode Status { get; }

    public IEnumerable<ProblemError> Errors { get; }

    public ProblemException(string title, HttpStatusCode status, IEnumerable<ProblemError> errors)
    {
        Title = title;
        Status = status;
        Errors = errors;
    }
}