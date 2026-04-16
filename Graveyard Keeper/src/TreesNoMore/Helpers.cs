namespace TreesNoMore;

public static class Helpers
{
    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    // One-shot in-game reminder so a player who flipped on Debug Logging in a previous session
    // is told it's still on next time they load. Mirrors WMS/QE/SaveNow patterns.
    internal static void ShowDebugWarningOnce()
    {
        if (!Plugin.DebugEnabled || Plugin.DebugDialogShown) return;
        Plugin.DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(Plugin.PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
