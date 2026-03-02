namespace BSourceCore.Application.Models.Results;

public record TokenResult(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt);
