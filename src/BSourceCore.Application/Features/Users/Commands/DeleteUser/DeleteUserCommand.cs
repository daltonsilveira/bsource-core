using MediatR;

namespace BSourceCore.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;
