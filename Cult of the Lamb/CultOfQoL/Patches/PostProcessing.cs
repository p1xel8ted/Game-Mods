namespace CultOfQoL.Patches;

[Harmony]
public static class PostProcessing
{
    private static TranslucentImage vignetteImage;
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TranslucentImage), nameof(TranslucentImage.OnEnable))]
    private static void TranslucentImage_OnEnable(TranslucentImage __instance)
    {
        if (__instance.activeSprite && __instance.activeSprite.name.Contains("vignette"))
        {
            vignetteImage = __instance;
            ToggleVignette();
        }
    }
    
    internal static void ToggleVignette()
    {
        if (vignetteImage)
        {
            vignetteImage.enabled = Plugin.VignetteEffect.Value;
        }
    }
}