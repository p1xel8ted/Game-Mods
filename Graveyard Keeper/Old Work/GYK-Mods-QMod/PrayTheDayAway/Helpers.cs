using System.Threading;
using Helper;

namespace PrayTheDayAway;

public partial class MainPatcher
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
            Tools.Log("PrayTheDayAway", $"{message}", true);
        else
            Tools.Log("PrayTheDayAway", $"{message}");
    }
}