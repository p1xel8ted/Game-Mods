namespace DecompDelight;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.decompdelight";
    private const string PluginName = "Decomp Delight!";
    private const string PluginVer = "0.1.3";
    internal static ManualLogSource LOG { get; private set; }

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
        SlowingColor = Config.Bind("Colors", "Slowing", new Color(0.478f, 0.235f, 0.043f), "Color for Slowing element");
        AccelerationColor = Config.Bind("Colors", "Acceleration", new Color(0.035f, 0.157f, 0.384f), "Color for Acceleration element");
        HealthColor = Config.Bind("Colors", "Health", new Color(0.145f, 0.322f, 0.004f), "Color for Health element");
        DeathColor = Config.Bind("Colors", "Death", new Color(0.220f, 0.039f, 0.310f), "Color for Death element");
        OrderColor = Config.Bind("Colors", "Order", new Color(0.851f, 0.984f, 0.455f), "Color for Order element");
        ToxicColor = Config.Bind("Colors", "Toxic", new Color(0.776f, 0.145f, 0.075f), "Color for Toxic element");
        ChaosColor = Config.Bind("Colors", "Chaos", new Color(0.537f, 0.035f, 0.843f), "Color for Chaos element");
        LifeColor = Config.Bind("Colors", "Life", new Color(0.647f, 0.435f, 0.004f), "Color for Life element");
        ElectricColor = Config.Bind("Colors", "Electric", new Color(0.141f, 1f, 1f), "Color for Electric element");
        SilverColor = Config.Bind("Colors", "Silver", new Color(0.753f, 0.753f, 0.753f), "Color for Silver element");
        WhiteColor = Config.Bind("Colors", "White", new Color(1f, 1f, 1f), "Color for White element");
        WaterColor = Config.Bind("Colors", "Water", new Color(0.004f, 0.004f, 0.404f), "Color for Water element");
        OilColor = Config.Bind("Colors", "Oil", new Color(0.157f, 0.157f, 0.157f), "Color for Oil element");
        BloodColor = Config.Bind("Colors", "Blood", new Color(0.404f, 0.004f, 0.004f), "Color for Blood element");
        SaltColor = Config.Bind("Colors", "Salt", new Color(0.404f, 0.404f, 0.404f), "Color for Salt element");
        AshColor = Config.Bind("Colors", "Ash", new Color(0.157f, 0.157f, 0.157f), "Color for Ash element");
        AlcoholColor = Config.Bind("Colors", "Alcohol", new Color(0.404f, 0.404f, 0.004f), "Color for Alcohol element");
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, LOG);
    }

}