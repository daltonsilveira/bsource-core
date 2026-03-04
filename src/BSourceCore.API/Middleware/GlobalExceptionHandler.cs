using BSourceCore.API.Contracts.Responses;
using BSourceCore.Shared.Kernel.Results;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace BSourceCore.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var (statusCode, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                validationException.Errors.Select(e =>
                    new Error("Validation", e.ErrorMessage, ErrorType.Validation))),

            InvalidOperationException => (
                StatusCodes.Status400BadRequest,
                new[] { new Error("BadRequest", exception.Message, ErrorType.BusinessRule) }.AsEnumerable()),

            KeyNotFoundException => (
                StatusCodes.Status404NotFound,
                new[] { new Error("NotFound", exception.Message, ErrorType.NotFound) }.AsEnumerable()),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                new[] { new Error("Unauthorized", exception.Message, ErrorType.Unauthorized) }.AsEnumerable()),

            _ => (
                StatusCodes.Status500InternalServerError,
                new[] { new Error("InternalError", "An unexpected error occurred", ErrorType.BusinessRule) }.AsEnumerable())
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(ErrorResponse.From(errors), cancellationToken);

        return true;
    }
}
