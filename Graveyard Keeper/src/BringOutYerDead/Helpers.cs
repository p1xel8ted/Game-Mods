namespace BringOutYerDead;

public static class Helpers
{
    private static readonly string[] Quests =
    [
        "start", "start2", "start3", "start4", "start5", "start6", "start7", "start8",
        "start_place_body_on_table_place_grave", "goto_bishop", "goto_tavern_start",
        "goto_tavern_tech", "goto_tavern_2", "player_repairs_sword_before", "player_repairs_sword"
    ];

    private static CultureInfo GameCulture =>
        CultureInfo.GetCultureInfo(GameSettings.me.language.Replace('_', '-').ToLower(CultureInfo.InvariantCulture).Trim());

    internal static void SetUICulture()
    {
        Thread.CurrentThread.CurrentUICulture = GameCulture;
    }

    internal static bool TutorialDone()
    {
        if (!MainGame.game_started) return false;
        var completed = true;
        foreach (var q in Quests)
        {
            completed = MainGame.me.save.quests.IsQuestSucced(q);
            if (!completed) break;
        }
        return !MainGame.me.save.IsInTutorial() && completed;
    }

    internal static void Log(string message, bool error = false)
    {
        if (error)
        {
            Plugin.Log.LogError($"{message}");
        }
        else
        {
            if (Plugin.Debug.Value)
            {
                Plugin.Log.LogInfo($"{message}");
            }
        }
    }
    
    
}