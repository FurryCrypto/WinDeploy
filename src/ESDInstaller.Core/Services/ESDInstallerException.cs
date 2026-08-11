namespace ESDInstaller.Core.Services;

public sealed class ESDInstallerException : Exception
{
    public ESDInstallerException(string messageKey, string technicalDetail, Exception? inner = null)
        : base(technicalDetail, inner)
    {
        MessageKey = messageKey;
        TechnicalDetail = technicalDetail;
    }

    public string MessageKey { get; }
    public string TechnicalDetail { get; }
}
