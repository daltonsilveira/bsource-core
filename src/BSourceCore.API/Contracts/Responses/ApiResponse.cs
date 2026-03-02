namespace BSourceCore.API.Contracts.Responses;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public IEnumerable<string>? Errors { get; init; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static ApiResponse<T> Fail(string error) => new()
    {
        Success = false,
        Error = error
    };

    public static ApiResponse<T> Fail(IEnumerable<string> errors) => new()
    {
        Success = false,
        Errors = errors
    };
}

public record ApiResponse
{
    public bool Success { get; init; }
    public string? Error { get; init; }
    public IEnumerable<string>? Errors { get; init; }

    public static ApiResponse Ok() => new() { Success = true };

    public static ApiResponse Fail(string error) => new()
    {
        Success = false,
        Error = error
    };

    public static ApiResponse Fail(IEnumerable<string> errors) => new()
    {
        Success = false,
        Errors = errors
    };
}
