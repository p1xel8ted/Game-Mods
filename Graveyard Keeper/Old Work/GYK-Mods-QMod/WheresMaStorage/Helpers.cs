using System.Threading;
using Helper;

namespace WheresMaStorage;

public partial class MainPatcher
{
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    internal static void Log(string message, bool error = false)
    {
        if (!_cfg.Debug) return;
        if (error)
            Tools.Log("WheresMaStorage", $"{message}", true);
        else
            Tools.Log("WheresMaStorage", $"{message}");
    }
}