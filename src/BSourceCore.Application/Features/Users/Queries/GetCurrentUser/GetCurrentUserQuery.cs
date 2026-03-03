using MediatR;

namespace BSourceCore.Application.Features.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<CurrentUserDto?>;
