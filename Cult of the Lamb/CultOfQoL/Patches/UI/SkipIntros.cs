namespace CultOfQoL.Patches.UI;

[HarmonyPatch]
public static class SkipIntros
{
    private static bool _hasSkippedDevIntros;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(IntroDeathSceneManager), nameof(IntroDeathSceneManager.GiveCrown))]
    public static bool IntroDeathSceneManager_GiveCrown(ref IntroDeathSceneManager __instance)
    {
        if (!Plugin.SkipCrownVideo.Value) return true;
        
        __instance.VideoComplete();
        DataManager.Instance.HadInitialDeathCatConversation = true;
        return false;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LoadMainMenu), nameof(LoadMainMenu.Start))]
    public static bool LoadMainMenu_Start()
    {
        // Only skip once per game session
        if (_hasSkippedDevIntros || !Plugin.SkipDevIntros.Value) 
            return true;
        
        _hasSkippedDevIntros = true;
        
        // Ensure audio is enabled and transition directly to main menu
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