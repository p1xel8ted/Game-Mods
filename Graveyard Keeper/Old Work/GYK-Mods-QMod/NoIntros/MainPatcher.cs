using HarmonyLib;
using Helper;
using LazyBearGames.Preloader;
using System;
using System.Reflection;

namespace NoIntros;

[HarmonyPatch]
public static class MainPatcher
{
    public static void Patch()
    {
        try
        {
            var harmony = new Harmony("p1xel8ted.GraveyardKeeper.NoIntros");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }

    private static void Log(string message, bool error = false)
    {
        Tools.Log("NoIntros", $"{message}", error);
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(LBPreloader), nameof(LBPreloader.StartAnimations))]
    public static void LBPreloader_StartAnimations(LBPreloader __instance)
    {
        if (MainGame.game_started) return;
        __instance.logos.Clear();
    }
}