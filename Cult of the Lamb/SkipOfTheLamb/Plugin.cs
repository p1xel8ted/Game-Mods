namespace SkipOfTheLamb;

[BepInDependency("com.bepis.bepinex.configurationmanager", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInIncompatibility("p1xel8ted.cotl.skipofthelamblite")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.skipofthelamb";
    private const string PluginName = "Skip of the Lamb";
    private const string PluginVer = "0.1.1";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> SkipDevIntros { get; private set; }
    internal static ConfigEntry<bool> SkipCrownVideo { get; private set; }
    internal static ConfigEntry<bool> SkipWoolhavenPrerenderedCinematic { get; private set; }
    internal static ConfigEntry<bool> SkipWoolhavenInGameFirstArrivalScene { get; private set; }
    internal static ConfigEntry<bool> SkipBossIntros { get; private set; }
    internal static ConfigEntry<bool> SkipMiniBossIntros { get; private set; }
    internal static ConfigEntry<bool> IgnoreFirstEncounter { get; private set; }

    private void Awake()
    {
        Log = Logger;

        SkipDevIntros = Config.Bind("01. Pre-Rendered Videos", "Skip Splash Screens", false,
            new ConfigDescription("Skip developer/publisher logo videos on startup.", null,
                new ConfigurationManagerAttributes { Order = 2 }));

        SkipCrownVideo = Config.Bind("01. Pre-Rendered Videos", "Skip Crown Video", false,
            new ConfigDescription("Skip the video when the player first dies and is crowned by the death cat.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        SkipWoolhavenPrerenderedCinematic = Config.Bind("01. Pre-Rendered Videos", "Skip Woolhaven Video", false,
            new ConfigDescription("Skip the pre-rendered video after completing the Woolhaven intro dungeon.", null,
                new ConfigurationManagerAttributes { Order = 0 }));

        SkipWoolhavenInGameFirstArrivalScene = Config.Bind("02. In-Game Cutscenes", "Skip Woolhaven Arrival", false,
            new ConfigDescription("Skip the in-game cutscene when first arriving in Woolhaven.", null,
                new ConfigurationManagerAttributes { Order = 3 }));

        SkipBossIntros = Config.Bind("02. In-Game Cutscenes", "Skip Main Boss Intros", false,
            new ConfigDescription("Skip in-game boss ritual/transformation sequences.", null,
                new ConfigurationManagerAttributes { Order = 2 }));

        SkipMiniBossIntros = Config.Bind("02. In-Game Cutscenes", "Skip Mini-Boss Intros", false,
            new ConfigDescription("Skip in-game mini-boss roar/spawn sequences.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        IgnoreFirstEncounter = Config.Bind("02. In-Game Cutscenes", "Ignore First Encounter", false,
            new ConfigDescription("Skip boss intros even on first encounter. Useful for veteran players on new saves.", null,
                new ConfigurationManagerAttributes { Order = 0 }));

       Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Helpers.PrintModLoaded(PluginName, Logger);
    }
}
