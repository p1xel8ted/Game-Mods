using System.Threading;
using Helper;

namespace UltraWide;

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
            Tools.Log("UltraWide", $"{message}", true);
        else
            Tools.Log("UltraWide", $"{message}");
    }
}