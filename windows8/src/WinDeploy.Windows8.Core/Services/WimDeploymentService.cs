using ManagedWimLib;

namespace WinDeploy.Windows8.Core.Services;

public sealed class WimDeploymentService
{
    public Task ApplyAsync(string imagePath, int index, string targetRoot, Action<int> progress,
        Action<string> log, CancellationToken cancellationToken) => Task.Run(() =>
    {
        WimLibNative.EnsureInitialized();
        try
        {
            using (var wim = Wim.OpenWim(imagePath, OpenFlags.CheckIntegrity))
            {
                wim.RegisterCallback((message, info, context) =>
                {
                    if (cancellationToken.IsCancellationRequested) return CallbackStatus.Abort;
                    var extraction = info as ExtractProgress;
                    if (extraction != null && extraction.TotalBytes > 0)
                    {
                        var percent = (int)Math.Min(100, extraction.CompletedBytes * 100UL / extraction.TotalBytes);
                        progress(percent);
                    }
                    if (message == ProgressMsg.ExtractImageBegin || message == ProgressMsg.ExtractImageEnd)
                        log(message.ToString());
                    return CallbackStatus.Continue;
                });
                wim.ExtractImage(index, targetRoot, ExtractFlags.RpFix);
            }
            cancellationToken.ThrowIfCancellationRequested();
            progress(100);
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception exception)
        {
            if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException(cancellationToken);
            throw new WinDeployException("ErrorDismApply", exception.Message, exception);
        }
    }, cancellationToken);
}
