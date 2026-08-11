namespace ESDInstaller.Windows8.Core.Services;

public sealed class ESDInstallerException : Exception
{
    public ESDInstallerException(string messageKey, string detail = "", Exception? innerException = null)
        : base(string.IsNullOrWhiteSpace(detail) ? messageKey : detail, innerException)
    {
        MessageKey = messageKey;
        Detail = detail;
    }

    public string MessageKey { get; }
    public string Detail { get; }
}
