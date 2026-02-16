using FluentValidation;

namespace BSourceCore.Application.Features.Permissions.Commands.CreatePermission;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(100).WithMessage("Code must not exceed 100 characters")
            .Matches(@"^[a-z]+\.[a-z]+$").WithMessage("Code must be in format 'module.action' (e.g., users.create)");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }
}
