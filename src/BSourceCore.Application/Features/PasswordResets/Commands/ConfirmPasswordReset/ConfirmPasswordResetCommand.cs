using MediatR;

namespace BSourceCore.Application.Features.PasswordResets.Commands.ConfirmPasswordReset;

public record ConfirmPasswordResetCommand(
    string Token,
    string Password
) : IRequest<ConfirmPasswordResetResult>;
