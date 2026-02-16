using BSourceCore.Application.Abstractions;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, CreateTenantResult>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly ILogger<CreateTenantCommandHandler> _logger;

    public CreateTenantCommandHandler(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        ILogger<CreateTenantCommandHandler> logger)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<CreateTenantResult> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating tenant with name: {Name}", request.Name);

        var existingTenant = await _tenantRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingTenant is not null)
        {
            _logger.LogWarning("Tenant with slug {Slug} already exists", request.Slug);
            throw new InvalidOperationException($"Tenant with slug '{request.Slug}' already exists");
        }

        var tenant = new Tenant(request.Name, request.Slug, request.Description);

        await _tenantRepository.AddAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tenant created with Id: {TenantId}", tenant.TenantId);

        return new CreateTenantResult(tenant.TenantId, tenant.Name, tenant.Slug);
    }
}
