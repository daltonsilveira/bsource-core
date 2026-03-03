using BSourceCore.Application.Abstractions;
using BSourceCore.Application.Abstractions.Repositories;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Application.Features.Auth.DTOs;
using BSourceCore.Shared.Abstractions;
using MediatR;

namespace BSourceCore.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenDto>
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public RefreshTokenCommandHandler(
        ITokenService tokenService,
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<TokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await _tokenService.RefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (tokenResult is null)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token");
        }

        // Note: In a real implementation, you would decode the token to get the user info
        // For now, we return a basic response
        return new TokenDto(
            tokenResult.AccessToken,
            tokenResult.RefreshToken,
            tokenResult.ExpiresAt,
            Guid.Empty,
            string.Empty,
            string.Empty);
    }
}
