using BackendAuthTemplate.Application.Common.Result;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BackendAuthTemplate.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default)
        {
            if (validators.Any())
            {
                ValidationContext<TRequest> context = new(request);

                ValidationResult[] validationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                List<ValidationFailure> failures = validationResults
                    .Where(r => r.Errors.Count != 0)
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Count != 0)
                {
                    Dictionary<string, string[]> details = failures
                        .GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key.ToLower(),
                            g => g.Select(f => f.ErrorCode).ToArray()
                        );

                    Error error = new(
                        $"{ErrorType.Validation}",
                        "Validation failed.",
                        ErrorType.Validation,
                        details
                    );

                    Type t = typeof(TResponse);

                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Result<>))
                    {
                        Type inner = t.GetGenericArguments()[0];
                        Type generic = typeof(Result<>).MakeGenericType(inner);

                        return (TResponse)generic
                            .GetMethod(nameof(Result<object>.Failure))!
                            .Invoke(null, [error])!;
                    }

                    if (t == typeof(Result.Result))
                    {
                        return (TResponse)(object)Result.Result.Failure(error);
                    }

                    throw new ValidationException(failures);
                }
            }

            return await next(cancellationToken);
        }
    }
}
