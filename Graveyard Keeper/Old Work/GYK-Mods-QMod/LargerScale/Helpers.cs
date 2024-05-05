using System.Threading;
using Helper;

namespace LargerScale;

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
            Tools.Log("LargerScale", $"{message}", true);
        else
            Tools.Log("LargerScale", $"{message}");
    }
}