namespace IdentityAPI.ErrorHandling;

public class ProblemException : Exception
{
    public string Error { get; }
    public override string Message { get; }

    public ProblemException(string error, string message) : base(message)
    {
        Error = error;
        Message = message;
    }
}