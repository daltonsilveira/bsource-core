using BSourceCore.Application.Features.Users.DTOs;

namespace BSourceCore.API.Contracts.Responses;

public class UserAuditResponse
{
    public Guid UserId { get; }
    public string Name { get; }

    public UserAuditResponse(UserAuditDto dto)
    {
        UserId = dto.UserId;
        Name = dto.Name;
    }
}
