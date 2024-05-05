namespace BiggerBackpack;

/// <summary>
/// Main plugin class for More Jewelry!, responsible for initializing and managing plugin configurations and behaviors.
/// </summary>
/// <remarks>
/// This class handles the setup and teardown of the plugin, including configuration settings, event subscriptions, and Harmony patching.
/// </remarks>
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.biggerbackpack";
    private const string PluginName = "Bigger Backpack!";
    private const string PluginVersion = "0.1.0";

    /// <summary>
    /// Gets the logging source for the plugin.
    /// </summary>
    internal static ManualLogSource LOG { get; private set; }

    /// <summary>
    /// Configuration entry for enabling debug logging.
    /// </summary>
    internal static ConfigEntry<bool> Debug { get; private set; }


    /// <summary>
    /// Initialization logic for the plugin.
    /// </summary>
    private void Awake()
    {
        UIHandler.OnInventoryOpened += UI.UIHandler_OpenInventory;
        UIHandler.OnInventoryClosed += UI.UIHandler_CloseInventory;
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);
        Debug = Config.Bind("00. Debug", "Debug", false, new ConfigDescription("Enable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = false, Order = 99}));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    /// <summary>
    /// Logic to execute when the plugin is disabled.
    /// </summary>
    private void OnDisable()
    {
        LOG.LogError($"{PluginName} has been disabled!");
    }

    /// <summary>
    /// Cleanup logic for the plugin.
    /// </summary>
    private void OnDestroy()
    {
        LOG.LogError($"{PluginName} has been destroyed!");
    }
}