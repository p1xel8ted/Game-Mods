namespace CultOfQoL.Patches;

[HarmonyPatch]
public static class GameOverPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Update))]
    public static void PlayerController_Update()
    {
        if (!Plugin.DisableGameOver.Value) return;
        DataManager.Instance.GameOverEnabled = false;
        DataManager.Instance.GameOver = false;
        DataManager.Instance.InGameOver = false;
        DataManager.Instance.DisplayGameOverWarning = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase), typeof(DayPhase))]
    public static void TimeManager_StartNewPhase()
    {
        if (!Plugin.DisableGameOver.Value) return;
        DataManager.Instance.GameOverEnabled = false;
        DataManager.Instance.GameOver = false;
        DataManager.Instance.InGameOver = false;
        DataManager.Instance.DisplayGameOverWarning = false;
    }
}