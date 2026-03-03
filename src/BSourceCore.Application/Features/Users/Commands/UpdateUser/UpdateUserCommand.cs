using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string Name,
    string Email
) : IRequest<Result<UserDto>>;
