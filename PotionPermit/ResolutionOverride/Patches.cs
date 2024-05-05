using HarmonyLib;
using UnityEngine;

namespace ResolutionOverride;

[HarmonyPatch]
public class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.LOAD))]
    public static void Load()
    {
        SettingsManager.resolutionList.Add(Plugin.Resolution);
        Application.targetFrameRate = Plugin.FrameRate.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GraphicSettingUI), nameof(GraphicSettingUI.SetGraphicLayout))]
    public static void SetGraphicLayout(ref GraphicSettingUI __instance)
    {
        __instance.resolutionAr.AddItem(Plugin.Resolution);
        Application.targetFrameRate = Plugin.FrameRate.Value;
    }
    
    

}