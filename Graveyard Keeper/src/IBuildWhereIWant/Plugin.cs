namespace IBuildWhereIWant;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.ibuildwhereiwant";
    private const string PluginName = "I Build Where I Want!";
    private const string PluginVer = "1.7.11";

    private const string Zone = "mf_wood";
    private const string BuildDeskConst = "buildanywhere_desk";

    internal static ManualLogSource Log { get; private set; }
    internal static ConfigEntry<bool> Grid { get; private set; }
    internal static ConfigEntry<bool> GreyOverlay { get; private set; }
    internal static ConfigEntry<bool> BuildingCollision { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ConfigEntry<KeyboardShortcut> MenuKeyBind { get; private set; }
    internal static ConfigEntry<string> MenuControllerButton { get; private set; }

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
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging for troubleshooting purposes.", null, new ConfigurationManagerAttributes {Order = 599}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        BuildingCollision = Config.Bind("01. Collision", "Building Collision", true, new ConfigDescription("Toggle collision between buildings to place them closer together (or on top of each other...)", null, new ConfigurationManagerAttributes {Order = 604}));

        Grid = Config.Bind("02. Display", "Grid", false, new ConfigDescription("Toggle the grid overlay from the building interface for a cleaner look.", null, new ConfigurationManagerAttributes {Order = 603}));

        GreyOverlay = Config.Bind("02. Display", "Grey Overlay", false, new ConfigDescription("Toggle the grey overlay that appears when removing objects in the building interface.", null, new ConfigurationManagerAttributes {Order = 602}));

        MenuKeyBind = Config.Bind("03. Keybinds", "Menu Key Bind", new KeyboardShortcut(KeyCode.Q), new ConfigDescription("Define the key used to open the mod menu.", null, new ConfigurationManagerAttributes {Order = 601}));

        MenuControllerButton = Config.Bind("04. Controller", "Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.LB), new ConfigDescription("Select the controller button used to open the mod menu.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 600}));
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

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
