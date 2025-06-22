namespace Seedify;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.outofseasonseeds";
    private const string PluginName = "Seedify";
    private const string PluginVersion = "0.1.0";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> DebugLogging { get; private set; }
    internal static ConfigEntry<bool> PlantSeedsInAnySeason { get; private set; }
    internal static ConfigEntry<bool> PlantSeedsInAnyFarmType { get; private set; }
    internal static ConfigEntry<bool> AddOutOfSeasonSeedsToFarmingShop { get; private set; }
    public static ConfigEntry<bool> EverythingIsRegrowable { get; private set; }
    public static Plugin PluginInstance { get; private set; }

    private void Awake()
    {
        PluginInstance = this;
        Log = Logger;

        DebugLogging = Config.Bind("01. General", "Debug Logging", false, new ConfigDescription("Enable debug logging. Recommended off unless asked to turn on.", null, new ConfigurationManagerAttributes { Order = 100 }));
        AddOutOfSeasonSeedsToFarmingShop = Config.Bind("01. Seeds", "Add Out Of Season Seeds To Farming Shop", true, new ConfigDescription("Add out of season seeds to the farming shop. You will need to reload your save to reset this.", null, new ConfigurationManagerAttributes { Order = 99 }));

        PlantSeedsInAnySeason = Config.Bind("02. Seeds", "Plant Seeds In Any Season", true, new ConfigDescription("Allow all seed types to be planted regardless of the current season.", null, new ConfigurationManagerAttributes { Order = 98 }));
        PlantSeedsInAnySeason.SettingChanged += (_, _) => { Patches.RunUpdaters(); };

        PlantSeedsInAnyFarmType = Config.Bind("02. Seeds", "Plant Seeds In Any Farm Type", true, new ConfigDescription("Allow planting of any seed on any farm type", null, new ConfigurationManagerAttributes { Order = 97 }));
        PlantSeedsInAnyFarmType.SettingChanged += (_, _) => { Patches.RunUpdaters(); };
        
        EverythingIsRegrowable = Config.Bind("02. Seeds", "Everything Is Regrowable", true, new ConfigDescription("Allow everything to be regrow-able.", null, new ConfigurationManagerAttributes { Order = 95 }));
        EverythingIsRegrowable.SettingChanged += (_, _) => { Patches.RunUpdaters(); };
        
        SceneManager.sceneLoaded += Patches.SetupModifiedData;

        ScenePortalManager.onLoadedWorld += Patches.RunUpdaters;
        ScenePortalManager.onFinishLoadingDecorations += Patches.RunUpdaters;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {PlatformHelper.Current}.");
    }
}