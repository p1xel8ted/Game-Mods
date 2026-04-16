namespace MiscBitsAndBobs;

public static class Helpers
{
    private static bool _sprintMsgShown;
    internal static bool Sprint;

    internal static void ActionsOnSpawnPlayer()
    {
        if (Plugin.DebugEnabled) Log("[SpawnPlayer] GlobalEventsCheck postfix fired — running spawn-time actions.");
        if (!MainGame.game_started) return;

        if (Plugin.DebugEnabled && !Plugin.DebugDialogShown)
        {
            Plugin.DebugDialogShown = true;
            Lang.Reload();
            GUIElements.me.dialog.OpenOK(Plugin.PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
        }

        if (!_sprintMsgShown && Sprint && Plugin.ModifyPlayerMovementSpeedConfig.Value)
        {
            if (Plugin.DebugEnabled) Log("[SpawnPlayer] Sprint Reloaded detected with player speed override on — showing incompatibility dialog.");
            Lang.Reload();
            GUIElements.me.dialog.OpenOK(Lang.Get("Title"), null, Lang.Get("Content"), true);
            _sprintMsgShown = true;
        }
    }

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
}
