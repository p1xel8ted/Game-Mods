using I2.Loc;

namespace TraitControl.Patches;

[Harmony]
public static class LocalizationPatches
{
    private static bool _hasInitialized;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.InitializeIfNeeded))]
    public static void LocalizationManager_InitializeIfNeeded()
    {
        if (_hasInitialized)
        {
            return;
        }

        _hasInitialized = true;
        Plugin.LogAllTraitNames();
        Plugin.UpdateTraitDisplayNames();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalizationManager), nameof(LocalizationManager.SetLanguageAndCode))]
    public static void LocalizationManager_SetLanguageAndCode()
    {
        // Update display names when language changes
        Plugin.UpdateTraitDisplayNames();
    }
}
