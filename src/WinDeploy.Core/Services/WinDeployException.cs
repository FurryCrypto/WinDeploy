namespace WinDeploy.Core.Services;

public sealed class WinDeployException : Exception
{
    public WinDeployException(string messageKey, string technicalDetail, Exception? inner = null)
        : base(technicalDetail, inner)
    {
        MessageKey = messageKey;
        TechnicalDetail = technicalDetail;
    }

    public string MessageKey { get; }
    public string TechnicalDetail { get; }
}
