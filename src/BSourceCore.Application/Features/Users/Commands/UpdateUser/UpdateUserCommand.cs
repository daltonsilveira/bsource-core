using MediatR;

namespace BSourceCore.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string Name,
    string Email
) : IRequest<UpdateUserResult>;

public record UpdateUserResult(
    Guid UserId,
    string Name,
    string Email);
