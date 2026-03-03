using BSourceCore.Shared.Kernel.Results;

namespace BSourceCore.API.Contracts.Responses;

public record ErrorResponse
{
    public List<ErrorItem> Errors { get; set; } = new();

    public static ErrorResponse From(IEnumerable<Error> errors)
    {
        var errorItems = errors.Select(e => new ErrorItem(e.Code, e.Message)).ToList();
        return new ErrorResponse { Errors = errorItems };
    }
}

public sealed record ErrorItem(string Code, string Message);