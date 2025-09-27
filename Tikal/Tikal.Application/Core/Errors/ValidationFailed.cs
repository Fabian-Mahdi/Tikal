using FluentValidation.Results;

namespace Tikal.Application.Core.Errors;

/// <summary>
///     Indicates that validation has failed for various reasons
/// </summary>
public class ValidationFailed
{
    /// <summary>
    ///     The list of <see cref="ValidationFailure" /> which resulted in a failed validation
    /// </summary>
    public List<ValidationFailure> Errors { get; set; } = [];
}