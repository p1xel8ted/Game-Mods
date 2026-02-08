namespace MiscBitsAndBobs;

public static class Helpers
{
    private static bool _sprintMsgShown;
    internal static bool Sprint;

    private static CultureInfo GameCulture =>
        CultureInfo.GetCultureInfo(GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());

    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = GameCulture;
        return content;
    }

    internal static void ActionsOnSpawnPlayer()
    {
        Plugin.Log.LogInfo($"Running MiscBitsAndBobs ActionsOnSpawnPlayer as Player has spawned in.");
        if (MainGame.game_started && !_sprintMsgShown && Sprint && Plugin.ModifyPlayerMovementSpeedConfig.Value)
        {
            GUIElements.me.dialog.OpenOK(GetLocalizedString(strings.Title), null, GetLocalizedString(strings.Content), true);
            _sprintMsgShown = true;
        }
    }
}