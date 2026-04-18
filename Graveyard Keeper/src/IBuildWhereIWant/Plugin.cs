namespace IBuildWhereIWant;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection   = "── Advanced ──";
    private const string CollisionSection  = "── Collision ──";
    private const string DisplaySection    = "── Display ──";
    private const string KeybindsSection   = "── Keybinds ──";
    private const string ControllerSection = "── Controller ──";
    private const string UpdatesSection    = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]   = AdvancedSection,
        ["01. Collision"]  = CollisionSection,
        ["02. Display"]    = DisplaySection,
        ["03. Keybinds"]   = KeybindsSection,
        ["04. Controller"] = ControllerSection,
    };

    private const string Zone = "mf_wood";
    private const string BuildDeskConst = "buildanywhere_desk";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> Grid { get; private set; }
    internal static ConfigEntry<bool> GreyOverlay { get; private set; }
    internal static ConfigEntry<bool> BuildingCollision { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static ConfigEntry<KeyboardShortcut> MenuKeyBind { get; private set; }
    internal static ConfigEntry<string> MenuControllerButton { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static WorldGameObject BuildDesk { get; set; }
    internal static WorldGameObject BuildDeskClone { get; set; }
    internal static CraftsInventory CraftsInventory { get; set; }
    internal static Dictionary<string, string> CraftDictionary { get; set; } = new();
    internal static int UnlockedCraftListCount { get; set; }
    internal static bool CraftAnywhere { get; set; }
    internal static string ZoneId => Zone;
    internal static string BuildDeskName => BuildDeskConst;

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
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose build-menu diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 599}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        BuildingCollision = Config.Bind(CollisionSection, "Building Collision", true,
            new ConfigDescription("Enforce collision between buildings. Turn off to let placed structures overlap — useful for tight layouts, handy if you like stacking things.", null,
                new ConfigurationManagerAttributes {Order = 604}));

        Grid = Config.Bind(DisplaySection, "Grid", false,
            new ConfigDescription("Show the build-mode grid overlay while placing structures.", null,
                new ConfigurationManagerAttributes {Order = 603}));

        GreyOverlay = Config.Bind(DisplaySection, "Grey Overlay", false,
            new ConfigDescription("Show the grey removal-mode overlay when tearing structures down.", null,
                new ConfigurationManagerAttributes {Order = 602}));

        MenuKeyBind = Config.Bind(KeybindsSection, "Menu Key Bind", new KeyboardShortcut(KeyCode.Q),
            new ConfigDescription("Keybind for opening the build-anywhere crafts menu.", null,
                new ConfigurationManagerAttributes {Order = 601}));

        MenuControllerButton = Config.Bind(ControllerSection, "Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.LB),
            new ConfigDescription("Gamepad button for opening the build-anywhere crafts menu.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 600}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 0 }));
    }

    internal static bool CanOpenCraftAnywhere()
    {
        return MainGame.game_started && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() &&
               !MainGame.paused && BaseGUI.all_guis_closed &&
               !MainGame.me.player.GetMyWorldZoneId().Contains("refugee");
    }

    internal static void OpenCraftAnywhere()
    {
        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
        if (MainGame.me.player.GetParamInt("in_tutorial") == 1 &&
            MainGame.me.player.GetParamInt("tut_shown_tut_1") == 0)
        {
            MainGame.me.player.Say("cant_do_it_now");
            return;
        }

        CraftsInventory ??= new CraftsInventory();

        CraftDictionary ??= new Dictionary<string, string>();

        if (BuildDesk == null)
        {
            BuildDesk = UnityEngine.Object.FindObjectsOfType<WorldGameObject>(true)
                .FirstOrDefault(x => string.Equals(x.obj_id, "mf_wood_builddesk"));
        }

        WriteLog(
            BuildDesk != null
                ? $"Found Build Desk: {BuildDesk}, Zone: {BuildDesk.GetMyWorldZone()}"
                : "Unable to locate a build desk.", BuildDesk == null);

        if (BuildDeskClone != null)
        {
            UnityEngine.Object.Destroy(BuildDeskClone);
        }

        BuildDeskClone = UnityEngine.Object.Instantiate(BuildDesk);

        BuildDeskClone.name = BuildDeskConst;

        var needsRefresh = false;
        if (MainGame.me.save.unlocked_crafts.Count > UnlockedCraftListCount)
        {
            UnlockedCraftListCount = MainGame.me.save.unlocked_crafts.Count;
            needsRefresh = true;
        }

        if (needsRefresh)
        {
            foreach (var objectCraftDefinition in GameBalance.me.craft_obj_data.Where(x =>
                             x.build_type == ObjectCraftDefinition.BuildType.Put)
                         .Where(a => a.icon.Length > 0)
                         .Where(b => !b.id.Contains("refugee"))
                         .Where(d => MainGame.me.save.IsCraftVisible(d))
                         .Where(e => !CraftDictionary.TryGetValue(GJL.L(e.GetNameNonLocalized()), out _)))
            {
                var itemName = GJL.L(objectCraftDefinition.GetNameNonLocalized());
                CraftDictionary.Add(itemName, objectCraftDefinition.id);
            }

            CraftsInventory = new CraftsInventory();

            var craftList = CraftDictionary.ToList();
            craftList.Sort((pair1, pair2) => string.CompareOrdinal(pair1.Key, pair2.Key));

            foreach (var craft in craftList)
            {
                CraftsInventory.AddCraft(craft.Value);
            }
        }

        CraftAnywhere = true;

        BuildModeLogics.last_build_desk = BuildDeskClone;

        MainGame.me.build_mode_logics.SetCurrentBuildZone(BuildDeskClone.obj_def.zone_id, "");
        GUIElements.me.craft.OpenAsBuild(BuildDeskClone, CraftsInventory);
        MainGame.paused = false;
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

}
