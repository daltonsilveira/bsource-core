using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.DeleteGroup;

public record DeleteGroupCommand(Guid GroupId) : IRequest<bool>;
