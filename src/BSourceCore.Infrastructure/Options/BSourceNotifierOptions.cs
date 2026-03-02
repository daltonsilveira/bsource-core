namespace BSourceCore.Infrastructure.Options;

/// <summary>
/// Configurações do serviço BSourceNotifier
/// </summary>
public class BSourceNotifierOptions
{
    public const string SectionName = "BSourceNotifier";

    /// <summary>
    /// URL base do serviço de notificação (ex: http://192.167.0.2:5000)
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// Indica se o serviço de notificação está habilitado
    /// </summary>
    public bool Enabled { get; set; } = true;
}
