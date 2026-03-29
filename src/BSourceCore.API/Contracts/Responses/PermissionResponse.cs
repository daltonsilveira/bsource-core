using BSourceCore.Application.Features.Permissions.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class PermissionResponse
{
    public Guid PermissionId { get; }
    public string Code { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Status { get; }
    public DateTimeOffset CreatedAt { get; }

    public PermissionResponse(PermissionDto dto)
    {
        PermissionId = dto.PermissionId;
        Code = dto.Code;
        Name = dto.Name;
        Description = dto.Description;
        Status = dto.Status;
        CreatedAt = dto.CreatedAt;
    }
}
