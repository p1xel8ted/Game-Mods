using System.Threading;
using GYKHelper;

namespace PrayTheDayAway;

public partial class Plugin
{
    internal static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }
    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            Log.LogError($"{message}");
        }
        else
        {
            if (_debug.Value)
            {
                Log.LogInfo($"{message}");
            }
        }
    }
}