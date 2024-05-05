using System.Threading;
using Helper;

namespace RegenerationReloaded;

public static partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }
    
    private static void Log(string message, bool error = false)
    {
        if (!_cfg.debug) return;
        if (error)
            Tools.Log("RegenerationReloaded", $"{message}", true);
        else
            Tools.Log("RegenerationReloaded", $"{message}");
    }
}