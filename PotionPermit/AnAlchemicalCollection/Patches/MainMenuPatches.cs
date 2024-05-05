using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace AnAlchemicalCollection;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MainMenuPatches
{
    private const string MainMenu = "MAIN_MENU";

    //make menu buttons appear instantly
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LeanTween), nameof(LeanTween.value), typeof(float), typeof(float), typeof(float))]
    public static void LeanTween_value(LeanTween __instance, float from, float to, ref float time)
    {
        if (!Plugin.SpeedUpMenuIntro.Value) return;
        if (from == 0f && Mathf.Approximately(to,1f) && Mathf.Approximately(time,3f))
        {
            time = 0f;
        }
    } 
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LeanTween), nameof(LeanTween.moveLocalY), typeof(GameObject), typeof(float), typeof(float))]
    public static void LeanTween_moveLocalY(LeanTween __instance, GameObject gameObject, float to, ref float time)
    {
        if (!Plugin.SpeedUpMenuIntro.Value) return;
        if (gameObject.name == MainMenu)
        {
            time = 0f;
        }
    }
}