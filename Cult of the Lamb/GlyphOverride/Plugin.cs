namespace GlyphOverride;

/// <summary>
/// Provides controller prompt customization for Cult of the Lamb.
/// Features:
/// - Force specific controller type prompts regardless of actual controller
/// - Fixes Nintendo Switch controller A/B button prompts (physically swapped vs Xbox)
/// </summary>
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.p1xel8ted.configurationmanagerenhanced", "1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.glyphoverride";
    private const string PluginName = "Glyph Override";
    private const string PluginVer = "0.1.0";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<ControllerPromptType> ForceControllerType { get; private set; }

    private void Awake()
    {
        Log = Logger;

        ForceControllerType = Config.Bind(
            "General",
            "Force Controller Prompts",
            ControllerPromptType.Auto,
            new ConfigDescription(
                "Force the game to display button prompts for a specific controller type. " +
                "'Auto' uses the currently connected controller.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"{PluginName} loaded.");
    }

    /// <summary>
    /// Converts our ControllerPromptType to the game's InputType.
    /// Returns null for Auto (meaning don't override).
    /// </summary>
    internal static InputType? GetForcedInputType()
    {
        return ForceControllerType.Value switch
        {
            ControllerPromptType.Auto => null,
            ControllerPromptType.Keyboard => InputType.Keyboard,
            ControllerPromptType.DualShock4 => InputType.DualShock4,
            ControllerPromptType.DualSense => InputType.DualSense,
            ControllerPromptType.Xbox360 => InputType.Xbox360,
            ControllerPromptType.XboxOne => InputType.XboxOne,
            ControllerPromptType.XboxSeries => InputType.XboxSeries,
            ControllerPromptType.SwitchJoyConsDetached => InputType.SwitchJoyConsDetached,
            ControllerPromptType.SwitchJoyConsDocked => InputType.SwitchJoyConsDocked,
            ControllerPromptType.SwitchHandheld => InputType.SwitchHandheld,
            ControllerPromptType.SwitchProController => InputType.SwitchProController,
            _ => null
        };
    }

    /// <summary>
    /// Checks if the given InputType is a Nintendo Switch controller type.
    /// </summary>
    internal static bool IsSwitchController(InputType inputType)
    {
        return inputType is InputType.SwitchJoyConsDetached
            or InputType.SwitchJoyConsDocked
            or InputType.SwitchHandheld
            or InputType.SwitchProController;
    }
}

/// <summary>
/// Controller types available for forcing button prompts.
/// Mirrors the game's InputType enum with an added Auto option.
/// </summary>
public enum ControllerPromptType
{
    Auto,
    Keyboard,
    DualShock4,
    DualSense,
    Xbox360,
    XboxOne,
    XboxSeries,
    SwitchJoyConsDetached,
    SwitchJoyConsDocked,
    SwitchHandheld,
    SwitchProController
}

[Harmony]
public static class Patches
{
    // Button icon codes from the game's controller font
    private const string ButtonA = "\uE900"; // action_bottom on Xbox, action_right on Switch
    private const string ButtonB = "\uE901"; // action_right on Xbox, action_bottom on Switch

    /// <summary>
    /// Forces the game to return a specific InputType when configured.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ControlUtilities), nameof(ControlUtilities.GetCurrentInputType))]
    public static void GetCurrentInputType_Postfix(ref InputType __result)
    {
        var forced = Plugin.GetForcedInputType();
        if (forced.HasValue)
        {
            __result = forced.Value;
        }
    }

    /// <summary>
    /// Fixes A/B button prompt swap for Nintendo Switch controllers.
    /// On Switch controllers, A/B buttons are physically swapped compared to Xbox:
    /// - Nintendo: A=right, B=bottom
    /// - Xbox: A=bottom, B=right
    /// The game handles X/Y swap but not A/B.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ControlMappings), nameof(ControlMappings.GetControllerCodeFromID))]
    public static void GetControllerCodeFromID_Postfix(int id, ref string __result)
    {
        // Only process button IDs 4 (A) and 5 (B)
        if (id != 4 && id != 5)
        {
            return;
        }

        // Get the effective input type (may be forced or natural)
        var controller = InputManager.General.GetLastActiveController();
        var inputType = ControlUtilities.GetCurrentInputType(controller);

        // Check if it's any Switch controller type
        if (!Plugin.IsSwitchController(inputType))
        {
            return;
        }

        // Swap A and B button icons for Switch controller
        // ID 4 = action_bottom: A on Xbox, B on Switch -> show B icon
        // ID 5 = action_right: B on Xbox, A on Switch -> show A icon
        __result = id == 4 ? ButtonB : ButtonA;
    }
}
