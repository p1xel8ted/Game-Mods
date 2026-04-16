namespace BringOutYerDead;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.bringoutyerdead";
    private const string PluginName = "Bring Out Yer Dead!";
    private const string PluginVer = "0.2.6";

    private const string AdvancedSection       = "── Advanced ──";
    private const string DeliveryTimesSection  = "── Delivery Times ──";
    private const string DonkeySection         = "── Donkey ──";
    private const string InternalSection       = "Internal (Dont Touch)";

    internal static ConfigEntry<bool> Debug;
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ManualLogSource Log { get; private set; }

    internal static bool PrideDayLogged { get; set; }
    internal static WorldGameObject Donkey { get; set; }

    internal static ConfigEntry<bool> MorningDelivery { get; set; }
    internal static ConfigEntry<bool> DayDelivery { get; set; }
    internal static ConfigEntry<bool> NightDelivery { get; set; }
    internal static ConfigEntry<bool> EveningDelivery { get; set; }
    internal static ConfigEntry<int> DonkeySpeed { get; private set; }

    internal static ConfigEntry<bool> InternalMorningDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalDayDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalEveningDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalNightDelivery { get; private set; }
    internal static ConfigEntry<bool> InternalDonkeySpawned { get; private set; }
    internal static ConfigEntry<bool> InternalTutMessageShown { get;  set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        InitInternalConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        if (DebugEnabled)
        {
            Log.LogInfo($"[Init] {PluginName} v{PluginVer} loaded. DonkeySpeed={DonkeySpeed.Value}, Morning={MorningDelivery.Value}, Day={DayDelivery.Value}, Evening={EveningDelivery.Value}, Night={NightDelivery.Value}");
        }
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription(
                "Write detailed delivery-scheduling diagnostics to the BepInEx console — phase transitions, donkey state, carrot checks, and retry reasons. Turn this on when reporting bugs so the log shows what the mod is actually seeing. Leave off for normal play.",
                null,
                new ConfigurationManagerAttributes {Order = 1}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        MorningDelivery = Config.Bind(DeliveryTimesSection, "Morning Delivery", true,
            new ConfigDescription(
                "Trigger a body delivery at the start of the morning phase (just after sunrise). Default: on.",
                null,
                new ConfigurationManagerAttributes {Order = 6}));

        DayDelivery = Config.Bind(DeliveryTimesSection, "Day Delivery", false,
            new ConfigDescription(
                "Trigger a body delivery at the start of the daytime phase (around midday). Default: off — turn on if you want an extra mid-day delivery on top of the morning/evening ones.",
                null,
                new ConfigurationManagerAttributes {Order = 5}));

        EveningDelivery = Config.Bind(DeliveryTimesSection, "Evening Delivery", true,
            new ConfigDescription(
                "Trigger a body delivery at the start of the evening phase (late afternoon). Default: on.",
                null,
                new ConfigurationManagerAttributes {Order = 4}));

        NightDelivery = Config.Bind(DeliveryTimesSection, "Night Delivery", false,
            new ConfigDescription(
                "Trigger a body delivery at the start of the night phase (after sunset). Default: off — the donkey would otherwise arrive right as you're heading to bed.",
                null,
                new ConfigurationManagerAttributes {Order = 3}));

        DonkeySpeed = Config.Bind(DonkeySection, "Donkey Speed", 2,
            new ConfigDescription(
                "How fast the donkey walks to the cemetery and back. Vanilla speed is 1; higher values shorten the round trip so each delivery finishes well within its time slot. Raise this to 10–20 if deliveries feel like they arrive too late.",
                new AcceptableValueRange<int>(2, 20),
                new ConfigurationManagerAttributes {Order = 2}));
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    private void InitInternalConfiguration()
    {
        InternalMorningDelivery = Config.Bind(InternalSection, "Morning Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 6}));
        InternalDayDelivery = Config.Bind(InternalSection, "Day Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 5}));
        InternalEveningDelivery = Config.Bind(InternalSection, "Evening Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 4}));
        InternalNightDelivery = Config.Bind(InternalSection, "Night Delivery Done", false, new ConfigDescription("Internal use. Used for tracking a days delivery state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 3}));
        InternalDonkeySpawned = Config.Bind(InternalSection, "Donkey Spawned Done", false, new ConfigDescription("Internal use. Used for tracking donkey spawn state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 2}));
        InternalTutMessageShown = Config.Bind(InternalSection, "Tut Message Shown", false, new ConfigDescription("Internal use. Used for tracking tutorial message state.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 1}));
    }
}
