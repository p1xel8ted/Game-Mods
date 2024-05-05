using System.Threading;
using Helper;

namespace AddStraightToTable;

public static partial class MainPatcher
{
    internal static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }
    internal static void Log(string message, bool error = false)
    {
        if (error)
            Tools.Log("AddStraightToTable", $"{message}", true);
        else
            Tools.Log("AddStraightToTable", $"{message}");
    }
}