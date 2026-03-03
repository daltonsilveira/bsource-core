using BSourceCore.Application.Features.Auth.DTOs;
using BSourceCore.Shared.Kernel.Results;
using MediatR;

namespace BSourceCore.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password,
    Guid? TenantId = null) : IRequest<Result<TokenDto>>;
