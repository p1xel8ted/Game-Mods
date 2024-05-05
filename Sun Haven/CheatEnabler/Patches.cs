namespace CheatEnabler;

[HarmonyPatch]
public partial class Plugin
{
    private static bool AlreadyRun { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetCheatsEnabled))]
    public static bool PlayerSettings_SetCheatsEnabled(ref bool enable)
    {
        if (AlreadyRun) return false;
        enable = true;
        AlreadyRun = true;
        LOG.LogInfo("Cheat Menu Enabled...");
        return true;
    }
}