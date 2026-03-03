using Microsoft.AspNetCore.Http;

namespace BSourceCore.API.Contracts.Responses;

public record PagedResponse<T>
{
    public IEnumerable<T> Results { get; init; } = [];
    public int Total { get; init; }

    public static ApiResponse<T> From(T item) =>
        new() { Results = [item], Total = 1 };

    public static ApiResponse<T> From(IEnumerable<T> items)
    {
        if (items is ICollection<T> collection)
            return new() { Results = collection, Total = collection.Count };

        var list = items.ToList();
        return new() { Results = list, Total = list.Count };
    }
}

public record ApiErrorResponse
{
    public string Type { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public int Status { get; init; }
    public string? Detail { get; init; }
    public IEnumerable<string>? Errors { get; init; }

    public static ApiErrorResponse NotFound(string detail) => new()
    {
        Type = "not_found",
        Title = "Not Found",
        Status = StatusCodes.Status404NotFound,
        Detail = detail
    };

    public static ApiErrorResponse Validation(IEnumerable<string> errors) => new()
    {
        Type = "validation_error",
        Title = "Validation Error",
        Status = StatusCodes.Status400BadRequest,
        Errors = errors
    };

    public static ApiErrorResponse BadRequest(string detail) => new()
    {
        Type = "bad_request",
        Title = "Bad Request",
        Status = StatusCodes.Status400BadRequest,
        Detail = detail
    };

    public static ApiErrorResponse Unauthorized(string detail) => new()
    {
        Type = "unauthorized",
        Title = "Unauthorized",
        Status = StatusCodes.Status401Unauthorized,
        Detail = detail
    };

    public static ApiErrorResponse InternalError() => new()
    {
        Type = "internal_error",
        Title = "Internal Server Error",
        Status = StatusCodes.Status500InternalServerError,
        Detail = "An unexpected error occurred. Please try again later."
    };
}
