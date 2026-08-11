using System.Security.Principal;

namespace ESDInstaller.Windows8.Core.Services;

public static class PrivilegeService
{
    public static bool IsAdministrator()
    {
        using (var identity = WindowsIdentity.GetCurrent())
            return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
    }
}
