namespace BSourceCore.Application.Abstractions.Services;

public interface IEmailService
{
    /// <summary>
    /// Envia um email para um endereço específico com assunto, mensagem e dados adicionais
    /// </summary>
    /// <param name="toAddress"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendEmail(string toAddress, string subject, string message, object? data = null, CancellationToken cancellationToken = default);
}
