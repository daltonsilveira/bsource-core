using BSourceCore.Application.Features.Groups.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Groups.Commands.CreateGroup;

public record CreateGroupCommand(
    Guid TenantId,
    string Name,
    string? Description
) : IRequest<Result<GroupDto>>;
