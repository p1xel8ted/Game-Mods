using System.Threading;
using Helper;

namespace AppleTreesEnhanced;

public static partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    private static void Log(string message, bool error = false)
    {
        if (error)
            Tools.Log("AppleTreesEnhanced", $"{message}", true);
        else if (_cfg.Debug)
            Tools.Log("AppleTreesEnhanced", $"{message}");
    }
}