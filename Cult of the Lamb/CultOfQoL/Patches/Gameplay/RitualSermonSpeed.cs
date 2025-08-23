namespace CultOfQoL.Patches.Gameplay;

[Harmony]
[HarmonyWrapSafe]
public static class RitualSermonSpeed
{
    internal static bool RitualRunning { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoRitual))] // rituals
    [HarmonyPatch(typeof(SermonController), nameof(SermonController.Play))] // sermons
    private static void Interaction_TempleAltar_Do()
    {
        if (Plugin.FastRitualSermons.Value)
        {
            RitualRunning = true;
        }
    }

    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.OpenPlayerUpgradeRoutine))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.OpenPlayerUpgradeMenu))]
    private static void Interaction_TempleAltar_OpenPlayerUpgradeMenu()
    {
        GameManager.SetTimeScale(1);
        RitualRunning = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.UseUnscaledTime), MethodType.Getter)]
    private static void Follower_UseUnscaledTime_Get(ref bool __result)
    {
        if (!RitualRunning) return;
        __result = false;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Follower), nameof(Follower.UseUnscaledTime), MethodType.Setter)]
    private static void Follower_UseUnscaledTime_Set(ref bool value)
    {
        if (!RitualRunning) return;
        value = false;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.RitualOnEnd))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.DoCancel))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.Close))]
    [HarmonyPatch(typeof(Interaction_TempleAltar), nameof(Interaction_TempleAltar.CloseAndSpeak))]
    private static void Interaction_TempleAltar_End()
    {
        RitualRunning = false;
        GameManager.SetTimeScale(1);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]
    private static void GameManager_Update()
    {
        if (!RitualRunning || !Plugin.FastRitualSermons.Value) return;

        GameManager.SetTimeScale(10); //set this too fast and stuff starts to break...
    }
}