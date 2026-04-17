namespace KeepersCandles;

internal static class Helpers
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
        // Shared multi-target postfix also fires on GameBalance.LoadGameBalance, before the
        // player GUI is alive — guard against that so OpenOK doesn't NRE.
        if (!MainGame.game_started || GUIElements.me == null || GUIElements.me.dialog == null) return;
        Plugin.DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
