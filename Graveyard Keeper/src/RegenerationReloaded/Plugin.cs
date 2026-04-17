namespace RegenerationReloaded;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static ManualLogSource Log { get; set; }
    internal static ConfigEntry<bool> ShowRegenUpdates { get; private set; }
    internal static ConfigEntry<float> LifeRegen { get; private set; }
    internal static ConfigEntry<float> EnergyRegen { get; private set; }
    internal static ConfigEntry<float> RegenDelay { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitConfiguration()
    {
        ShowRegenUpdates = Config.Bind("01. Regeneration", "Show Regeneration Updates", true, new ConfigDescription("Display updates when life and energy regenerate.", null, new ConfigurationManagerAttributes {Order = 4}));
        ShowRegenUpdates.SettingChanged += (_, _) => { Patches.ShowRegenUpdates = ShowRegenUpdates.Value; };
        LifeRegen = Config.Bind("01. Regeneration", "Life Regeneration Rate", 2f, new ConfigDescription("Set the rate at which life regenerates per tick. Set to 0 to disable.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 3}));
        LifeRegen.SettingChanged += (_, _) => { Patches.LifeRegen = LifeRegen.Value; };
        EnergyRegen = Config.Bind("01. Regeneration", "Energy Regeneration Rate", 1f, new ConfigDescription("Set the rate at which energy regenerates per tick. Set to 0 to disable.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 2}));
        EnergyRegen.SettingChanged += (_, _) => { Patches.EnergyRegen = EnergyRegen.Value; };
        RegenDelay = Config.Bind("01. Regeneration", "Regeneration Delay", 5f, new ConfigDescription("Set the delay in seconds between each regeneration tick.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 1}));
        RegenDelay.SettingChanged += (_, _) => { Patches.RegenDelay = RegenDelay.Value; };

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }
    
}