namespace FasterCraftReloaded;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fastercraftreloaded";
    private const string PluginName = "FasterCraft Reloaded";
    private const string PluginVer = "1.4.11";

    private const string AdvancedSection   = "── Advanced ──";
    private const string SpeedSection      = "── Speed ──";
    private const string CompostingSection = "── Composting ──";
    private const string GardensSection    = "── Gardens ──";
    private const string ProductionSection = "── Zombie Production ──";

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> IncreaseBuildAndDestroySpeed { get; private set; }
    internal static ConfigEntry<float> BuildAndDestroySpeed { get; private set; }
    internal static ConfigEntry<float> CraftSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerGardenSpeed { get; private set; }
    internal static ConfigEntry<float> PlayerGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieGardenSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyRefugeeGardenSpeed { get; private set; }
    internal static ConfigEntry<float> RefugeeGardenSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieVineyardSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieVineyardSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieSawmillSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieSawmillSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyZombieMinesSpeed { get; private set; }
    internal static ConfigEntry<float> ZombieMinesSpeedMultiplier { get; private set; }
    internal static ConfigEntry<bool> ModifyCompostSpeed { get; private set; }
    internal static ConfigEntry<float> CompostSpeedMultiplier { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription(
                "Write verbose crafting diagnostics to the BepInEx console — which workbench, which craft, which speed branch was applied. Useful for reporting bugs. Leave off for normal play.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        CraftSpeedMultiplier = Config.Bind(SpeedSection, "Craft Speed Multiplier", 2f,
            new ConfigDescription(
                "Multiplier applied to crafting speed on most workbenches. Higher values finish crafts faster but cost the same total energy. Repair crafts are always excluded so the per-frame energy cost can't exceed your max energy pool.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 100}));

        IncreaseBuildAndDestroySpeed = Config.Bind(SpeedSection, "Faster Build And Destroy", true,
            new ConfigDescription(
                "When on, placing new structures and tearing down old ones happens faster.",
                null,
                new ConfigurationManagerAttributes {Order = 99}));

        BuildAndDestroySpeed = Config.Bind(SpeedSection, "Build And Destroy Speed", 4f,
            new ConfigDescription(
                "Multiplier for the build/destroy speed when the toggle above is on.",
                new AcceptableValueRange<float>(2f, 10f),
                new ConfigurationManagerAttributes {Order = 98}));

        ModifyCompostSpeed = Config.Bind(CompostingSection, "Speed Up Composting", false,
            new ConfigDescription(
                "Speed up the compost heap so it turns waste into fertiliser faster.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        CompostSpeedMultiplier = Config.Bind(CompostingSection, "Composting Multiplier", 2f,
            new ConfigDescription(
                "How much faster compost heaps run when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99}));

        ModifyPlayerGardenSpeed = Config.Bind(GardensSection, "Speed Up Your Garden", false,
            new ConfigDescription(
                "Speed up your own garden beds so crops grow faster.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        PlayerGardenSpeedMultiplier = Config.Bind(GardensSection, "Your Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster your garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99}));

        ModifyRefugeeGardenSpeed = Config.Bind(GardensSection, "Speed Up Refugee Garden", false,
            new ConfigDescription(
                "Speed up the refugee camp's garden beds (Game Of Crone DLC).",
                null,
                new ConfigurationManagerAttributes {Order = 98}));

        RefugeeGardenSpeedMultiplier = Config.Bind(GardensSection, "Refugee Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster refugee garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 97}));

        ModifyZombieGardenSpeed = Config.Bind(GardensSection, "Speed Up Zombie Garden", false,
            new ConfigDescription(
                "Speed up zombie-tended garden beds.",
                null,
                new ConfigurationManagerAttributes {Order = 96}));

        ZombieGardenSpeedMultiplier = Config.Bind(GardensSection, "Zombie Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster zombie garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 95}));

        ModifyZombieMinesSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Mines", false,
            new ConfigDescription(
                "Speed up ore output from zombie-staffed mines.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        ZombieMinesSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Mines Multiplier", 2f,
            new ConfigDescription(
                "How much faster zombie mines produce ore when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99}));

        ModifyZombieSawmillSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Sawmill", false,
            new ConfigDescription(
                "Speed up plank and log output from the zombie sawmill.",
                null,
                new ConfigurationManagerAttributes {Order = 98}));

        ZombieSawmillSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Sawmill Multiplier", 2f,
            new ConfigDescription(
                "How much faster the zombie sawmill produces when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 97}));

        ModifyZombieVineyardSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Vineyard", false,
            new ConfigDescription(
                "Speed up grape and wine output from the zombie vineyard.",
                null,
                new ConfigurationManagerAttributes {Order = 96}));

        ZombieVineyardSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Vineyard Multiplier", 2f,
            new ConfigDescription(
                "How much faster the zombie vineyard produces when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 95}));
    }
}
