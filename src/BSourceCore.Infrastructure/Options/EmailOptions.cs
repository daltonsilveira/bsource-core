namespace BSourceCore.Infrastructure.Options;

public class EmailOptions
{
    public const string SectionName = "Email";
    
    public string Account { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? From { get; set; }
    public string? DisplayName { get; set; }
    public bool UseSSL { get; set; } = true;
    public bool IgnoreCertificateErrors { get; set; } = false;
    public List<string>? BccShare { get; set; }
}