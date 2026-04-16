namespace GiveMeMoar;

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

    internal static void ShowDebugWarningOnce()
    {
        if (!Plugin.DebugEnabled || Plugin.DebugDialogShown) return;
        Plugin.DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK("Give Me Moar!", null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
