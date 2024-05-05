using HarmonyLib;

namespace NewCycle;

[Harmony]
public static class Patches
{

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.Update))]
    public static void MenuManager_Update(ref MenuManager __instance)
    {
        if (__instance.isIntroPlaying)
        {
            __instance.isIntroPlaying = false;
            LeanTween.cancel(__instance.tweenIntro, true);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScenePersistentData), nameof(ScenePersistentData.CheckForSupporterPack))]
    [HarmonyPatch(typeof(ScenePersistentData), nameof(ScenePersistentData.CheckForSupporterPackOwnership))]
    public static void ScenePersistentData_Check(ref bool __result)
    {
        __result = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(MenuManager), nameof(MenuManager.ShowMainMenu))]
    public static void MenuManager_ShowMainMenu(ref MenuManager __instance)
    {
        ScenePersistentData.Ins.isIntroPlayed = true;
        ScenePersistentData.Ins.isEnableAsync = true;
        ScenePersistentData.Ins.hasDLC2 = true;
        ScenePersistentData.Ins.hasDLC3 = true;
        ScenePersistentData.Ins.hasSupporterPack = true;
    }

}