namespace BringOutYerDead;

public static class Helpers
{
    // Returns true once the game's natural donkey delivery LogicData has Executed at least once.
    // Replaces a fragile 15-quest IsQuestSucced check that drifted out of sync on some saves
    // (users with the carrot box already unlocked could still report TutorialDone == false).
    // LogicData._started flips true inside Execute() (game_code/Assembly-CSharp/LogicData.cs:89)
    // and persists across saves via [SerializeField], so this is a definitive ground-truth signal
    // for "the donkey has begun delivering bodies, BOYD can safely take over the cadence".
    internal static bool TutorialDone()
    {
        if (!MainGame.game_started) return false;
        if (MainGame.me?.save?.game_logics == null) return false;
        var donkeyLogic = MainGame.me.save.game_logics.GetLogicByID("donkey");
        return donkeyLogic != null && donkeyLogic._started;
    }

    // Gives the human-readable reason the last TutorialDone() call returned whatever it did —
    // only meant for diagnostic logging so we can tell phase-gate regressions apart from
    // save-state weirdness in user-submitted BepInEx logs.
    internal static string TutorialDoneReason()
    {
        if (!MainGame.game_started) return "MainGame.game_started==false";
        if (MainGame.me?.save?.game_logics == null) return "save.game_logics==null";
        var donkeyLogic = MainGame.me.save.game_logics.GetLogicByID("donkey");
        if (donkeyLogic == null) return "no LogicData with id='donkey' in save";
        return donkeyLogic._started ? "donkey LogicData._started==true" : "donkey LogicData._started==false (vanilla delivery hasn't fired yet)";
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
