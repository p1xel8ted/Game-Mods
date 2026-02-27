namespace GlyphOverride;

/// <summary>
/// Provides controller prompt customization for Cult of the Lamb.
/// Features:
/// - Force specific controller type prompts per-player (Player 1 and Player 2 independently)
/// - Fixes Nintendo Switch controller A/B button prompts (physically swapped vs Xbox)
/// - Fixes co-op Player 2 seeing "--" instead of button prompts (vanilla bug)
/// </summary>
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.glyphoverride";
    private const string PluginName = "Glyph Override";
    private const string PluginVer = "0.1.2";

    private const string ControllerSection = "── Controller ──";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<ControllerPromptType> ForceControllerType { get; private set; }
    internal static ConfigEntry<ControllerPromptType> ForcePlayer2ControllerType { get; private set; }

    private void Awake()
    {
        Log = Logger;

        ForceControllerType = Config.Bind(
            ControllerSection,
            "Force Controller Prompts",
            ControllerPromptType.Auto,
            new ConfigDescription(
                "Force the game to display button prompts for a specific controller type. " +
                "'Auto' uses the currently connected controller. In co-op, this applies to Player 1 (Lamb).",
                null,
                new ConfigurationManagerAttributes { Order = 2 }));

        ForcePlayer2ControllerType = Config.Bind(
            ControllerSection,
            "Force Player 2 Controller Prompts",
            ControllerPromptType.Auto,
            new ConfigDescription(
                "Force Player 2 (Goat) to display button prompts for a specific controller type. " +
                "'Auto' uses Player 2's currently connected controller. Only applies during co-op.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        // Persistent watcher that broadcasts Player 2's controller changes in co-op
        var watcherGo = new GameObject($"{PluginName}_CoopWatcher");
        watcherGo.AddComponent<CoopControllerWatcher>();
        DontDestroyOnLoad(watcherGo);

        Log.LogInfo($"{PluginName} loaded.");
    }

    /// <summary>
    /// Converts our ControllerPromptType to the game's InputType for the given player.
    /// Returns null for Auto (meaning don't override).
    /// </summary>
    internal static InputType? GetForcedInputType(PlayerFarming player = null)
    {
        var isPlayer2 = (UnityEngine.Object)player != null && !player.isLamb;
        var configValue = isPlayer2 ? ForcePlayer2ControllerType.Value : ForceControllerType.Value;

        return configValue switch
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

/// <summary>
/// Carries player context through static method calls during prompt rendering.
/// Set by AssignPrompt prefix, consumed by downstream patches, cleared by finalizer.
/// </summary>
internal static class PlayerContext
{
    internal static PlayerFarming Current;
}

/// <summary>
/// Monitors Player 2's controller in co-op and broadcasts changes
/// to update button prompts. Fixes the vanilla bug where the global
/// controller change event only fires for Player 1.
/// </summary>
internal class CoopControllerWatcher : MonoBehaviour
{
    private Controller _lastP2Controller;

    private void Update()
    {
        if (!CoopManager.CoopActive || PlayerFarming.playersCount <= 1) return;
        if (PlayerFarming.players.Count <= 1) return;

        var p2 = PlayerFarming.players[1];
        if ((UnityEngine.Object)p2 == null || p2.rewiredPlayer == null) return;

        var currentController = p2.rewiredPlayer.controllers.GetLastActiveController();
        if (currentController == null || currentController == _lastP2Controller) return;

        _lastP2Controller = currentController;
        InputManager.General.OnActiveControllerChanged?.Invoke(currentController);
    }
}

[Harmony]
public static class Patches
{
    // Button icon codes from the game's controller font
    private const string ButtonA = "\uE900"; // action_bottom on Xbox, action_right on Switch
    private const string ButtonB = "\uE901"; // action_right on Xbox, action_bottom on Switch
    private const string ButtonX = "\uE902"; // action_left on Xbox, action_top on Switch
    private const string ButtonY = "\uE903"; // action_top on Xbox, action_left on Switch

    /// <summary>
    /// Captures player context before AssignPrompt executes, so downstream
    /// static methods can know which player's prompts are being rendered.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(MMControlPrompt), nameof(MMControlPrompt.AssignPrompt))]
    public static void AssignPrompt_Prefix(MMControlPrompt __instance)
    {
        PlayerContext.Current = __instance.playerFarming;
    }

    /// <summary>
    /// Clears player context after AssignPrompt completes. Uses Finalizer
    /// (not Postfix) to ensure cleanup even if the method throws.
    /// </summary>
    [HarmonyFinalizer]
    [HarmonyPatch(typeof(MMControlPrompt), nameof(MMControlPrompt.AssignPrompt))]
    public static void AssignPrompt_Finalizer()
    {
        PlayerContext.Current = null;
    }

    /// <summary>
    /// When vanilla ForceUpdate fails to resolve a player's controller,
    /// try harder by checking the player's assigned joysticks directly.
    /// Fixes Player 2 seeing "--" when they haven't pressed any buttons yet.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MMControlPrompt), nameof(MMControlPrompt.ForceUpdate))]
    public static void ForceUpdate_Postfix(MMControlPrompt __instance)
    {
        var pf = __instance.playerFarming;
        if ((UnityEngine.Object)pf == null || pf.rewiredPlayer == null) return;

        // If vanilla already found a controller, no need to intervene
        if (InputManager.General.GetLastActiveController(pf) != null) return;
        if (InputManager.General.GetDefaultController(pf) != null) return;

        // Vanilla failed - try to find any joystick assigned to this player
        foreach (var joystick in pf.rewiredPlayer.controllers.Joysticks)
        {
            if (joystick.isConnected && joystick.enabled)
            {
                __instance.OnActiveControllerChanged(joystick);
                return;
            }
        }
    }

    /// <summary>
    /// Forces the game to return a specific InputType when configured.
    /// Uses player context to apply per-player force settings in co-op.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ControlUtilities), nameof(ControlUtilities.GetCurrentInputType))]
    public static void GetCurrentInputType_Postfix(ref InputType __result)
    {
        var forced = Plugin.GetForcedInputType(PlayerContext.Current);
        if (forced.HasValue)
        {
            __result = forced.Value;
        }
    }

    /// <summary>
    /// Fixes button prompt glyph codes for the correct player's controller.
    /// - A/B swap for all Nintendo Switch controller types (vanilla only handles X/Y)
    /// - X/Y detection uses per-player controller context instead of global state
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ControlMappings), nameof(ControlMappings.GetControllerCodeFromID))]
    public static void GetControllerCodeFromID_Postfix(int id, ref string __result)
    {
        if (id is not (4 or 5 or 7 or 8)) return;

        // Use per-player controller when available, falls back to global when null
        var controller = InputManager.General.GetLastActiveController(PlayerContext.Current);
        var inputType = ControlUtilities.GetCurrentInputType(controller);

        switch (id)
        {
            // A/B swap for all Switch controller types
            case 4 when Plugin.IsSwitchController(inputType):
                __result = ButtonB;
                break;
            case 5 when Plugin.IsSwitchController(inputType):
                __result = ButtonA;
                break;
            // X/Y: vanilla already swaps these for SwitchProController but uses global state;
            // we re-apply with per-player context to fix co-op with mixed controllers
            case 7:
                __result = inputType == InputType.SwitchProController ? ButtonY : ButtonX;
                break;
            case 8:
                __result = inputType == InputType.SwitchProController ? ButtonX : ButtonY;
                break;
        }
    }
}
