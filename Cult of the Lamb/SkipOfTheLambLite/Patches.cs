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
