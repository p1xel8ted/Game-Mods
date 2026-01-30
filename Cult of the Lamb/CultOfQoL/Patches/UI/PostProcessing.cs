using UnityEngine.Rendering.PostProcessing;

namespace CultOfQoL.Patches.UI;

[Harmony]
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

    /// <summary>
    /// Caches volume when registered and applies game graphics settings.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PostProcessManager), nameof(PostProcessManager.Register), typeof(PostProcessVolume))]
    [HarmonyPatch(typeof(PostProcessManager), nameof(PostProcessManager.Register), typeof(PostProcessVolume), typeof(int))]
    public static void VolumeManager_Register(PostProcessVolume volume)
    {
        Volumes.CacheVolume(volume);
        Volumes.UpdateSingleVolume(volume);
    }

    /// <summary>
    /// Updates volume components when a volume is enabled.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(PostProcessVolume), nameof(PostProcessVolume.OnEnable))]
    public static void Volume_OnEnable(PostProcessVolume __instance)
    {
        Volumes.CacheVolume(__instance);
        Volumes.UpdateSingleVolume(__instance);
    }

    /// <summary>
    /// Intercepts game graphics settings changes to propagate to all cached volumes.
    /// Fixes DLC zones not respecting graphics settings.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GraphicsSettingsUtilities), nameof(GraphicsSettingsUtilities.UpdatePostProcessing))]
    public static void GraphicsSettingsUtilities_UpdatePostProcessing()
    {
        Volumes.UpdateAllVolumes();
    }

    /// <summary>
    /// Refreshes the volume cache on location change.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocationManager), nameof(LocationManager.ActivateLocation))]
    public static void LocationManager_ActivateLocation()
    {
        Volumes.RefreshVolumeCache();
    }
}