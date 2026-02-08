namespace GiveMeMoar;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.givememoar";
    private const string PluginName = "Give Me Moar!";
    private const string PluginVer = "1.2.10";
    
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ConfigEntry<bool> MultiplySticks { get; private set; }
    internal static ConfigEntry<float> FaithMultiplier { get; private set; }
    internal static ConfigEntry<float> ResourceMultiplier { get; private set; }
    internal static ConfigEntry<float> GratitudeMultiplier { get; private set; }
    internal static ConfigEntry<float> SinShardMultiplier { get; private set; }
    internal static ConfigEntry<float> DonationMultiplier { get; private set; }
    internal static ConfigEntry<float> BlueTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> GreenTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> RedTechPointMultiplier { get; private set; }
    internal static ConfigEntry<float> HappinessMultiplier { get; private set; }
    internal static ManualLogSource Log { get; private set; }
   


    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {

        BlueTechPointMultiplier = Config.Bind("01. Multipliers", "Blue Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for blue tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 17}));

        DonationMultiplier = Config.Bind("01. Multipliers", "Donation Multiplier", 1f, new ConfigDescription("Adjust the multiplier for donations", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 16}));

        FaithMultiplier = Config.Bind("01. Multipliers", "Faith Multiplier", 1f, new ConfigDescription("Adjust the multiplier for faith", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 15}));

        GratitudeMultiplier = Config.Bind("01. Multipliers", "Gratitude Multiplier", 1f, new ConfigDescription("Adjust the multiplier for gratitude", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 14}));

        GreenTechPointMultiplier = Config.Bind("01. Multipliers", "Green Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for green tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 13}));

        HappinessMultiplier = Config.Bind("01. Multipliers", "Happiness Multiplier", 1f, new ConfigDescription("Adjust the multiplier for happiness", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 12}));

        RedTechPointMultiplier = Config.Bind("01. Multipliers", "Red Tech Point Multiplier", 1f, new ConfigDescription("Adjust the multiplier for red tech points", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 11}));

        ResourceMultiplier = Config.Bind("01. Multipliers", "Resource Multiplier", 1f, new ConfigDescription("Adjust the multiplier for resources", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 10}));

        SinShardMultiplier = Config.Bind("01. Multipliers", "Sin Shard Multiplier", 1f, new ConfigDescription("Adjust the multiplier for sin shards", new AcceptableValueRange<float>(1f, 50f), new ConfigurationManagerAttributes {Order = 9}));

        MultiplySticks = Config.Bind("3. Miscellaneous", "Multiply Sticks", false, new ConfigDescription("Sticks get multiplied endlessly when used in the garden. Enable this to exclude them.", null, new ConfigurationManagerAttributes {Order = 8}));

        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 7}));
    }
}