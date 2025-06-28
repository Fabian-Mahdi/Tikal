namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    public string Title { get; }
    public int Status { get; }
    public IEnumerable<ProblemError> Errors { get; }

    public ProblemException(string title, int status, IEnumerable<ProblemError> errors)
    {
        Title = title;
        Status = status;
        Errors = errors;
    }
}