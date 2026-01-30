using Lamb.UI.SettingsMenu;
using UnityEngine.Rendering.PostProcessing;

namespace CultOfQoL.Patches.UI;

/// <summary>
/// Ensures game graphics settings (Chromatic Aberration, Bloom, Vignette, Depth of Field)
/// apply to all PostProcessVolume instances including DLC zones.
/// </summary>
public static class Volumes
{
    /// <summary>
    /// Cached PostProcessVolume instances - refreshed on location changes.
    /// </summary>
    private static readonly HashSet<PostProcessVolume> CachedVolumes = [];

    /// <summary>
    /// Called when a volume is registered or enabled - add to cache.
    /// </summary>
    internal static void CacheVolume(PostProcessVolume volume)
    {
        if (volume != null)
        {
            CachedVolumes.Add(volume);
        }
    }

    /// <summary>
    /// Refresh the cache - call on location changes.
    /// </summary>
    internal static void RefreshVolumeCache()
    {
        CachedVolumes.Clear();
        var volumes = Resources.FindObjectsOfTypeAll<PostProcessVolume>();
        foreach (var vol in volumes)
        {
            CachedVolumes.Add(vol);
        }
    }

    /// <summary>
    /// Update all cached volumes with game graphics settings.
    /// </summary>
    public static void UpdateAllVolumes()
    {
        // Remove any destroyed volumes
        CachedVolumes.RemoveWhere(v => v == null);

        foreach (var vol in CachedVolumes)
        {
            UpdateSingleVolume(vol);
        }
    }

    /// <summary>
    /// Updates a single volume's components to reflect game graphics settings.
    /// This ensures DLC zones respect the same settings as the base game.
    /// </summary>
    public static void UpdateSingleVolume(PostProcessVolume volume)
    {
        var settings = SettingsManager.Settings?.Graphics;
        if (settings == null) return;

        foreach (var effect in volume.profile.settings)
        {
            switch (effect)
            {
                case ChromaticAberration ca:
                    ca.enabled.value = settings.ChromaticAberration;
                    break;
                case Bloom bloom:
                    bloom.enabled.value = settings.Bloom;
                    break;
                case Vignette vignette:
                    vignette.enabled.value = settings.Vignette;
                    break;
                case DepthOfField dof:
                    dof.enabled.value = settings.DepthOfField;
                    break;
            }
        }
    }
}
