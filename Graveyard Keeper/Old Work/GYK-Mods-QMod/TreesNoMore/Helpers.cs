using System.Threading;
using Helper;

namespace TreesNoMore;

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
            Tools.Log("TreesNoMore", $"{message}", true);
        else
            Tools.Log("TreesNoMore", $"{message}");
    }
}