using FluentValidation;

namespace BSourceCore.Application.Features.PasswordResets.Commands.ConfirmPasswordReset;

public class ConfirmPasswordResetCommandValidator : AbstractValidator<ConfirmPasswordResetCommand>
{
    public ConfirmPasswordResetCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long");
    }
}
