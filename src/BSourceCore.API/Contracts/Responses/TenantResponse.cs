using BSourceCore.Application.Features.Tenants.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class TenantResponse
{
    public Guid TenantId { get; }
    public string Name { get; }
    public string Slug { get; }
    public string Description { get; }
    public string Status { get; }
    public DateTimeOffset CreatedAt { get; }
    public UserAuditResponse? CreatedBy { get; }
    public DateTimeOffset? UpdatedAt { get; }
    public UserAuditResponse? UpdatedBy { get; }

    public TenantResponse(TenantDto dto)
    {
        TenantId = dto.TenantId;
        Name = dto.Name;
        Slug = dto.Slug;
        Description = dto.Description;
        Status = dto.Status.ToString();
        CreatedAt = dto.CreatedAt;
        CreatedBy = dto.CreatedBy != null ? new UserAuditResponse(dto.CreatedBy) : null;
        UpdatedAt = dto.UpdatedAt;
        UpdatedBy = dto.UpdatedBy != null ? new UserAuditResponse(dto.UpdatedBy) : null;
    }
}
