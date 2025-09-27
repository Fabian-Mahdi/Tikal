using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using OneOf;
using Tikal.Application.Core.Errors;

namespace Tikal.Application.Core.Pipelines;

public class ValidationPipeline<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        ValidationResult validationResult = ValidateAsync(request);

        if (validationResult.IsValid)
        {
            return await next(cancellationToken);
        }

        ValidationFailed validationFailed = new()
        {
            Errors = validationResult.Errors.Select(error => new ValidationError
            {
                ErrorMessage = error.ErrorMessage,
                PropertyName = error.PropertyName
            }).ToList()
        };

        return CreateValidationFailedResponse(validationFailed);
    }

    private ValidationResult ValidateAsync(TRequest request)
    {
        if (!validators.Any())
        {
            return new ValidationResult();
        }

        IEnumerable<ValidationFailure> errors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .Distinct();

        return new ValidationResult(errors);
    }

    private static TResponse CreateValidationFailedResponse(ValidationFailed validationFailed)
    {
        Type responseType = typeof(TResponse);

        MethodInfo? implicitConversion = responseType.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(method =>
                method.Name == "op_Implicit" &&
                method.ReturnType == responseType &&
                method.GetParameters().Length == 1 &&
                method.GetParameters()[0].ParameterType == typeof(ValidationFailed)
            );

        if (implicitConversion is null)
        {
            throw new InvalidOperationException(
                $"No implicit conversion exists between {responseType.Name} and {nameof(ValidationFailed)}"
            );
        }

        return (TResponse)implicitConversion.Invoke(null, [validationFailed])!;
    }
}