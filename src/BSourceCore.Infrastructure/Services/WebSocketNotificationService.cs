using System.Net.Http.Json;
using System.Text.Json;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Infrastructure.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BSourceCore.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de notificações BSourceNotifier
/// </summary>
public class WebSocketNotificationService : IWebSocketNotificationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly BSourceNotifierOptions _options;
    private readonly ILogger<WebSocketNotificationService> _logger;

    public WebSocketNotificationService(
        IHttpClientFactory httpClientFactory,
        IOptions<BSourceNotifierOptions> options,
        ILogger<WebSocketNotificationService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendNotification(string title, string message, Guid userId, object? data = null, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogDebug("BSourceNotifier está desabilitado. Notificação não enviada.");
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            _logger.LogWarning("BSourceNotifier BaseUrl não configurada. Notificação não enviada.");
            return;
        }

        try
        {
            var payload = new
            {
                title,
                message,
                channels = new[] { 0 }, // 0 = WebSocket
                target = new
                {
                    userId,
                    endpoints = new
                    {
                        webSocket = new
                        {
                            group = (string)null
                        }
                    },
                    data
                }
            };

            var client = _httpClientFactory.CreateClient("BSourceNotifier");

            var response = await client.PostAsJsonAsync(
                "/api/notifications/send",
                payload,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase },
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogDebug("Notificação WebSocket enviada com sucesso para usuário {UserId}", userId);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "Falha ao enviar notificação WebSocket. Status: {StatusCode}, Response: {Response}",
                    response.StatusCode,
                    content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar notificação WebSocket para usuário {UserId}", userId);
            // Não propaga a exceção para não interromper o fluxo principal
        }
    }
}
