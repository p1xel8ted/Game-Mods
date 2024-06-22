namespace CheatEnabler;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetCheatsEnabled))]
    public static void PlayerSettings_SetCheatsEnabled(ref bool enable)
    {
        LOG.LogInfo("Override PlayerSettings.SetCheatsEnabled() to TRUE");
        enable = true;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.EnableMenu))]
    public static void MainMenuController_EnableMenu()
    {
        Settings.EnableCheats = true;
        if (QuantumConsole.Instance)
        {
            QuantumConsole.Instance.GenerateCommands = true;
            QuantumConsoleProcessor.ClearCommandTable();
            QuantumConsole.Instance.Initialize();
        }
    }
}