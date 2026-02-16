using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using BSourceCore.Application.Abstractions.Services;
using BSourceCore.Infrastructure.Options;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Razor;
using FluentEmail.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentEmailCore = FluentEmail.Core.Email;

namespace BSourceCore.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions, ILogger<EmailService> logger)
    {
        _emailOptions = emailOptions.Value;
        _logger = logger;
    }

    public async Task SendEmail(string toAddress, string subject, string htmlMessage)
    {
        MailMessage message = new MailMessage
        {
            From = new MailAddress(string.IsNullOrWhiteSpace(_emailOptions.From) ? _emailOptions.UserName : _emailOptions.From, _emailOptions.DisplayName),
            Sender = new MailAddress(_emailOptions.UserName),
            Subject = subject,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(toAddress));

        if (_emailOptions.BccShare is not null)
        {
            foreach (string item in _emailOptions.BccShare)
            {
                message.Bcc.Add(new MailAddress(item));
            }
        }

        AlternateView altView = AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html");
        message.AlternateViews.Add(altView);

        try
        {
            if (_emailOptions.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
            }

            using SmtpClient smtpClient = new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port);
            smtpClient.EnableSsl = _emailOptions.UseSSL;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_emailOptions.UserName, _emailOptions.Password);
            smtpClient.Timeout = 30000;
            await smtpClient.SendMailAsync(message);
        }
        finally
        {
            if (_emailOptions.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }
    }

    public async Task SendEmailWithData(string toAddress, string subject, string templateMessage, object data)
    {
        try
        {
            if (_emailOptions.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
            }

            FluentEmailCore.DefaultSender = new SmtpSender(() => new SmtpClient(_emailOptions.SmtpServer, _emailOptions.Port)
            {
                EnableSsl = _emailOptions.UseSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailOptions.UserName, _emailOptions.Password),
                Timeout = 30000
            });

            FluentEmailCore.DefaultRenderer = new RazorRenderer();

            IFluentEmail emailBuilder = FluentEmailCore
                .From(string.IsNullOrWhiteSpace(_emailOptions.From) ? _emailOptions.UserName : _emailOptions.From)
                .To(toAddress)
                .Subject(subject);

            if (_emailOptions.BccShare is not null)
            {
                foreach (string bccAddress in _emailOptions.BccShare)
                {
                    emailBuilder = emailBuilder.BCC(bccAddress);
                }
            }

            IFluentEmail email = emailBuilder.UsingTemplate(templateMessage, data, true);
            await email.SendAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar e-mail para {ToAddress} com o assunto {Subject}", toAddress, subject);
        }
        finally
        {
            if (_emailOptions.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }
    }
}
