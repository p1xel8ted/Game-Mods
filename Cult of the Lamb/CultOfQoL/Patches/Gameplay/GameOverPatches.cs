namespace CultOfQoL.Patches.Gameplay;

[Harmony]
public static class GameOverPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    public static void PlayerController_Update()
    {
        if (!Plugin.DisableGameOver.Value) return;
        DisableGameOverFlags();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase), typeof(DayPhase))]
    public static void TimeManager_StartNewPhase()
    {
        if (!Plugin.DisableGameOver.Value) return;
        DisableGameOverFlags();
    }

    private static void DisableGameOverFlags()
    {
        var data = DataManager.Instance;
        data.GameOverEnabled = false;
        data.GameOver = false;
        data.InGameOver = false;
        data.DisplayGameOverWarning = false;
    }
}