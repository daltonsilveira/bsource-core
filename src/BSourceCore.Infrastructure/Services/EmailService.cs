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
public class EmailService : IEmailService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly BSourceNotifierOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IHttpClientFactory httpClientFactory,
        IOptions<BSourceNotifierOptions> options,
        ILogger<EmailService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmail(string toAddress, string subject, string message, object? data = null, CancellationToken cancellationToken = default)
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
                title = subject,
                message,
                channels = new[] { 1 }, // 1 = Email
                target = new
                {
                    endpoints = new
                    {
                        email = new
                        {
                            to = toAddress
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
                _logger.LogDebug("Email enviado com sucesso para {ToAddress}", toAddress);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogWarning(
                    "Falha ao enviar email. Status: {StatusCode}, Response: {Response}",
                    response.StatusCode,
                    content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar email para {ToAddress}", toAddress);
        }
    }
}
