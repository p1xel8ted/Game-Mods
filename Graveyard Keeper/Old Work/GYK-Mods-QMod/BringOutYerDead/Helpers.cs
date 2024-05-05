using System.Threading;
using Helper;

namespace BringOutYerDead;

public static partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }
    
    private static void Log(string message, bool error = false)
    {
        if (!_cfg.Debug) return;
        if (error)
            Tools.Log("BringOutYerDead", $"{message}", true);
        else
            Tools.Log("BringOutYerDead", $"{message}");
    }
}