namespace BeamMeUpGerry;

[Harmony]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection        = "── Advanced ──";
    private const string FeaturesSection        = "── Features ──";
    private const string KeybindsSection        = "── Keybinds ──";
    private const string ControllerSection      = "── Controller ──";
    private const string LocationsSection       = "── Locations ──";
    private const string CustomLocationsSection = "── Custom Locations ──";
    private const string UpdatesSection         = "── Updates ──";

    // Old section names get rewritten to the new "── Name ──" style on first launch so
    // existing user customisations are preserved. Idempotent.
    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Debug"]              = AdvancedSection,
        ["Internal (Dont Touch)"]  = AdvancedSection,
        ["Print Known"]            = AdvancedSection,
        ["01. Features"]           = FeaturesSection,
        ["02. Keybinds"]           = KeybindsSection,
        ["03. Controller"]         = ControllerSection,
        ["04. Locations"]          = LocationsSection,
        ["5. Locations"]           = LocationsSection,
        ["05. Custom Locations"]   = CustomLocationsSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static ConfigEntry<bool> IncreaseMenuAnimationSpeed { get; private set; }
    internal static ConfigEntry<bool> EnableListExpansion { get; private set; }
    internal static ConfigEntry<bool> GerryAppears { get; private set; }
    internal static ConfigEntry<bool> GerryCharges { get; private set; }

    internal static ConfigEntry<bool> CinematicEffect { get; private set; }
    internal static ConfigEntry<bool> EnablePreviousPageChoices { get; private set; }
    internal static ConfigEntry<bool> PreviousPageChoiceAtTop { get; private set; }
    internal static ConfigEntry<KeyboardShortcut> TeleportMenuKeyBind { get; set; }
    internal static ConfigEntry<string> TeleportMenuControllerButton { get; set; }
    internal static ConfigEntry<int> LocationsPerPage { get; private set; }
    internal static ConfigEntry<bool> SortAlphabetically { get; private set; }
    internal static ConfigEntry<bool> EnableCustomLocations { get; set; }
    internal static ConfigEntry<bool> RestrictToFoundLocations { get; set; }
    internal static ConfigEntry<bool> OpenNewLocationFileOnSave { get; private set; }
    internal static ConfigEntry<bool> CustomLocationMessage { get; set; }
    internal static ConfigEntry<KeyboardShortcut> SaveCustomLocationKeybind { get; set; }
    internal static ConfigEntry<KeyboardShortcut> ReloadCustomLocationsKeybind { get; set; }
    internal static ConfigEntry<string> SaveCustomLocationControllerButton { get; set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    internal static Player CachedPlayer { get; set; }
    internal static Item CachedHearthstone { get; set; }

    private static ConfigFile ConfigInstance { get; set; }

    private void Awake()
    {
        ConfigInstance = Config;
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        InitInternalConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites old numbered "[00. Debug]" / "[01. Features]" headers to "[── Name ──]" in the .cfg
    // so existing user values survive the section rename. Idempotent — re-running on an
    // already-migrated file is a no-op.
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

    private void InitInternalConfiguration()
    {
        CustomLocationMessage = Config.Bind(AdvancedSection, "Custom Location Shown", false,
            new ConfigDescription("Internal: tracks whether the one-time 'Custom Locations were enabled' notice has already been shown.", null,
                new ConfigurationManagerAttributes {Browsable = false, HideDefaultButton = true, IsAdvanced = true, ReadOnly = true, Order = 6}));

        Config.Bind(AdvancedSection, "Print Known", false,
            new ConfigDescription("Click to dump known NPCs, zones, one-time crafts, and unlocked/blacklisted phrases to the log. Handy for diagnostics.", null,
                new ConfigurationManagerAttributes {CustomDrawer = PrintKnown, HideDefaultButton = true, Order = 5}));
    }
    private static void PrintKnown(ConfigEntryBase __obj)
    {
        var button = GUILayout.Button("Print Save Data", GUILayout.ExpandWidth(true));
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
            Log.LogInfo("Unlocked Phrases:");
            foreach (var phrase in MainGame.me.save.unlocked_phrases)
            {
                Log.LogInfo(phrase);
            }
            Log.LogInfo("\n");
            Log.LogInfo("Blacklisted Phrases:");
            foreach (var phrase in MainGame.me.save.black_list_of_phrases)
            {
                Log.LogInfo(phrase);
            }
            Log.LogInfo("\n");
        }
    }

    internal static void InitConfiguration()
    {
        LocationLists.LoadCustomZones();

        Debug = ConfigInstance.Bind(AdvancedSection, "Debug", false,
            new ConfigDescription("Write verbose teleport and location diagnostics to the BepInEx console. Leave off for normal play.",
                null,
                new ConfigurationManagerAttributes {Order = 803}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        IncreaseMenuAnimationSpeed = ConfigInstance.Bind(FeaturesSection, "Increase Menu Animation Speed", true,
            new ConfigDescription("Speed up the teleport menu's open and close animation for snappier navigation.", null,
                new ConfigurationManagerAttributes {Order = 801}));

        EnableListExpansion = ConfigInstance.Bind(FeaturesSection, "Enable List Expansion", true,
            new ConfigDescription("Expand the visible locations list so more locations fit on screen at once.", null,
                new ConfigurationManagerAttributes {Order = 799}));

        GerryAppears = ConfigInstance.Bind(FeaturesSection, "Gerry Appears", false,
            new ConfigDescription("When on, Gerry appears at each teleport destination as a little story beat. Off for silent instant teleports.", null,
                new ConfigurationManagerAttributes {Order = 798}));

        CinematicEffect = ConfigInstance.Bind(FeaturesSection, "Cinematic Effect", true,
            new ConfigDescription("Play a brief fade effect when teleporting.", null,
                new ConfigurationManagerAttributes {Order = 797}));

        GerryCharges = ConfigInstance.Bind(FeaturesSection, "Gerry Charges", false,
            new ConfigDescription("Charge money for each non-starter teleport. Turn off to make teleports free.", null,
                new ConfigurationManagerAttributes {Order = 796}));

        EnablePreviousPageChoices = ConfigInstance.Bind(FeaturesSection, "Enable Previous Page Choices", true,
            new ConfigDescription("Show a back-to-previous-page entry in the teleport menu.", null,
                new ConfigurationManagerAttributes {Order = 795}));

        PreviousPageChoiceAtTop = ConfigInstance.Bind(FeaturesSection, "Previous Page Choice At Top", true,
            new ConfigDescription("Put the back-to-previous-page entry at the top of the list instead of the bottom.", null,
                new ConfigurationManagerAttributes {Order = 794, DispName = "    └ Previous Page Choice At Top"}));

        TeleportMenuKeyBind = ConfigInstance.Bind(KeybindsSection, "Teleport Menu Keybind", new KeyboardShortcut(KeyCode.Z),
            new ConfigDescription("Keybind for opening the teleport menu.", null,
                new ConfigurationManagerAttributes {Order = 796}));

        TeleportMenuControllerButton = ConfigInstance.Bind(ControllerSection, "Teleport Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.RB),
            new ConfigDescription("Gamepad button for opening the teleport menu.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 795}));

        var maxPages = Constants.MaxPages;
        var minPages = Mathf.CeilToInt(LocationLists.AllLocations.Count / (float) Constants.MaxPages);

        if (minPages >= Constants.MaxPages)
        {
            maxPages = minPages + 1;
        }

        Log.LogInfo($"Min Pages: {minPages} | Max Pages: {maxPages}");

        LocationsPerPage = ConfigInstance.Bind(LocationsSection, "Locations Per Page", 8,
            new ConfigDescription("How many locations to show per page in the teleport menu.",
                new AcceptableValueRange<int>(minPages, maxPages),
                new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 794}));

        SortAlphabetically = ConfigInstance.Bind(LocationsSection, "Sort Alphabetically", false,
            new ConfigDescription("Sort visible locations alphabetically. Off uses the game's default order.", null,
                new ConfigurationManagerAttributes {Order = 793}));

        RestrictToFoundLocations = ConfigInstance.Bind(LocationsSection, "Restrict To Found Locations", true,
            new ConfigDescription("Only show locations you've already discovered. Off shows every location regardless of progress.", null,
                new ConfigurationManagerAttributes {Order = 792}));

        EnableCustomLocations = ConfigInstance.Bind(CustomLocationsSection, "Enable Custom Locations", false,
            new ConfigDescription("Allow saving and loading custom teleport points at your current position.", null,
                new ConfigurationManagerAttributes {Order = 792}));

        SaveCustomLocationKeybind = ConfigInstance.Bind(CustomLocationsSection, "Save Custom Location Keybind", new KeyboardShortcut(KeyCode.X),
            new ConfigDescription("Keybind for saving the current spot as a custom teleport location.", null,
                new ConfigurationManagerAttributes {Order = 791, DispName = "    └ Save Custom Location Keybind"}));

        ReloadCustomLocationsKeybind = ConfigInstance.Bind(CustomLocationsSection, "Reload Custom Locations Keybind", new KeyboardShortcut(KeyCode.X, KeyCode.LeftControl),
            new ConfigDescription("Keybind for reloading the custom locations file from disk after editing it externally.", null,
                new ConfigurationManagerAttributes {Order = 790, DispName = "    └ Reload Custom Locations Keybind"}));

        SaveCustomLocationControllerButton = ConfigInstance.Bind(CustomLocationsSection, "Save Custom Location Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.None),
            new ConfigDescription("Gamepad button for saving the current spot as a custom teleport location.",
                new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))),
                new ConfigurationManagerAttributes {Order = 789, DispName = "    └ Save Custom Location Controller Button"}));

        OpenNewLocationFileOnSave = ConfigInstance.Bind(CustomLocationsSection, "Open New Location File On Save", true,
            new ConfigDescription("Open the custom locations file in your text editor after saving so you can name the new entry.", null,
                new ConfigurationManagerAttributes {Order = 788, DispName = "    └ Open New Location File On Save"}));

        CheckForUpdates = ConfigInstance.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes { Order = 0 }));
    }

    internal static readonly Dictionary<string, ConfigEntry<bool>> LocationSettings = [];


    internal static void UpdateLists()
    {
        LocationLists.LoadCustomZones();

        var originalLanguage = GameSettings._cur_lng;
        GJL.LoadLanguageResource("en");

        foreach (var location in LocationLists.AllLocations.OrderByDescending(a => Helpers.RemoveCharacters(a.zone)))
        {
            var key = Helpers.RemoveCharacters(location.zone);
            var configEntry = ConfigInstance.Bind(LocationsSection, key, true, $"Toggle visibility of {key} in the menu.");
            location.enabled = configEntry.Value;
            configEntry.SettingChanged += (_, _) =>
            {
                LocationLists.CreatePages();
                LocationLists.AllLocations.Find(a => a.zone == location.zone).enabled = configEntry.Value;
            };

            LocationSettings[location.zone] = configEntry;
        }

        if (!originalLanguage.IsNullOrWhiteSpace())
        {
            GJL.LoadLanguageResource(originalLanguage);
        }

        LocationLists.CreatePages();
    }
}
