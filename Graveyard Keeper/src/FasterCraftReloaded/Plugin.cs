namespace FasterCraftReloaded;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection   = "── Advanced ──";
    private const string SpeedSection      = "── Speed ──";
    private const string CompostingSection = "── Composting ──";
    private const string GardensSection    = "── Gardens ──";
    private const string ProductionSection = "── Zombie Production ──";
    private const string UpdatesSection    = "── Updates ──";

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;

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
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
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
                "Multiplier applied to crafts at regular workbenches (anvils, alchemy, cooking, study desks, etc.). Repair crafts are always excluded to protect per-frame energy cost. Gardens, composting, and zombie-worked stations are not covered by this — each has its own toggle further down.",
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
                new ConfigurationManagerAttributes {Order = 98, DispName = "    └ Build And Destroy Speed"}));

        ModifyCompostSpeed = Config.Bind(CompostingSection, "Speed Up Composting", false,
            new ConfigDescription(
                "Speed up the compost heap so it turns waste into fertiliser faster.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        CompostSpeedMultiplier = Config.Bind(CompostingSection, "Composting Multiplier", 2f,
            new ConfigDescription(
                "How much faster compost heaps run when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Composting Multiplier"}));

        ModifyPlayerGardenSpeed = Config.Bind(GardensSection, "Speed Up Your Garden", false,
            new ConfigDescription(
                "Speed up your own garden beds so crops grow faster.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        PlayerGardenSpeedMultiplier = Config.Bind(GardensSection, "Your Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster your garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Your Garden Multiplier"}));

        ModifyRefugeeGardenSpeed = Config.Bind(GardensSection, "Speed Up Refugee Garden", false,
            new ConfigDescription(
                "Speed up the refugee camp's garden beds (Game Of Crone DLC).",
                null,
                new ConfigurationManagerAttributes {Order = 98}));

        RefugeeGardenSpeedMultiplier = Config.Bind(GardensSection, "Refugee Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster refugee garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 97, DispName = "    └ Refugee Garden Multiplier"}));

        ModifyZombieGardenSpeed = Config.Bind(GardensSection, "Speed Up Zombie Garden", false,
            new ConfigDescription(
                "Speed up zombie-tended garden beds.",
                null,
                new ConfigurationManagerAttributes {Order = 96}));

        ZombieGardenSpeedMultiplier = Config.Bind(GardensSection, "Zombie Garden Multiplier", 2f,
            new ConfigDescription(
                "How much faster zombie garden crops grow when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 95, DispName = "    └ Zombie Garden Multiplier"}));

        ModifyZombieMinesSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Mines", false,
            new ConfigDescription(
                "Speed up ore output from zombie-staffed mines in the stone yard, marble deposit, and iron mine.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        ZombieMinesSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Mines Multiplier", 2f,
            new ConfigDescription(
                "How much faster zombie mines produce ore when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Zombie Mines Multiplier"}));

        ModifyZombieSawmillSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Sawmill", false,
            new ConfigDescription(
                "Speed up plank and log output from the zombie sawmill.",
                null,
                new ConfigurationManagerAttributes {Order = 98}));

        ZombieSawmillSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Sawmill Multiplier", 2f,
            new ConfigDescription(
                "How much faster the zombie sawmill produces when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 97, DispName = "    └ Zombie Sawmill Multiplier"}));

        ModifyZombieVineyardSpeed = Config.Bind(ProductionSection, "Speed Up Zombie Vineyard", false,
            new ConfigDescription(
                "Speed up grape and wine output from the zombie vineyard.",
                null,
                new ConfigurationManagerAttributes {Order = 96}));

        ZombieVineyardSpeedMultiplier = Config.Bind(ProductionSection, "Zombie Vineyard Multiplier", 2f,
            new ConfigDescription(
                "How much faster the zombie vineyard produces when the toggle above is on.",
                new AcceptableValueRange<float>(1f, 50f),
                new ConfigurationManagerAttributes {Order = 95, DispName = "    └ Zombie Vineyard Multiplier"}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
    }
}
