namespace BeamMeUpGerry;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.beammeupgerryrewrite";
    private const string PluginName = "Beam Me Up Gerry!";
    private const string PluginVer = "3.0.3";
    
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
    private static ConfigEntry<KeyboardShortcut> SaveCustomLocationKeybind { get; set; }
    private static ConfigEntry<string> SaveCustomLocationControllerButton { get; set; }
    
    internal static ManualLogSource Log { get; private set; }

    private Player CachedPlayer { get; set; }
    private Item CachedHearthstone { get; set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
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
                    Menus.ShowMultiAnswer(LocationLists.Locations[0]);
                }
                else
                {
                    CachedHearthstone.UseItem(MainGame.me.player);
                }
            }
        }
        else
        {
            Helpers.SpawnGerry(strings.WhereIsIt, Vector3.zero);
        }
    }


    private void InitConfiguration()
    {
        LocationLists.LoadCustomZones();
        
        DebugEnabled = Config.Bind("00. Debug", "Debug", false, new ConfigDescription("Toggle debug logging", null, new ConfigurationManagerAttributes {IsAdvanced = true,Order = 803}));
        //ModEnabled = Config.Bind("01. General", "Enabled", true, new ConfigDescription($"Toggle {PluginName}", null, new ConfigurationManagerAttributes {Order = 802}));
        IncreaseMenuAnimationSpeed = Config.Bind("01. Features", "Increase Menu Animation Speed", true, new ConfigDescription("Toggle increased menu animation speed", null, new ConfigurationManagerAttributes {Order = 801}));
        EnableListExpansion = Config.Bind("01. Features", "Enable List Expansion", true, new ConfigDescription("Toggle list expansion functionality", null, new ConfigurationManagerAttributes {Order = 799}));
        GerryAppears = Config.Bind("01. Features", "Gerry Appears", false, new ConfigDescription("Toggle Gerry's presence", null, new ConfigurationManagerAttributes {Order = 798}));
        GerryCharges = Config.Bind("01. Features", "Gerry Charges", false, new ConfigDescription("Toggle the cost of teleporting", null, new ConfigurationManagerAttributes {Order = 797}));
        EnablePreviousPageChoices = Config.Bind("01. Features", "Enable Previous Page Choices", true, new ConfigDescription("Toggle the ability to go back to the previous page", null, new ConfigurationManagerAttributes {Order = 796}));
        PreviousPageChoiceAtTop = Config.Bind("01. Features", "Previous Page Choice At Top", true, new ConfigDescription("Toggle the placement of the previous page choice at the top of the list", null, new ConfigurationManagerAttributes {Order = 795}));
        TeleportMenuKeyBind = Config.Bind("02. Keybinds", "Teleport Menu Keybind", new KeyboardShortcut(KeyCode.Z), new ConfigDescription("Set the keybind for opening the teleport menu", null, new ConfigurationManagerAttributes {Order = 796}));
        TeleportMenuControllerButton = Config.Bind("03. Controller", "Teleport Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.RB), new ConfigDescription("Set the controller button for opening the teleport menu", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 795}));
        LocationsPerPage = Config.Bind("04. Locations", "Locations Per Page", 8, new ConfigDescription("Set the number of locations to display per page", new AcceptableValueRange<int>(Mathf.CeilToInt(LocationLists.AllLocations.Count / Constants.MaxPages), (int) Constants.MaxPages), new ConfigurationManagerAttributes {ShowRangeAsPercent = false, Order = 794}));
        SortAlphabetically = Config.Bind("04. Locations", "Sort Alphabetically", false, new ConfigDescription("Toggle alphabetical sorting of locations", null, new ConfigurationManagerAttributes {Order = 793}));
        EnableCustomLocations = Config.Bind("05. Custom Locations", "Enable Custom Locations", false, new ConfigDescription("Toggle the ability to save & load custom locations.", null, new ConfigurationManagerAttributes {Order = 792}));
        SaveCustomLocationKeybind = Config.Bind("05. Custom Locations", "Save Custom Location Keybind", new KeyboardShortcut(KeyCode.X), new ConfigDescription("Set the keybind for saving custom locations.", null, new ConfigurationManagerAttributes {Order = 791}));
        SaveCustomLocationControllerButton = Config.Bind("05. Custom Locations", "Save Custom Location Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.None), new ConfigDescription("Set the controller button for saving custom locations.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 790}));
        
        var originalLanguage = GameSettings._cur_lng;
        GJL.LoadLanguageResource("en");
  
        foreach (var location in LocationLists.AllLocations.OrderByDescending(a => Helpers.RemoveCharacters(a.zone)))
        {
            if (location.zone.Equals(strings.Page_1) || location.zone.Equals(strings.Page_2) || location.zone.Equals(strings.Page_3) || location.zone.Equals(strings.Custom_Locations) || location.zone.Equals(Constants.Cancel)) continue;
            var key = Helpers.RemoveCharacters(location.zone);
            var configEntry = Config.Bind("5. Locations", key, true, $"Toggle visibility of {key} in the menu.");
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