using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;
