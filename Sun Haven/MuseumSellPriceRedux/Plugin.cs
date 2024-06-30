namespace MuseumSellPriceRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.museumsellpriceredux";
    private const string PluginName = "Museum Sell Price Redux";
    private const string PluginVersion = "0.1.4";

    internal static ConfigEntry<bool> Enabled { get; private set; }
    private static ConfigEntry<bool> Debug { get; set; }
    internal static ConfigEntry<float> Multiplier { get; private set; }
    internal static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        Database.OnDataFinishedLoading += Patches.ItemData_Loaded;
        LOG = new ManualLogSource(PluginName);
        BepInEx.Logging.Logger.Sources.Add(LOG);
        Enabled = Config.Bind("01. General", "Enabled", true, new ConfigDescription("Toggle mod. Click 'Apply' to save changes.", null, new ConfigurationManagerAttributes
            {Order = 51}));
        Debug = Config.Bind("01. General", "Debug", false, new ConfigDescription("Toggle debug logging", null, new ConfigurationManagerAttributes
            {Order = 50}));
        Multiplier = Config.Bind("02. Selling", "Multiplier", 100f, new ConfigDescription("A base value that will be used to determine how much to multiply the value of USELESS museum only items (default 100 to me seems like the minimum to make someone care to not throw away something like an 'Ancient Sun Haven Sword'). Click 'Apply' to save changes.", new AcceptableValueRange<float>(10, 1000), new ConfigurationManagerAttributes
            {Order = 49}));
        Multiplier.SettingChanged += (_, _) =>
        {
            Multiplier.Value = Mathf.Clamp(Multiplier.Value, 10, 1000);
            Multiplier.Value = Mathf.Round(Multiplier.Value * 2) / 2;
        };
        Config.Bind("03. Apply", "Apply", true,
            new ConfigDescription("Apply Changes", null,
                new ConfigurationManagerAttributes
                    {Order = 48, HideDefaultButton = true, CustomDrawer = ApplyChanges}));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private static void ApplyChanges(ConfigEntryBase entry)
    {
        var button = GUILayout.Button("Apply Changes", GUILayout.ExpandWidth(true));
        if (!button) return;
        if (Enabled.Value)
        {
            Patches.ApplyPriceChanges(true);  
        }
        else
        {
            Patches.RestorePrices();
        }
    }

    internal static void SendNotification(string message)
    {
        if (SingletonBehaviour<NotificationStack>.Instance is null)
        {
            return;
        }
        SingletonBehaviour<NotificationStack>.Instance.SendNotification($"{PluginName}: {message}");
    }
    
    internal static void Log(string message, bool error = false, bool debug = false)
    {
        if (error)
        {
            LOG.LogError(message);
            return;
        }

        if (Debug.Value && debug)
        {
            LOG.LogWarning(message);
            return;
        }

        if (!debug)
        {
            LOG.LogInfo(message);
        }
    }
    
    private void OnDestroy()
    {
        OnDisable();
    }
    
    private void OnDisable()
    {
        LOG.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }
}