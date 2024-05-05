using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Helper;

namespace NamelessFasterTimeMod;

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
            Tools.Log("NamelessFasterTimeMod", $"{message}", true);
        else if (!_cfg.Debug) return;
        Tools.Log("NamelessFasterTimeMod", $"{message}");
    }
}