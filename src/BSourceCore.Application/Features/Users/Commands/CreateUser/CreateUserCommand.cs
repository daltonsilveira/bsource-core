using MediatR;

namespace BSourceCore.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    Guid TenantId,
    string Name,
    string Email
) : IRequest<CreateUserResult>;
