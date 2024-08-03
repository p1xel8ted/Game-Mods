namespace BeamMeUpGerry;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.beammeupgerryrewrite";
    private const string PluginName = "Beam Me Up Gerry!";
    private const string PluginVer = "3.0.7";

    internal static ConfigEntry<bool> DebugEnabled { get; private set; }
    internal static ConfigEntry<bool> IncreaseMenuAnimationSpeed { get; private set; }
    internal static ConfigEntry<bool> EnableListExpansion { get; private set; }
    internal static ConfigEntry<bool> GerryAppears { get; private set; }
    internal static ConfigEntry<bool> GerryCharges { get; private set; }
    internal static ConfigEntry<bool> EnablePreviousPageChoices { get; private set; }
    internal static ConfigEntry<bool> PreviousPageChoiceAtTop { get; private set; }
    private static ConfigEntry<KeyboardShortcut> TeleportMenuKeyBind { get; set; }
    private static ConfigEntry<string> TeleportMenuControllerButton { get; set; }
    internal static ConfigEntry<int> LocationsPerPage { get; private set; }
    internal static ConfigEntry<bool> SortAlphabetically { get; private set; }
    private static ConfigEntry<bool> EnableCustomLocations { get; set; }
    internal static ConfigEntry<bool> OpenNewLocationFileOnSave { get; private set; }
    private static ConfigEntry<bool> CustomLocationMessage { get; set; }
    private static ConfigEntry<KeyboardShortcut> SaveCustomLocationKeybind { get; set; }
    private static ConfigEntry<KeyboardShortcut> ReloadCustomLocationsKeybind { get; set; }
    private static ConfigEntry<string> SaveCustomLocationControllerButton { get; set; }

    internal static ManualLogSource Log { get; private set; }

    internal static Player CachedPlayer { get; set; }
    private Item CachedHearthstone { get; set; }

    private static ConfigFile ConfigInstance { get; set; }

    private void Awake()
    {
        ConfigInstance = Config;
        Log = Logger;
        InitConfiguration();
        InitInternalConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void OnDestroy()
    {
        Log.LogError($"Plugin {PluginName} is unloading!");
    }

    private void OnDisable()
    {
        Log.LogError($"Plugin {PluginName} is being disabled!");
    }

    private void Update()
    {
        if (!Helpers.IsUpdateConditionsMet()) return;

        CachedPlayer ??= ReInput.players.GetPlayer(0);

        if (EnableCustomLocations.Value)
        {
            if (SaveCustomLocationKeybind.Value.IsUp() || LazyInput.gamepad_active && CachedPlayer.GetButtonDown(SaveCustomLocationControllerButton.Value))
            {
                StartCoroutine(Helpers.LogPosition(InitConfiguration));
                if (!CustomLocationMessage.Value)
                {
                    Tools.ShowAlertDialog("Beam Me Up Gerry!", "You have just saved your first custom location! Please ensure you open the saved file (Locations folder) and change the 'zone' name to something proper!", string.Empty, true);
                    CustomLocationMessage.Value = true;
                }
            }

            if (ReloadCustomLocationsKeybind.Value.IsUp())
            {
                LocationLists.LoadCustomZones();
                UpdateLists(true);
            }
        }

        var shouldHandleTeleport = LazyInput.gamepad_active && CachedPlayer.GetButtonDown(TeleportMenuControllerButton.Value) || TeleportMenuKeyBind.Value.IsUp();

        if (!shouldHandleTeleport || Tools.PlayerDisabled() || Helpers.InTutorial()) return;


        CachedHearthstone ??= Helpers.GetHearthstone();

        if (CachedHearthstone != null)
        {
            if (CrossModFields.IsInDungeon)
            {
                Helpers.SpawnGerry(strings.CantUseHere, Vector3.zero);
            }
            else
            {
                if (EnableListExpansion.Value)
                {
                    LocationLists.CreatePages();
                }

                CachedHearthstone.UseItem(MainGame.me.player);
            }
        }
        else
        {
            Helpers.SpawnGerry(strings.WhereIsIt, Vector3.zero);
        }
    }

    private void InitInternalConfiguration()
    {
        CustomLocationMessage = Config.Bind("Internal (Dont Touch)", "Custom Location Shown", false, new ConfigDescription("Internal use. Used for tracking if the custom location alert has been shown.", null, new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 6}));
        Config.Bind("Print Known", "Print Known", false, new ConfigDescription("Click to output known zones to log.", null, new ConfigurationManagerAttributes {CustomDrawer = PrintKnown, HideDefaultButton = true, Order = 5}));
    }
    private static void PrintKnown(ConfigEntryBase __obj)
    {
        var button = GUILayout.Button("Print Known Zones, NPC's & One-Time Crafts", GUILayout.ExpandWidth(true));
        if (button)
        {
            Log.LogInfo("\n");
            Log.LogInfo("Known NPC:");
            foreach (var npc in MainGame.me.save.known_npcs.npcs)
            {
                Log.LogInfo(npc.npc_id);
            }
            Log.LogInfo("\n");
            Log.LogInfo("Known Zones:");
            foreach (var zone in MainGame.me.save.known_world_zones)
            {
                Log.LogInfo(zone);
            }
            Log.LogInfo("\n");
            Log.LogInfo("One-Time Crafts:");
            foreach (var craft in MainGame.me.save.completed_one_time_crafts)
            {
                Log.LogInfo(craft);
            }
            Log.LogInfo("\n");
        }
    }

    internal static void InitConfiguration()
    {
        LocationLists.LoadCustomZones();

        DebugEnabled = ConfigInstance.Bind("00. Debug", "Debug", false, new ConfigDescription("Toggle debug logging", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 803}));
        //ModEnabled = Config.Bind("01. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 802}));
        IncreaseMenuAnimationSpeed = ConfigInstance.Bind("01. Features", "Increase Menu Animation Speed", true, new ConfigDescription("Toggle increased menu animation speed", null, new ConfigurationManagerAttributes {Order = 801}));
        EnableListExpansion = ConfigInstance.Bind("01. Features", "Enable List Expansion", true, new ConfigDescription("Toggle list expansion functionality", null, new ConfigurationManagerAttributes {Order = 799}));
        GerryAppears = ConfigInstance.Bind("01. Features", "Gerry Appears", false, new ConfigDescription("Toggle Gerry's presence", null, new ConfigurationManagerAttributes {Order = 798}));
        GerryCharges = ConfigInstance.Bind("01. Features", "Gerry Charges", false, new ConfigDescription("Toggle the cost of teleporting", null, new ConfigurationManagerAttributes {Order = 797}));
        EnablePreviousPageChoices = ConfigInstance.Bind("01. Features", "Enable Previous Page Choices", true, new ConfigDescription("Toggle the ability to go back to the previous page", null, new ConfigurationManagerAttributes {Order = 796}));
        PreviousPageChoiceAtTop = ConfigInstance.Bind("01. Features", "Previous Page Choice At Top", true, new ConfigDescription("Toggle the placement of the previous page choice at the top of the list", null, new ConfigurationManagerAttributes {Order = 795}));
        TeleportMenuKeyBind = ConfigInstance.Bind("02. Keybinds", "Teleport Menu Keybind", new KeyboardShortcut(KeyCode.Z), new ConfigDescription("Set the keybind for opening the teleport menu", null, new ConfigurationManagerAttributes {Order = 796}));
        TeleportMenuControllerButton = ConfigInstance.Bind("03. Controller", "Teleport Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.RB), new ConfigDescription("Set the controller button for opening the teleport menu", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 795}));

        var maxPages = Constants.MaxPages;
        var minPages = Mathf.CeilToInt(LocationLists.AllLocations.Count / (float) Constants.MaxPages);

        if (minPages >= Constants.MaxPages)
        {
            maxPages = minPages + 1;
        }

        Log.LogInfo($"Min Pages: {minPages} | Max Pages: {maxPages}");

        LocationsPerPage = ConfigInstance.Bind("04. Locations", "Locations Per Page", 8, new ConfigDescription("Set the number of locations to display per page", new AcceptableValueRange<int>(minPages, maxPages), new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 794}));
        SortAlphabetically = ConfigInstance.Bind("04. Locations", "Sort Alphabetically", false, new ConfigDescription("Toggle alphabetical sorting of locations", null, new ConfigurationManagerAttributes {Order = 793}));
        EnableCustomLocations = ConfigInstance.Bind("05. Custom Locations", "Enable Custom Locations", false, new ConfigDescription("Toggle the ability to save & load custom locations.", null, new ConfigurationManagerAttributes {Order = 792}));
        SaveCustomLocationKeybind = ConfigInstance.Bind("05. Custom Locations", "Save Custom Location Keybind", new KeyboardShortcut(KeyCode.X), new ConfigDescription("Set the keybind for saving custom locations.", null, new ConfigurationManagerAttributes {Order = 791}));
        ReloadCustomLocationsKeybind = ConfigInstance.Bind("05. Custom Locations", "Reload Custom Locations Keybind", new KeyboardShortcut(KeyCode.X, KeyCode.LeftControl), new ConfigDescription("Set the keybind for reloading custom locations.", null, new ConfigurationManagerAttributes {Order = 790}));
        SaveCustomLocationControllerButton = ConfigInstance.Bind("05. Custom Locations", "Save Custom Location Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.None), new ConfigDescription("Set the controller button for saving custom locations.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 790}));
        OpenNewLocationFileOnSave = ConfigInstance.Bind("05. Custom Locations", "Open New Location File On Save", true, new ConfigDescription("Toggle the ability to open the new location file after saving.", null, new ConfigurationManagerAttributes {Order = 789}));

        UpdateLists();
    }


    internal static void UpdateLists(bool forceReload = false)
    {
        if (forceReload)
        {
            Plugin.Log.LogInfo("Force reloading custom locations.");
        }
        var originalLanguage = GameSettings._cur_lng;
        GJL.LoadLanguageResource("en");
        foreach (var location in LocationLists.AllLocations.OrderByDescending(a => Helpers.RemoveCharacters(a.zone)))
        {
            if (location.zone.Equals(strings.Page_1) || location.zone.Equals(strings.Page_2) || location.zone.Equals(strings.Page_3) || location.zone.Equals(strings.Custom_Locations) || location.zone.Equals(Constants.Cancel)) continue;
            var key = Helpers.RemoveCharacters(location.zone);
            var configEntry = ConfigInstance.Bind("5. Locations", key, true, $"Toggle visibility of {key} in the menu.");
            location.enabled = configEntry.Value;
            configEntry.SettingChanged += (_, _) =>
            {
                LocationLists.CreatePages();
                LocationLists.AllLocations.Find(a => a.zone == location.zone).enabled = configEntry.Value;
            };
        }

        if (!originalLanguage.IsNullOrWhiteSpace())
        {
            GJL.LoadLanguageResource(originalLanguage);
        }

        LocationLists.CreatePages();
    }
}