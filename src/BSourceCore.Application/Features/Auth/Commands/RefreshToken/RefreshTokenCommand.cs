using BSourceCore.Application.Features.Auth.DTOs;
using MediatR;

namespace BSourceCore.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<TokenDto>;
