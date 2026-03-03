using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    Guid TenantId,
    string Name,
    string Email
) : IRequest<Result<UserDto>>;
