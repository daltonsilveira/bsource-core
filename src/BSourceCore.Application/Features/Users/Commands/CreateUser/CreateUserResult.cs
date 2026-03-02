namespace BSourceCore.Application.Features.Users.Commands.CreateUser;

public record CreateUserResult(
    Guid UserId,
    string Name,
    string Email);
