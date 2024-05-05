using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

namespace AnAlchemicalCollection;

[HarmonyPatch]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class ResolutionPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SettingsManager), nameof(SettingsManager.LOAD))]
    public static void SettingsManager_LOAD()
    {
        if (Plugin.CustomTargetFramerate.Value)
        {
            Application.targetFrameRate = Plugin.FrameRate.Value;
        }

        if (!Plugin.ModifyResolutions.Value) return;

        SettingsManager.resolutionList.Add(Plugin.Resolution);
        Screen.SetResolution(Plugin.Resolution.width, Plugin.Resolution.height, Screen.fullScreen, Plugin.Resolution.refreshRate);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(GraphicSettingUI), nameof(GraphicSettingUI.SetGraphicLayout))]
    public static void GraphicSettingUI_SetGraphicLayout(ref GraphicSettingUI __instance)
    {
        if (Plugin.CustomTargetFramerate.Value)
        {
            Application.targetFrameRate = Plugin.FrameRate.Value;
        }

        if (!Plugin.ModifyResolutions.Value) return;

        __instance.resolutionAr.AddItem(Plugin.Resolution);
        Screen.SetResolution(Plugin.Resolution.width, Plugin.Resolution.height, Screen.fullScreen, Plugin.Resolution.refreshRate);
    }
}