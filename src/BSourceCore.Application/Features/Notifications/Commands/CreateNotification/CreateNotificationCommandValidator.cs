using FluentValidation;

namespace BSourceCore.Application.Features.Notifications.Commands.CreateNotification;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(8000).WithMessage("Message must not exceed 8000 characters");

        RuleFor(x => x.Data)
            .MaximumLength(8000).WithMessage("Data must not exceed 8000 characters")
            .When(x => x.Data is not null);

        RuleFor(x => x.Types)
            .NotEmpty().WithMessage("At least one notification type is required");

        RuleFor(x => x)
            .Must(x => (x.UserIds is not null && x.UserIds.Count > 0) || (x.GroupIds is not null && x.GroupIds.Count > 0))
            .WithMessage("At least one UserId or GroupId is required");
    }
}
