namespace TreesNoMore;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.treesnomore";
    private const string PluginName = "Trees, No More!";
    private const string PluginVer = "2.5.6";
    private static bool ShowConfirmationDialog { get; set; }
    internal static ManualLogSource Log { get; private set; }

    internal static List<Tree> Trees { get; private set; } = [];

    internal static ConfigEntry<int> TreeSearchDistance { get; private set; }
    internal static ConfigEntry<bool> InstantStumpRemoval { get; private set; }
    private static string FilePath => Path.Combine(Application.persistentDataPath, $"{MainGame.me.save_slot.filename_no_extension}_trees.json");

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Actions.GameStartedPlaying += Patches.DestroyTress;
        Actions.ReturnToMenu += SaveTrees;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void InitConfiguration()
    {
        TreeSearchDistance = Config.Bind("01. Trees", "Tree Search Distance", 2, new ConfigDescription("The allowable distance to check if a tree shouldn't exist on load. The default value of 2 seems to work well. Setting this too large may cause trees surrounding the intended tree to also be removed.", null, new ConfigurationManagerAttributes {Order = 2}));

        InstantStumpRemoval = Config.Bind("02. Stumps", "Instant Stump Removal", true, new ConfigDescription("Instantly remove stumps when chopping down trees.", null, new ConfigurationManagerAttributes {Order = 1}));

        Config.Bind("03. Reset", "Reset Trees", true, new ConfigDescription("All felled trees will be restored on restart.", null, new ConfigurationManagerAttributes {HideDefaultButton = true, Order = 0, CustomDrawer = RestoreTrees}));
    }


    private static void RestoreTrees(ConfigEntryBase entry)
    {
        if (ShowConfirmationDialog)
        {
            Tools.DisplayConfirmationDialog("Are you sure you want to reset all trees?", () =>
            {
                Log.LogWarning("All felled trees will be restored on restart.");
                Trees.Clear();
                File.Delete(FilePath);
                ShowConfirmationDialog = false;
            }, () => ShowConfirmationDialog = false);
        }
        else
        {
            var button = GUILayout.Button("Reset Trees", GUILayout.ExpandWidth(true));
            if (button)
            {
                ShowConfirmationDialog = true;
            }
        }
    }

    internal static bool LoadTrees()
    {
        if (MainGame.me.save_slot.linked_save == null) return false;

        if (!File.Exists(FilePath)) return false;
        var jsonString = File.ReadAllText(FilePath);
        Trees = JsonConvert.DeserializeObject<List<Tree>>(jsonString);
        Log.LogWarning($"Loaded {Trees.Count} trees from {FilePath}");
        return true;
    }

    internal static void SaveTrees()
    {
        if (MainGame.me.save_slot.linked_save == null) return;
        //remove near enough duplicates from Trees list
        var count = Trees.RemoveAll(x => Trees.FindAll(y => y.location == x.location).Count > 1);
        Log.LogWarning($"Removed {count} duplicate trees");
        var jsonString = JsonConvert.SerializeObject(Trees, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });

        File.WriteAllText(FilePath, jsonString);
        Log.LogWarning($"Saved {Trees.Count} trees to {FilePath}");
    }

    private void OnDisable()
    {
        SaveTrees();
    }

    private void OnDestroy()
    {
        SaveTrees();
    }
}