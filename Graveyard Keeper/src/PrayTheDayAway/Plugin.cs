namespace PrayTheDayAway;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.praythedayaway";
    private const string PluginName = "Pray The Day Away!";
    private const string PluginVer = "0.3.3";

    private static ConfigEntry<bool> Debug { get; set; }
    private static ManualLogSource Log { get; set; }

    private static ConfigEntry<bool> EverydayIsSermonDay { get; set; }
    private static ConfigEntry<bool> SermonOverAndOver { get; set; }
    private static ConfigEntry<bool> NotifyOnPrayerLoss { get; set; }
    private static ConfigEntry<bool> AlternateMode { get; set; }
    private static ConfigEntry<bool> RandomlyUpgradeBasicPrayer { get; set; }
    private static ConfigEntry<bool> SpeedUpSermon { get; set; }
    private static ConfigEntry<int> SermonSpeed { get; set; }
    private static ConfigEntry<bool> CheatModeConfig { get; set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        StartupLogger.PrintModLoaded(PluginName, Log);
    }

    private void InitConfiguration()
    {
        EverydayIsSermonDay = Config.Bind("01. General", "Everyday Is Sermon Day", true, new ConfigDescription("Allow sermons to be held every day.", null, new ConfigurationManagerAttributes {Order = 606}));

        SermonOverAndOver = Config.Bind("01. General", "Sermon Over And Over", false, new ConfigDescription("Allow sermons to be repeated without limitation.", null, new ConfigurationManagerAttributes {Order = 605}));


        AlternateMode = Config.Bind("02. Mode", "Alternate Mode", true, new ConfigDescription("Chance to lower item level instead of chance to lose it on prayer.", null, new ConfigurationManagerAttributes {Order = 603}));

        NotifyOnPrayerLoss = Config.Bind("03. Notifications", "Notify On Prayer Loss", true, new ConfigDescription("Display notifications when prayer items are lost.", null, new ConfigurationManagerAttributes {Order = 602}));

        RandomlyUpgradeBasicPrayer = Config.Bind("04. Upgrades", "Randomly Upgrade Basic Prayer", true, new ConfigDescription("Allow basic prayers to be randomly upgraded (to a known starred prayer).", null, new ConfigurationManagerAttributes {Order = 601}));

        SpeedUpSermon = Config.Bind("04. Speed", "Speed Up Sermon", false, new ConfigDescription("Speed up the sermon sequence. Note that everything will be faster while the sermon is running.", null, new ConfigurationManagerAttributes {Order = 600}));
        SermonSpeed = Config.Bind("04. Speed", "Sermon Speed", 5, new ConfigDescription("Default game speed is 1. Increase as desired.", new AcceptableValueRange<int>(2, 10), new ConfigurationManagerAttributes {Order = 599}));


        CheatModeConfig = Config.Bind("05. Cheats", "Cheat Mode", false, new ConfigDescription("Allow sermons to be repeated without limitation. Other settings do no function when this is enabled.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 598}));

        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {Order = 597}));
    }
}