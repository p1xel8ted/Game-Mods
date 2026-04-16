namespace TreesNoMore;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.treesnomore";
    internal const string PluginName = "Trees, No More!";
    private const string PluginVer = "2.5.12";

    // Section names. Numbered "── N. Name ──" so BepInEx ConfigurationManager renders them
    // in this exact order (CM uses Config.Bind call order, not alphabetic). Advanced renders
    // first because Debug is the very first Config.Bind call below.
    private const string AdvancedSection = "── 1. Advanced ──";
    private const string TreesSection    = "── 2. Trees ──";
    private const string StumpsSection   = "── 3. Stumps ──";
    private const string ResetSection    = "── 4. Reset ──";

    // Maps the legacy 2.5.11-and-earlier section names to the new "── N. Name ──" form so
    // existing user values survive the rename. Idempotent — once migrated there are no old
    // headers left for the next launch to match.
    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["01. Trees"]    = TreesSection,
        ["02. Stumps"]   = StumpsSection,
        ["03. Reset"]    = ResetSection,
    };

    private static bool ShowConfirmationDialog { get; set; }
    internal static ManualLogSource Log { get; private set; }

    internal static List<Tree> Trees { get; private set; } = [];

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ConfigEntry<int> TreeSearchDistance { get; private set; }
    internal static ConfigEntry<bool> InstantStumpRemoval { get; private set; }
    private static string FilePath => Path.Combine(Application.persistentDataPath, $"{MainGame.me.save_slot.filename_no_extension}_trees.json");

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Application.quitting += SaveTrees;
    }

    // Rewrites legacy "[00. Advanced]" style headers to "[── 1. Advanced ──]" in the .cfg
    // file so existing user values survive the section rename. Idempotent.
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

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── N. Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        // ── 1. Advanced ──
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription(
                "Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Trees ──
        TreeSearchDistance = Config.Bind(TreesSection, "Tree Search Distance", 2,
            new ConfigDescription(
                "How far around a remembered tree to look when deciding whether the world copy should be removed. Default 2 catches the same tree without grabbing its neighbours. Raise it only if felled trees re-spawn after a reload; values that are too high will start removing trees you never chopped.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 3. Stumps ──
        InstantStumpRemoval = Config.Bind(StumpsSection, "Instant Stump Removal", true,
            new ConfigDescription(
                "On: stumps disappear the moment a tree falls — no second 'mine the stump' step. Off: stumps stay in the world until you remove them yourself with the right tool.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 4. Reset ──
        Config.Bind(ResetSection, "Reset Trees", true,
            new ConfigDescription(
                "Restore every felled tree on the next game launch. The mod's record of which trees you've chopped is wiped, so on next load the world spawns them all back. Useful if you want a fresh map or accidentally tracked the wrong objects.",
                null,
                new ConfigurationManagerAttributes {HideDefaultButton = true, Order = 100, CustomDrawer = RestoreTrees}));
    }

    private static void RestoreTrees(ConfigEntryBase entry)
    {
        if (ShowConfirmationDialog)
        {
            GUILayout.Label(Lang.Get("ConfirmText"));
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(Lang.Get("Yes"), GUILayout.ExpandWidth(true)))
                {
                    if (Plugin.DebugEnabled) Helpers.Log($"[Reset] User confirmed — clearing {Trees.Count} tracked tree(s) and deleting {FilePath}");
                    Trees.Clear();
                    File.Delete(FilePath);
                    ShowConfirmationDialog = false;
                }

                if (GUILayout.Button(Lang.Get("No"), GUILayout.ExpandWidth(true)))
                {
                    ShowConfirmationDialog = false;
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button(Lang.Get("ResetButton"), GUILayout.ExpandWidth(true)))
            {
                ShowConfirmationDialog = true;
            }
        }
    }

    internal static bool LoadTrees()
    {
        if (MainGame.me.save_slot.linked_save == null)
        {
            if (DebugEnabled) Helpers.Log("[LoadTrees] save_slot.linked_save is null — nothing to load");
            return false;
        }

        if (!File.Exists(FilePath))
        {
            if (DebugEnabled) Helpers.Log($"[LoadTrees] no state file at {FilePath} — starting with empty tracked-tree list");
            return false;
        }
        var jsonString = File.ReadAllText(FilePath);
        Trees = JsonConvert.DeserializeObject<List<Tree>>(jsonString);
        if (DebugEnabled) Helpers.Log($"[LoadTrees] loaded {Trees.Count} tracked tree(s) from {FilePath}");
        return true;
    }

    internal static void SaveTrees()
    {
        if (MainGame.me.save_slot.linked_save == null)
        {
            if (DebugEnabled) Helpers.Log("[SaveTrees] save_slot.linked_save is null — skipping save");
            return;
        }
        var seen = new HashSet<Vector3>();
        var count = Trees.RemoveAll(x => !seen.Add(x.location));
        if (count > 0 && DebugEnabled)
        {
            Helpers.Log($"[SaveTrees] removed {count} duplicate tree entry/entries before write");
        }
        var jsonString = JsonConvert.SerializeObject(Trees, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });

        File.WriteAllText(FilePath, jsonString);
        if (DebugEnabled) Helpers.Log($"[SaveTrees] wrote {Trees.Count} tracked tree(s) to {FilePath}");
    }

}
