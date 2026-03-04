using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Features.Tenants.DTOs;
using BSourceCore.Application.Features.Tenants.Queries.GetTenantById;
using BSourceCore.Domain.Entities;
using BSourceCore.Shared.Abstractions;
using BSourceCore.Shared.Kernel.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<TenantDto>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateTenantCommandHandler> _logger;

    public CreateTenantCommandHandler(
        ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IMediator mediator,
        ILogger<CreateTenantCommandHandler> logger)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<TenantDto>> Handle(
        CreateTenantCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating tenant with name: {Name}", request.Name);

        var existingTenant = await _tenantRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingTenant is not null)
        {
            _logger.LogWarning("Tenant with slug {Slug} already exists", request.Slug);
            return Result<TenantDto>.Fail(new Error(
                "Tenant.SlugConflict",
                $"Tenant with slug '{request.Slug}' already exists",
                ErrorType.Conflict));
        }

        var tenant = new Tenant(request.Name, request.Slug, request.Description);

        await _tenantRepository.AddAsync(tenant, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tenant created with Id: {TenantId}", tenant.TenantId);

        var result = await _mediator.Send(new GetTenantByIdQuery(tenant.TenantId), cancellationToken);

        return Result<TenantDto>.Success(result.Value!);
    }
}
