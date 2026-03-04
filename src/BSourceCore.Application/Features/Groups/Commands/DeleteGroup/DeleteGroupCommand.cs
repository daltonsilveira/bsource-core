using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.DeleteGroup;

public record DeleteGroupCommand(Guid GroupId) : IRequest<Result>;
