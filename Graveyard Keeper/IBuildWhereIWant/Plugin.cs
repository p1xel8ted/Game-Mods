namespace IBuildWhereIWant;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.5")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.ibuildwhereiwant";
    private const string PluginName = "I Build Where I Want!";
    private const string PluginVer = "1.7.5";
    
    private static ManualLogSource Log { get; set; }
    private static ConfigEntry<bool> Grid { get;set; }
    private static ConfigEntry<bool> GreyOverlay { get;set; }
    private static ConfigEntry<bool> BuildingCollision { get;set; }
    private static ConfigEntry<bool> Debug { get;set; }
    private static ConfigEntry<KeyboardShortcut> MenuKeyBind { get;set; }
    private static ConfigEntry<string> MenuControllerButton { get;set; }

    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Harmony.CreateAndPatchAll(  Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void InitConfiguration()
    {

        BuildingCollision = Config.Bind("01. Collision", "Building Collision", true, new ConfigDescription("Toggle collision between buildings to place them closer together (or on top of each other...)", null, new ConfigurationManagerAttributes {Order = 604}));

        Grid = Config.Bind("02. Display", "Grid", false, new ConfigDescription("Toggle the grid overlay from the building interface for a cleaner look.", null, new ConfigurationManagerAttributes {Order = 603}));

        GreyOverlay = Config.Bind("02. Display", "Grey Overlay", false, new ConfigDescription("Toggle the grey overlay that appears when removing objects in the building interface.", null, new ConfigurationManagerAttributes {Order = 602}));

        MenuKeyBind = Config.Bind("03. Keybinds", "Menu Key Bind", new KeyboardShortcut(KeyCode.Q), new ConfigDescription("Define the key used to open the mod menu.", null, new ConfigurationManagerAttributes {Order = 601}));

        MenuControllerButton = Config.Bind("04. Controller", "Menu Controller Button", Enum.GetName(typeof(GamePadButton), GamePadButton.LB), new ConfigDescription("Select the controller button used to open the mod menu.", new AcceptableValueList<string>(Enum.GetNames(typeof(GamePadButton))), new ConfigurationManagerAttributes {Order = 600}));

        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging for troubleshooting purposes.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 599}));
    }
    
    private void Update()
    {
        if (!CanOpenCraftAnywhere()) return;

        if ((LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(MenuControllerButton.Value)) ||
            MenuKeyBind.Value.IsUp())
        {
            OpenCraftAnywhere();
        }
    }

    private static bool CanOpenCraftAnywhere()
    {
        return MainGame.game_started && !MainGame.me.player.is_dead && !MainGame.me.player.IsDisabled() &&
               !MainGame.paused && BaseGUI.all_guis_closed &&
               !MainGame.me.player.GetMyWorldZoneId().Contains("refugee");
    }
}