namespace Fabledom;

[Harmony]
public static class Patches
{
    
    private readonly static string[] ScalersToAdd = ["Main canvas"];

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CanvasScaler), nameof(CanvasScaler.OnEnable))]
    public static void CanvasScaler_OnEnable(CanvasScaler __instance)
    {
        if (ScalersToAdd.Contains(__instance.name))
        {
            Plugin.CanvasScalers.Add(__instance);
            __instance.screenMatchMode = Plugin.ScreenMatchMode.Value;
            __instance.uiScaleMode = Plugin.ScaleMode.Value;
            if (__instance.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                __instance.scaleFactor = Plugin.ScaleFactor.Value;
            }
        }
    }
}