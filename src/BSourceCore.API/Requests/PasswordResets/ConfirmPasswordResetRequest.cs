namespace BSourceCore.API.Requests.PasswordResets;

public record ConfirmPasswordResetRequest(
    string Token,
    string Password);
