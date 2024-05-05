using System.Diagnostics;
using Eremite.Controller;
using Eremite.Intro;
using HarmonyLib;

namespace AgainstTheStorm;

[Harmony]
public static class Patches
{
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MainController), nameof(MainController.ShowLoadingScreen))]
    private static void MainController_ShowLoadingScreen(ref MainController __instance)
    {
        //print out method stack trace, last 5 methods
        var stackTrace = new StackTrace();
        var stackFrames = stackTrace.GetFrames();
        if (stackFrames == null) return;
        for (var i = 0; i < 20; i++)
        {
            var method = stackFrames[i].GetMethod();
            Plugin.Log.LogWarning($"Method: {method.DeclaringType}.{method.Name}");
        }
    }
}