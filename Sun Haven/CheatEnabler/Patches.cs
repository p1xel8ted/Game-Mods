namespace CheatEnabler;

[Harmony]
public class Patches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerSettings), nameof(PlayerSettings.SetCheatsEnabled))]
    public static void PlayerSettings_SetCheatsEnabled(ref bool enable)
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Override PlayerSettings.SetCheatsEnabled() to TRUE");
        }
        enable = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(QuantumConsoleManager), nameof(QuantumConsoleManager.additem))]
    public static bool QuantumConsoleManager_additem(string item)
    {
        var id = Database.GetID(item);
        if (id <= 0)
        {
            Utils.LogToPlayer($"Item '{item}' not found.");
            return false; //don't let the original method run
        }
        Database.GetData(id, delegate(ItemData data)
        {
            if (!data.isDLCItem) return;
            Utils.LogToPlayer($"Unfortunately, {data.FormattedName} is a DLC item and cannot be added via Cheat Enabler.");
        });
        return true; //let the original method run
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuantumConsole), nameof(QuantumConsole.Initialize))]
    public static void QuantumConsole_Initialize()
    {
        if (Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo("Forcing load of custom commands, and built-in commands.");
        }
        QuantumConsoleProcessor.LoadCommandsFromType(typeof(CheatEnablerCommands));
        QuantumConsoleProcessor.LoadCommandsFromType(typeof(QuantumConsoleManager));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuantumConsoleProcessor), nameof(QuantumConsoleProcessor.TryAddCommand))]
    public static void QuantumConsoleProcessor_TryAddCommand(CommandData command, bool __result)
    {
        if (__result && Plugin.Debug.Value)
        {
            Plugin.LOG.LogInfo($"Added Command: '{command.CommandSignature}' from '{command.MethodData.DeclaringType.GetDisplayName()}'");
        }
    }

}