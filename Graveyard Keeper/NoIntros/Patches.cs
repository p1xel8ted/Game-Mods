namespace NoIntros;

[Harmony]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LBPreloader), nameof(LBPreloader.StartAnimations))]
    public static void LBPreloader_StartAnimations(LBPreloader __instance)
    {
        if (MainGame.game_started) return;
        __instance.logos.Clear();
    }
    
}