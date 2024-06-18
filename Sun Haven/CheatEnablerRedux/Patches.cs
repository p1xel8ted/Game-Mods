namespace CheatEnablerRedux;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetCheatsEnabled))]
    public static void PlayerSettings_SetCheatsEnabled(ref bool enable)
    {
        enable = true;
    }
}