using System.Net;

namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    protected ProblemException(string title, HttpStatusCode status, IEnumerable<ProblemError> errors)
    {
        Title = title;
        Status = status;
        Errors = errors;
    }

    public string Title { get; }
    public HttpStatusCode Status { get; }
    public IEnumerable<ProblemError> Errors { get; }
}