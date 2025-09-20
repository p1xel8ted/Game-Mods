namespace CultOfQoL.Patches.UI;

[HarmonyPatch]
public static class PostProcessing
{
    private static readonly HashSet<TranslucentImage> VignetteImages = [];
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TranslucentImage), nameof(TranslucentImage.OnEnable))]
    private static void TranslucentImage_OnEnable(TranslucentImage __instance)
    {
        if (!IsVignetteImage(__instance)) return;
        
        VignetteImages.Add(__instance); // HashSet handles duplicates automatically
        __instance.enabled = Plugin.VignetteEffect.Value;
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(TranslucentImage), nameof(TranslucentImage.OnDisable))]
    private static void TranslucentImage_OnDisable(TranslucentImage __instance)
    {
        VignetteImages.Remove(__instance);
    }
    
    internal static void ToggleVignette()
    {
        // Clean up any destroyed objects and toggle remaining
        VignetteImages.RemoveWhere(img => !img);
        
        foreach (var vignetteImage in VignetteImages)
        {
            vignetteImage.enabled = Plugin.VignetteEffect.Value;
        }
    }
    
    private static bool IsVignetteImage(TranslucentImage image)
    {
        return image?.activeSprite && 
               image.activeSprite.name.Contains("vignette", StringComparison.OrdinalIgnoreCase);
    }
}