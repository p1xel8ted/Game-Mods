using I2.Loc;

namespace SkipOfTheLambLite;

[Harmony]
public static class Patches
{
    private static bool _hasSkippedDevIntros;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.Start))]
    public static bool LoadMainMenu_Start()
    {
        if (_hasSkippedDevIntros) return true;

        Plugin.Log.LogInfo("[LoadMainMenu.Start]: Skipping dev intros");

        _hasSkippedDevIntros = true;

        AudioManager.Instance.enabled = true;

        // Initialize fonts that would normally be set up during splash screens
        // Without this, localized fonts (Russian, CJK, Arabic) may fail to load
        GameManager.ForceFontReload();
        LocalizationManager.SetupFonts();

        MMTransition.Play(
            MMTransition.TransitionType.ChangeSceneAutoResume,
            MMTransition.Effect.BlackFade,
            "Main Menu",
            0f,
            "",
            null
        );

        return false;
    }
}
