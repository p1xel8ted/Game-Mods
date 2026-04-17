namespace PrayTheDayAway;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> EverydayIsSermonDay { get; private set; }
    internal static ConfigEntry<bool> SermonOverAndOver { get; private set; }
    internal static ConfigEntry<bool> NotifyOnPrayerLoss { get; private set; }
    internal static ConfigEntry<bool> AlternateMode { get; private set; }
    internal static ConfigEntry<bool> NoLossOnDailySermons { get; private set; }
    internal static ConfigEntry<bool> RandomlyUpgradeBasicPrayer { get; private set; }
    internal static ConfigEntry<bool> SpeedUpSermon { get; private set; }
    internal static ConfigEntry<int> SermonSpeed { get; private set; }
    internal static ConfigEntry<bool> CheatModeConfig { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {Order = 597}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        EverydayIsSermonDay = Config.Bind("01. General", "Everyday Is Sermon Day", true, new ConfigDescription("Allow sermons to be held every day.", null, new ConfigurationManagerAttributes {Order = 606}));

        SermonOverAndOver = Config.Bind("01. General", "Sermon Over And Over", false, new ConfigDescription("Allow sermons to be repeated without limitation.", null, new ConfigurationManagerAttributes {Order = 605}));


        AlternateMode = Config.Bind("02. Mode", "Alternate Mode", true, new ConfigDescription("Chance to lower item level instead of chance to lose it on prayer.", null, new ConfigurationManagerAttributes {Order = 603}));

        NoLossOnDailySermons = Config.Bind("02. Mode", "No Loss On Daily Sermons", false, new ConfigDescription("When enabled, daily sermons will never lose or downgrade prayer items. Only applies when Everyday Is Sermon Day is enabled.", null, new ConfigurationManagerAttributes {Order = 602}));

        NotifyOnPrayerLoss = Config.Bind("03. Notifications", "Notify On Prayer Loss", true, new ConfigDescription("Display notifications when prayer items are lost.", null, new ConfigurationManagerAttributes {Order = 602}));

        RandomlyUpgradeBasicPrayer = Config.Bind("04. Upgrades", "Randomly Upgrade Basic Prayer", true, new ConfigDescription("Allow basic prayers to be randomly upgraded (to a known starred prayer).", null, new ConfigurationManagerAttributes {Order = 601}));

        SpeedUpSermon = Config.Bind("04. Speed", "Speed Up Sermon", false, new ConfigDescription("Speed up the sermon sequence. Note that everything will be faster while the sermon is running.", null, new ConfigurationManagerAttributes {Order = 600}));
        SermonSpeed = Config.Bind("04. Speed", "Sermon Speed", 5, new ConfigDescription("Default game speed is 1. Increase as desired.", new AcceptableValueRange<int>(2, 10), new ConfigurationManagerAttributes {Order = 599}));


        CheatModeConfig = Config.Bind("05. Cheats", "Cheat Mode", false, new ConfigDescription("Allow sermons to be repeated without limitation. Other settings do no function when this is enabled.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 598}));

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }
}
