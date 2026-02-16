namespace BSourceCore.Application.Abstractions.Services;

public interface IEmailService
{
    Task SendEmail(string toAddress, string subject, string htmlMessage);
    Task SendEmailWithData(string toAddress, string subject, string templateMessage, object data);
}
