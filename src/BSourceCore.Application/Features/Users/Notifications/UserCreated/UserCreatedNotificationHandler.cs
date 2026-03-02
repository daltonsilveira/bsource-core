using BSourceCore.Application.Abstractions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BSourceCore.Application.Features.Users.Notifications.UserCreated;

public class UserCreatedNotificationHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<UserCreatedNotificationHandler> _logger;

    public UserCreatedNotificationHandler(
        IEmailService emailService,
        ILogger<UserCreatedNotificationHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Sending welcome email to user {UserId} at {Email}",
            notification.UserId,
            notification.Email);

        var templateData = new WelcomeEmailModel
        {
            UserName = notification.Name,
            Email = notification.Email,
            Password = notification.Password
        };

        var template = GetWelcomeEmailTemplate();

        await _emailService.SendEmail(
            notification.Email,
            "Bem-vindo ao Sistema - Suas Credenciais de Acesso",
            template,
            templateData);

        _logger.LogInformation(
            "Welcome email sent successfully to user {UserId}",
            notification.UserId);
    }

    private static string GetWelcomeEmailTemplate()
    {
        return """
            <!DOCTYPE html>
            <html lang="pt-BR">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Bem-vindo</title>
                <style>
                    body {
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        background-color: #f4f4f4;
                    }
                    .container {
                        background-color: #ffffff;
                        border-radius: 8px;
                        padding: 30px;
                        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    }
                    .header {
                        text-align: center;
                        border-bottom: 2px solid #007bff;
                        padding-bottom: 20px;
                        margin-bottom: 20px;
                    }
                    .header h1 {
                        color: #007bff;
                        margin: 0;
                    }
                    .content {
                        padding: 20px 0;
                    }
                    .credentials {
                        background-color: #f8f9fa;
                        border-left: 4px solid #007bff;
                        padding: 15px;
                        margin: 20px 0;
                        border-radius: 0 4px 4px 0;
                    }
                    .credentials p {
                        margin: 8px 0;
                    }
                    .credentials strong {
                        color: #495057;
                    }
                    .warning {
                        background-color: #fff3cd;
                        border: 1px solid #ffc107;
                        border-radius: 4px;
                        padding: 15px;
                        margin: 20px 0;
                    }
                    .warning p {
                        margin: 0;
                        color: #856404;
                    }
                    .footer {
                        text-align: center;
                        padding-top: 20px;
                        border-top: 1px solid #dee2e6;
                        color: #6c757d;
                        font-size: 12px;
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <div class="header">
                        <h1>Bem-vindo!</h1>
                    </div>
                    <div class="content">
                        <p>Olá, <strong>@Model.UserName</strong>!</p>
                        <p>Sua conta foi criada com sucesso. Abaixo estão suas credenciais de acesso:</p>
                        
                        <div class="credentials">
                            <p><strong>E-mail:</strong> @Model.Email</p>
                            <p><strong>Senha:</strong> @Model.Password</p>
                        </div>
                        
                        <div class="warning">
                            <p><strong>⚠️ Importante:</strong> Por motivos de segurança, você será solicitado a alterar sua senha no primeiro acesso ao sistema.</p>
                        </div>
                        
                        <p>Se você tiver alguma dúvida ou precisar de assistência, entre em contato com nosso suporte.</p>
                    </div>
                    <div class="footer">
                        <p>Este é um e-mail automático. Por favor, não responda.</p>
                    </div>
                </div>
            </body>
            </html>
            """;
    }
}

public class WelcomeEmailModel
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
