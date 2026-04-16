namespace AppleTreesEnhanced;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.appletreesenhanced";
    private const string PluginName = "Apple Tree's Enhanced!";
    private const string PluginVer = "2.7.13";
    internal static ManualLogSource Log { get; private set; }

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> IncludeGardenBerryBushes { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenTrees { get; private set; }
    internal static ConfigEntry<bool> IncludeWorldBerryBushes { get; private set; }
    internal static ConfigEntry<bool> ShowHarvestReadyMessages { get; private set; }
    internal static ConfigEntry<bool> RealisticHarvest { get; private set; }
    internal static ConfigEntry<bool> IncludeGardenBeeHives { get; private set; }
    internal static ConfigEntry<bool> BeeKeeperBuyback { get; private set; }


    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        IncludeGardenBerryBushes = Config.Bind("1. Player Garden", "Include Garden Berry Bushes", true, new ConfigDescription("Enable enhancements for player garden berry bushes", null, new ConfigurationManagerAttributes {Order = 9}));
        IncludeGardenTrees = Config.Bind("1. Player Garden", "Include Garden Trees", true, new ConfigDescription("Enable enhancements for player garden trees", null, new ConfigurationManagerAttributes {Order = 8}));
        IncludeGardenBeeHives = Config.Bind("1. Player Garden", "Include Garden Bee Hives", false, new ConfigDescription("Enable enhancements for player garden bee hives", null, new ConfigurationManagerAttributes {Order = 7}));

        RealisticHarvest = Config.Bind("2. Harvesting", "Realistic Harvest", true, new ConfigDescription("Enable randomization of harvest amounts and drop time", null, new ConfigurationManagerAttributes {Order = 6}));
        ShowHarvestReadyMessages = Config.Bind("2. Harvesting", "Show Harvest Ready Messages", true, new ConfigDescription("Display messages when harvest is ready", null, new ConfigurationManagerAttributes {Order = 5}));

        IncludeWorldBerryBushes = Config.Bind("3. World Environment", "Include World Berry Bushes", false, new ConfigDescription("Enable enhancements for world berry bushes (not recommended without Wheres Ma Storage)", null, new ConfigurationManagerAttributes {Order = 4}));

        BeeKeeperBuyback = Config.Bind("4. Economy", "Bee Keeper Buyback", false, new ConfigDescription("Allow beekeeper to buy back bees", null, new ConfigurationManagerAttributes {Order = 3}));

        Debug = Config.Bind("5. Advanced", "Debug Logging", false, new ConfigDescription("Toggle debug logging on or off", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 2}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
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