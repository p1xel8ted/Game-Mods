namespace RegenerationReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.regenerationreloaded";
    private const string PluginName = "Regeneration Reloaded";
    private const string PluginVer = "1.1.8";

    private static ManualLogSource Log { get; set; }
    internal static ConfigEntry<bool> ShowRegenUpdates { get; private set; }
    internal static ConfigEntry<float> LifeRegen { get; private set; }
    internal static ConfigEntry<float> EnergyRegen { get; private set; }
    internal static ConfigEntry<float> RegenDelay { get; private set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        ShowRegenUpdates = Config.Bind("01. Regeneration", "Show Regeneration Updates", true, new ConfigDescription("Display updates when life and energy regenerate.", null, new ConfigurationManagerAttributes {Order = 4}));
        ShowRegenUpdates.SettingChanged += (_, _) => { Patches.ShowRegenUpdates = ShowRegenUpdates.Value; };
        LifeRegen = Config.Bind("01. Regeneration", "Life Regeneration Rate", 2f, new ConfigDescription("Set the rate at which life regenerates per tick.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes {Order = 3}));
        LifeRegen.SettingChanged += (_, _) => { Patches.LifeRegen = LifeRegen.Value; };
        EnergyRegen = Config.Bind("01. Regeneration", "Energy Regeneration Rate", 1f, new ConfigDescription("Set the rate at which energy regenerates per tick.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes {Order = 2}));
        EnergyRegen.SettingChanged += (_, _) => { Patches.EnergyRegen = EnergyRegen.Value; };
        RegenDelay = Config.Bind("01. Regeneration", "Regeneration Delay", 5f, new ConfigDescription("Set the delay in seconds between each regeneration tick.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes {Order = 1}));
        RegenDelay.SettingChanged += (_, _) => { Patches.RegenDelay = RegenDelay.Value; };
    }
    
}