using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Application.Features.Users.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenants;

public class ListTenantsQueryHandler : IRequestHandler<ListTenantsQuery, IEnumerable<TenantDto>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<ListTenantsQueryHandler> _logger;

    public ListTenantsQueryHandler(
        ITenantRepository tenantRepository,
        IUserContext userContext,
        ILogger<ListTenantsQueryHandler> logger)
    {
        _tenantRepository = tenantRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<IEnumerable<TenantDto>> Handle(
        ListTenantsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tenants");

        var tenants = await _tenantRepository.GetAllAsync(cancellationToken);

        return tenants.Select(t => new TenantDto(
            t.TenantId,
            t.Name,
            t.Slug,
            t.Description,
            t.Status,
            t.CreatedAt,
            t.CreatedBy != null ? new UserAuditDto(t.CreatedBy.UserId, t.CreatedBy.Name) : null,
            t.UpdatedAt,
            t.UpdatedBy != null ? new UserAuditDto(t.UpdatedBy.UserId, t.UpdatedBy.Name) : null));
    }
}
