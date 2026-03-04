using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string Slug,
    string? Description
) : IRequest<Result<TenantDto>>;
