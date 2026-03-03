using System.Reflection;
using BSourceCore.Shared.Kernel.Results;
using FluentValidation;
using MediatR;

namespace BSourceCore.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
        {
            var details = failures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(f => f.ErrorMessage).ToArray());

            var error = new Error(
                "Validation",
                "One or more validation errors occurred.",
                ErrorType.Validation,
                details);

            if (TryCreateFailResult(error, out var failResult))
            {
                return failResult!;
            }

            throw new ValidationException(failures);
        }

        return await next();
    }

    private static bool TryCreateFailResult(Error error, out TResponse? result)
    {
        result = default;
        var responseType = typeof(TResponse);

        if (!responseType.IsAssignableTo(typeof(Result)))
            return false;

        if (responseType == typeof(Result))
        {
            result = (TResponse)(object)Result.Fail(error);
            return true;
        }

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var failMethod = responseType.GetMethod(
                "Fail",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                null,
                [typeof(Error)],
                null);

            if (failMethod is not null)
            {
                result = (TResponse)failMethod.Invoke(null, [error])!;
                return true;
            }
        }

        return false;
    }
}
