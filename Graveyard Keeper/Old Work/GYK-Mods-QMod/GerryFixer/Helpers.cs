using System.Threading;
using Helper;

namespace GerryFixer;

public static partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }
    
    private static void Log(string message, bool error = false)
    {
        if (_cfg.Debug || error)
            Tools.Log("GerryFixer", $"{message}", true);
        else
            Tools.Log("GerryFixer", $"{message}");
    }
}