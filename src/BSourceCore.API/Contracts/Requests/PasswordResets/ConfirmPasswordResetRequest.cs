namespace BSourceCore.API.Contracts.Requests.PasswordResets;

public record ConfirmPasswordResetRequest(
    string Token,
    string Password);
