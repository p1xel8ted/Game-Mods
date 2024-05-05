using System.Threading;
using Helper;

namespace LongerDays;

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
            Tools.Log("LongerDays", $"{message}", true);
        else
            Tools.Log("LongerDays", $"{message}");
    }
}