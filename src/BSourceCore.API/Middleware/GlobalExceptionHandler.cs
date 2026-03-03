using BSourceCore.API.Contracts.Responses;
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

        ApiErrorResponse errorResponse = exception switch
        {
            ValidationException validationException =>
                ApiErrorResponse.Validation(
                    validationException.Errors.Select(e => e.ErrorMessage)),

            InvalidOperationException =>
                ApiErrorResponse.BadRequest(exception.Message),

            KeyNotFoundException =>
                ApiErrorResponse.NotFound(exception.Message),

            UnauthorizedAccessException =>
                ApiErrorResponse.Unauthorized(exception.Message),

            _ => ApiErrorResponse.InternalError()
        };

        httpContext.Response.StatusCode = errorResponse.Status;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }
}
