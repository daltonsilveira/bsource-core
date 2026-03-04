using BSourceCore.API.Contracts.Responses;
using BSourceCore.Shared.Kernel.Results;
using Microsoft.AspNetCore.Mvc;

namespace BSourceCore.API.Extensions;

/// <summary>
/// Maps Result / Result&lt;T&gt; to IActionResult, centralizing error-to-HTTP-status mapping.
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToProblemDetails(
        this Result result,
        ControllerBase controller)
    {
        // Agrupar por tipo
        var errors = result.Errors;

        var firstType = errors.First().Type;

        if (firstType == ErrorType.Validation)
        {
            var validationErrors = errors
                .Where(e => e.Type == ErrorType.Validation)
                .SelectMany(e => e.Details ?? new Dictionary<string, string[]>())
                .GroupBy(x => x.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.SelectMany(x => x.Value).ToArray()
                );

            return controller.BadRequest(new ValidationProblemDetails(validationErrors)
            {
                Title = "Validation Failed"
            });
        }

        // Para outros tipos, agregue mensagens
        var problems = ErrorResponse.From(errors);

        return firstType switch
        {
            ErrorType.BadRequest => controller.BadRequest(problems),
            ErrorType.NotFound => controller.NotFound(problems),
            ErrorType.Conflict => controller.Conflict(problems),
            ErrorType.Forbidden => controller.StatusCode(403, problems),
            ErrorType.Unauthorized => controller.Unauthorized(problems),
            ErrorType.BusinessRule => controller.UnprocessableEntity(problems),
            _ => controller.StatusCode(500, problems)
        };
    }
}
