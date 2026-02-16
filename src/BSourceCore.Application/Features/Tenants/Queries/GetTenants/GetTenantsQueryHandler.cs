using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Queries.GetTenants;

public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, IEnumerable<TenantDto>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserContext _userContext;
    private readonly ILogger<GetTenantsQueryHandler> _logger;

    public GetTenantsQueryHandler(
        ITenantRepository tenantRepository,
        IUserContext userContext,
        ILogger<GetTenantsQueryHandler> logger)
    {
        _tenantRepository = tenantRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<IEnumerable<TenantDto>> Handle(
        GetTenantsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tenants");

        var tenants = await _tenantRepository.GetAllAsync(cancellationToken);

        return tenants.Select(t => new TenantDto(
            t.TenantId,
            t.Name,
            t.Slug,
            t.Description,
            t.Status.ToString(),
            t.CreatedAt));
    }
}
