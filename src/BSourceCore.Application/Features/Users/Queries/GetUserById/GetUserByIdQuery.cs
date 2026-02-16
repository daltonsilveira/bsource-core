using BSourceCore.Application.Features.Users.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;
