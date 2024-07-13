namespace MiscBitsAndBobs;

public static class Helpers
{
    private static bool _sprintMsgShown;
    internal static bool SprintTools, SprintHarmony, Sprint;
    private static string GetLocalizedString(string content)
    {
        Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        return content;
    }

    internal static void ActionsOnSpawnPlayer()
    {
        Plugin.Log.LogInfo($"Running MiscBitsAndBobs ActionsOnSpawnPlayer as Player has spawned in.");
        if (MainGame.game_started && !_sprintMsgShown && Sprint && Plugin.ModifyPlayerMovementSpeedConfig.Value)
        {
            Tools.ShowAlertDialog(GetLocalizedString(strings.Title), GetLocalizedString(strings.Content), separateWithStars: true);
            _sprintMsgShown = true;
        }
    }
}