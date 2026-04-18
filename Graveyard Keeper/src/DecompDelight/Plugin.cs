namespace DecompDelight;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection = "── Advanced ──";
    private const string ColorsSection   = "── Colors ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["Colors"]       = ColorsSection,
    };

    internal static ManualLogSource LOG { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private static ConfigEntry<Color> SlowingColor { get; set; }
    private static ConfigEntry<Color> AccelerationColor { get; set; }
    private static ConfigEntry<Color> HealthColor { get; set; }
    private static ConfigEntry<Color> DeathColor { get; set; }
    private static ConfigEntry<Color> OrderColor { get; set; }
    private static ConfigEntry<Color> ToxicColor { get; set; }
    private static ConfigEntry<Color> ChaosColor { get; set; }
    private static ConfigEntry<Color> LifeColor { get; set; }
    private static ConfigEntry<Color> ElectricColor { get; set; }
    private static ConfigEntry<Color> SilverColor { get; set; }
    private static ConfigEntry<Color> WhiteColor { get; set; }
    private static ConfigEntry<Color> WaterColor { get; set; }
    private static ConfigEntry<Color> OilColor { get; set; }
    private static ConfigEntry<Color> BloodColor { get; set; }
    private static ConfigEntry<Color> SaltColor { get; set; }
    private static ConfigEntry<Color> AshColor { get; set; }
    private static ConfigEntry<Color> AlcoholColor { get; set; }
        
    internal static string SlowingColorHex => Utils.ColorToHex(SlowingColor.Value);
    internal static string AccelerationColorHex => Utils.ColorToHex(AccelerationColor.Value);
    internal static string HealthColorHex => Utils.ColorToHex(HealthColor.Value);
    internal static string DeathColorHex => Utils.ColorToHex(DeathColor.Value);
    internal static string OrderColorHex => Utils.ColorToHex(OrderColor.Value);
    internal static string ToxicColorHex => Utils.ColorToHex(ToxicColor.Value);
    internal static string ChaosColorHex => Utils.ColorToHex(ChaosColor.Value);
    internal static string LifeColorHex => Utils.ColorToHex(LifeColor.Value);
    internal static string ElectricColorHex => Utils.ColorToHex(ElectricColor.Value);
    internal static string SilverColorHex => Utils.ColorToHex(SilverColor.Value);
    internal static string WhiteColorHex => Utils.ColorToHex(WhiteColor.Value);
    internal static string WaterColorHex => Utils.ColorToHex(WaterColor.Value);
    internal static string OilColorHex => Utils.ColorToHex(OilColor.Value);
    internal static string BloodColorHex => Utils.ColorToHex(BloodColor.Value);
    internal static string SaltColorHex => Utils.ColorToHex(SaltColor.Value);
    internal static string AshColorHex => Utils.ColorToHex(AshColor.Value);
    internal static string AlcoholColorHex => Utils.ColorToHex(AlcoholColor.Value);
    
    private void Awake()
    {
        LOG = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose decompose-element diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 1}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);

        SlowingColor      = Config.Bind(ColorsSection, "Slowing",      new Color(0.478f, 0.235f, 0.043f), "Colour used in decompose tooltips for the Slowing element.");
        AccelerationColor = Config.Bind(ColorsSection, "Acceleration", new Color(0.035f, 0.157f, 0.384f), "Colour used in decompose tooltips for the Acceleration element.");
        HealthColor       = Config.Bind(ColorsSection, "Health",       new Color(0.145f, 0.322f, 0.004f), "Colour used in decompose tooltips for the Health element.");
        DeathColor        = Config.Bind(ColorsSection, "Death",        new Color(0.220f, 0.039f, 0.310f), "Colour used in decompose tooltips for the Death element.");
        OrderColor        = Config.Bind(ColorsSection, "Order",        new Color(0.851f, 0.984f, 0.455f), "Colour used in decompose tooltips for the Order element.");
        ToxicColor        = Config.Bind(ColorsSection, "Toxic",        new Color(0.776f, 0.145f, 0.075f), "Colour used in decompose tooltips for the Toxic element.");
        ChaosColor        = Config.Bind(ColorsSection, "Chaos",        new Color(0.537f, 0.035f, 0.843f), "Colour used in decompose tooltips for the Chaos element.");
        LifeColor         = Config.Bind(ColorsSection, "Life",         new Color(0.647f, 0.435f, 0.004f), "Colour used in decompose tooltips for the Life element.");
        ElectricColor     = Config.Bind(ColorsSection, "Electric",     new Color(0.141f, 1f, 1f),         "Colour used in decompose tooltips for the Electric element.");
        SilverColor       = Config.Bind(ColorsSection, "Silver",       new Color(0.753f, 0.753f, 0.753f), "Colour used in decompose tooltips for the Silver element.");
        WhiteColor        = Config.Bind(ColorsSection, "White",        new Color(1f, 1f, 1f),             "Colour used in decompose tooltips for the White element.");
        WaterColor        = Config.Bind(ColorsSection, "Water",        new Color(0.004f, 0.004f, 0.404f), "Colour used in decompose tooltips for the Water element.");
        OilColor          = Config.Bind(ColorsSection, "Oil",          new Color(0.157f, 0.157f, 0.157f), "Colour used in decompose tooltips for the Oil element.");
        BloodColor        = Config.Bind(ColorsSection, "Blood",        new Color(0.404f, 0.004f, 0.004f), "Colour used in decompose tooltips for the Blood element.");
        SaltColor         = Config.Bind(ColorsSection, "Salt",         new Color(0.404f, 0.404f, 0.404f), "Colour used in decompose tooltips for the Salt element.");
        AshColor          = Config.Bind(ColorsSection, "Ash",          new Color(0.157f, 0.157f, 0.157f), "Colour used in decompose tooltips for the Ash element.");
        AlcoholColor      = Config.Bind(ColorsSection, "Alcohol",      new Color(0.404f, 0.404f, 0.004f), "Colour used in decompose tooltips for the Alcohol element.");

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));

        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old numbered section headers to the new "── Name ──" style so existing
    // user values survive the rename. Idempotent.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try
        {
            content = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            LOG.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
            return;
        }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception ex)
        {
            LOG.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        LOG.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }
}