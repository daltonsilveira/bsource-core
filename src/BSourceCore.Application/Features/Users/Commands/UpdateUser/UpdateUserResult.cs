namespace BSourceCore.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserResult(
    Guid UserId,
    string Name,
    string Email);
