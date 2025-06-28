namespace IdentityAPI.ErrorHandling;

public record ProblemError
{
    public required string Detail { get; init; }
}