using MediatR;

namespace BSourceCore.Application.Features.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    string Name,
    string Slug,
    string? Description
) : IRequest<CreateTenantResult>;
