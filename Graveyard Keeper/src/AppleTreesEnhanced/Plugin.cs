namespace AppleTreesEnhanced;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string PlayerGardenSection     = "── Player Garden ──";
    private const string HarvestingSection       = "── Harvesting ──";
    private const string WorldEnvironmentSection = "── World Environment ──";
    private const string EconomySection          = "── Economy ──";
    private const string AdvancedSection         = "── Advanced ──";
    private const string UpdatesSection          = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["1. Player Garden"]     = PlayerGardenSection,
        ["2. Harvesting"]        = HarvestingSection,
        ["3. World Environment"] = WorldEnvironmentSection,
        ["4. Economy"]           = EconomySection,
        ["5. Advanced"]          = AdvancedSection,
    };

    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;

    internal static ConfigEntry<bool> IncludeGardenBerryBushes { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenTrees { get; private set; }
    internal static ConfigEntry<bool> IncludeWorldBerryBushes { get; private set; }
    internal static ConfigEntry<bool> ShowHarvestReadyMessages { get; private set; }
    internal static ConfigEntry<bool> RealisticHarvest { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenBeeHives { get; private set; }
    internal static ConfigEntry<bool> BeeKeeperBuyback { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }


    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
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
            Log.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
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
            Log.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        IncludeGardenBerryBushes = Config.Bind(PlayerGardenSection, "Include Garden Berry Bushes", true,
            new ConfigDescription("Apply the mod's respawn and harvest fixes to berry bushes you've planted in your garden.", null,
                new ConfigurationManagerAttributes {Order = 9}));
        IncludeGardenTrees = Config.Bind(PlayerGardenSection, "Include Garden Trees", true,
            new ConfigDescription("Apply the mod's respawn and harvest fixes to fruit trees you've planted in your garden.", null,
                new ConfigurationManagerAttributes {Order = 8}));
        IncludeGardenBeeHives = Config.Bind(PlayerGardenSection, "Include Garden Bee Hives", false,
            new ConfigDescription("Apply the mod's respawn and harvest fixes to bee hives you've placed in your garden.", null,
                new ConfigurationManagerAttributes {Order = 7}));

        RealisticHarvest = Config.Bind(HarvestingSection, "Realistic Harvest", true,
            new ConfigDescription("Randomise the harvest amount and drop timing slightly so yields don't feel mechanical.", null,
                new ConfigurationManagerAttributes {Order = 6}));
        ShowHarvestReadyMessages = Config.Bind(HarvestingSection, "Show Harvest Ready Messages", true,
            new ConfigDescription("Show a small floating message when trees, bushes, or hives are ready to harvest.", null,
                new ConfigurationManagerAttributes {Order = 5}));

        IncludeWorldBerryBushes = Config.Bind(WorldEnvironmentSection, "Include World Berry Bushes", false,
            new ConfigDescription("Apply the mod's fixes to wild berry bushes out in the world, not just garden-planted ones. Not recommended without Where's Ma' Storage — world bushes produce large amounts of loose items.", null,
                new ConfigurationManagerAttributes {Order = 4}));

        BeeKeeperBuyback = Config.Bind(EconomySection, "Bee Keeper Buyback", false,
            new ConfigDescription("Let the beekeeper buy bees back from you, so surplus hives are worth selling.", null,
                new ConfigurationManagerAttributes {Order = 3}));

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose harvest and respawn diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {IsAdvanced = true, Order = 2}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }));
    }

    internal static void CleanUpTrees()
    {
        if (!MainGame.game_started) return;
        Log.LogInfo($"Running CleanUpTrees as Player has spawned in.");
        ProcessDudBees();
        ProcessDudTrees();
        ProcessDudBushes();
        ProcessReadyObjects();
    }

    private static void ProcessDudBees()
    {
        var dudBees = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Helpers.Constants.HarvestGrowing.BeeHouse).Where(b => b.progress <= 0)
            .Where(Helpers.IsPlayerBeeHive);

        var dudBeesCount = 0;
        foreach (var dudBee in dudBees)
        {
            dudBeesCount++;
            Helpers.ProcessBeeRespawn(dudBee);

            if (DebugEnabled)
            {
                Log.LogMessage($"Fixed DudBee {dudBeesCount}");
            }
        }
    }

    private static void ProcessDudTrees()
    {
        var dudTrees = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Helpers.Constants.HarvestGrowing.GardenAppleTree).Where(b => b.progress <= 0);

        var dudTreeCount = 0;
        foreach (var dudTree in dudTrees)
        {
            dudTreeCount++;
            Helpers.ProcessRespawn(dudTree, Helpers.Constants.HarvestGrowing.GardenAppleTree,
                Helpers.Constants.HarvestSpawner.GardenAppleTree);

            if (DebugEnabled)
            {
                Log.LogMessage($"Fixed DudGardenTree {dudTreeCount}");
            }
        }
    }

    private static void ProcessDudBushes()
    {
        var dudBushes = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Helpers.Constants.HarvestGrowing.GardenBerryBush).Where(b => b.progress <= 0);

        var dudBushCount = 0;
        foreach (var dudBush in dudBushes)
        {
            dudBushCount++;
            Helpers.ProcessRespawn(dudBush, Helpers.Constants.HarvestGrowing.GardenBerryBush,
                Helpers.Constants.HarvestSpawner.GardenBerryBush);

            if (DebugEnabled)
            {
                Log.LogMessage($"Fixed DudGardenBush {dudBushCount}");
            }
        }
    }

    private static void ProcessReadyObjects()
    {
        var readyBees = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Helpers.Constants.HarvestReady.BeeHouse)
            .Where(Helpers.IsPlayerBeeHive);
        var readyGardenTrees = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Helpers.Constants.HarvestReady.GardenAppleTree);
        var readyGardenBushes = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Helpers.Constants.HarvestReady.GardenBerryBush);
        var readyWorldBushes = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true).Where(a => Helpers.WorldReadyHarvests.Contains(a.obj_id));

        foreach (var item in readyBees)
        {
            Helpers.ProcessGardenBeeHive(item);
        }

        foreach (var item in readyGardenTrees)
        {
            Helpers.ProcessGardenAppleTree(item);
        }

        foreach (var item in readyGardenBushes)
        {
            Helpers.ProcessGardenBerryBush(item);
        }

        foreach (var item in readyWorldBushes)
        {
            switch (item.obj_id)
            {
                case Helpers.Constants.HarvestReady.WorldBerryBush1:
                    Helpers.ProcessBerryBush1(item);
                    break;

                case Helpers.Constants.HarvestReady.WorldBerryBush2:
                    Helpers.ProcessBerryBush2(item);
                    break;

                case Helpers.Constants.HarvestReady.WorldBerryBush3:
                    Helpers.ProcessBerryBush3(item);
                    break;
            }
        }
    }
}