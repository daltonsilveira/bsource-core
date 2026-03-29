using BSourceCore.Application.Features.Users.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class UserResponse
{
    public Guid UserId { get; }
    public string Name { get; }
    public string Email { get; }
    public string Status { get; }
    public DateTimeOffset CreatedAt { get; }
    public UserAuditResponse? CreatedBy { get; }
    public DateTimeOffset? UpdatedAt { get; }
    public UserAuditResponse? UpdatedBy { get; }

    public UserResponse(UserDto dto)
    {
        UserId = dto.UserId;
        Name = dto.Name;
        Email = dto.Email;
        Status = dto.Status.ToString();
        CreatedAt = dto.CreatedAt;
        CreatedBy = dto.CreatedBy != null ? new UserAuditResponse(dto.CreatedBy) : null;
        UpdatedAt = dto.UpdatedAt;
        UpdatedBy = dto.UpdatedBy != null ? new UserAuditResponse(dto.UpdatedBy) : null;
    }
}


