using BSourceCore.Application.Features.Groups.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class GroupResponse
{
    public Guid GroupId { get; }
    public Guid TenantId { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Status { get; }
    public DateTimeOffset CreatedAt { get; }
    public UserAuditResponse? CreatedBy { get; }
    public DateTimeOffset? UpdatedAt { get; }
    public UserAuditResponse? UpdatedBy { get; }

    public GroupResponse(GroupDto dto)
    {
        GroupId = dto.GroupId;
        TenantId = dto.TenantId;
        Name = dto.Name;
        Description = dto.Description;
        Status = dto.Status.ToString();
        CreatedAt = dto.CreatedAt;
        CreatedBy = dto.CreatedBy != null ? new UserAuditResponse(dto.CreatedBy) : null;
        UpdatedAt = dto.UpdatedAt;
        UpdatedBy = dto.UpdatedBy != null ? new UserAuditResponse(dto.UpdatedBy) : null;
    }
}
