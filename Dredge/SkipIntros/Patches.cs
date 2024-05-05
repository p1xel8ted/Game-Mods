using HarmonyLib;

namespace SkipIntros;

[Harmony]
public static class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SplashController), nameof(SplashController.OnEnable))]
    public static bool SplashController_OnEnable(ref SplashController __instance)
    {
        __instance.OnSplashComplete();
        return false;
    }
}