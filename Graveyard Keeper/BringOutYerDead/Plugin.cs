namespace BringOutYerDead;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.bringoutyerdead";
    private const string PluginName = "Bring Out Yer Dead!";
    private const string PluginVer = "0.1.9";

    internal static ConfigEntry<bool> Debug;
    internal static ManualLogSource Log { get; private set; }

    private static ConfigEntry<bool> MorningDelivery { get; set; }
    private static ConfigEntry<bool> DayDelivery { get; set; }
    private static ConfigEntry<bool> NightDelivery { get; set; }
    private static ConfigEntry<bool> EveningDelivery { get; set; }
    internal static ConfigEntry<int> DonkeySpeed { get; private set; }

    internal static ConfigEntry<bool> InternalMorningDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalDayDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalEveningDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalNightDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalDonkeySpawned { get; private set; }
    private static ConfigEntry<bool> InternalTutMessageShown { get;  set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        InitInternalConfiguration();
        Actions.EndOfDayPrefix += Patches.EnvironmentEngine_OnEndOfDay;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void InitConfiguration()
    {
        MorningDelivery = Config.Bind("01. Delivery Times", "Morning Delivery", true, new ConfigDescription("Enable deliveries during the morning hours", null, new ConfigurationManagerAttributes {Order = 6}));
        DayDelivery = Config.Bind("01. Delivery Times", "Day Delivery", false, new ConfigDescription("Enable deliveries during the daytime hours", null, new ConfigurationManagerAttributes {Order = 5}));
        NightDelivery = Config.Bind("01. Delivery Times", "Night Delivery", false, new ConfigDescription("Enable deliveries during the nighttime hours", null, new ConfigurationManagerAttributes {Order = 4}));
        EveningDelivery = Config.Bind("01. Delivery Times", "Evening Delivery", true, new ConfigDescription("Enable deliveries during the evening hours", null, new ConfigurationManagerAttributes {Order = 3}));

        DonkeySpeed = Config.Bind("02. Donkey Settings", "Donkey Speed", 2, new ConfigDescription("Adjust the donkey's speed for deliveries (minimum value is 2)", new AcceptableValueRange<int>(2, 20), new ConfigurationManagerAttributes {Order = 2}));
        Debug = Config.Bind("03. Advanced", "Debug Logging", false, new ConfigDescription("Enable detailed logging for debugging purposes", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
    }

    private void InitInternalConfiguration()
    {
        InternalMorningDelivery = Config.Bind("Internal (Dont Touch)", "Morning Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 6}));
        InternalDayDelivery = Config.Bind("Internal (Dont Touch)", "Day Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 5}));
        InternalEveningDelivery = Config.Bind("Internal (Dont Touch)", "Evening Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 4}));
        InternalNightDelivery = Config.Bind("Internal (Dont Touch)", "Night Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 3}));
        InternalDonkeySpawned = Config.Bind("Internal (Dont Touch)", "Donkey Spawned Done", false, new ConfigDescription("Internal use. Used for tracking donkey spawn state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 2}));
        InternalTutMessageShown = Config.Bind("Internal (Dont Touch)", "Tut Message Shown", false, new ConfigDescription("Internal use. Used for tracking tutorial message state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 1}));
    }
}