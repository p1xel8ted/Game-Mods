namespace MiscBitsAndBobs;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.miscbitsandbobs";
    private const string PluginName = "Misc. Bits & Bobs";
    private const string PluginVer = "2.3.4";

    internal static ConfigEntry<bool> Debug { get; set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;
    internal static ConfigEntry<bool> QuietMusicInGuiConfig { get; private set; }
    internal static ConfigEntry<bool> CondenseXpBarConfig { get; private set; }
    internal static ConfigEntry<bool> ModifyPlayerMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<float> PlayerMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<bool> ModifyPorterMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<float> PorterMovementSpeedConfig { get; private set; }
    internal static ConfigEntry<bool> HalloweenNowConfig { get; private set; }
    internal static ConfigEntry<bool> HideCreditsButtonOnMainMenuConfig { get; private set; }
    internal static ConfigEntry<bool> SkipIntroVideoOnNewGameConfig { get; private set; }
    internal static ConfigEntry<bool> CinematicLetterboxingConfig { get; private set; }
    internal static ConfigEntry<bool> KitsuneKitoModeConfig { get; private set; }
    internal static ConfigEntry<bool> LessenFootprintImpactConfig { get; private set; }
    internal static ConfigEntry<bool> RemovePrayerOnUseConfig { get; private set; }
    internal static ConfigEntry<bool> AddCoalToTavernOvenConfig { get; private set; }
    internal static ConfigEntry<bool> AddZombiesToPyreAndCrematoriumConfig { get; private set; }
    internal static ConfigEntry<bool> OldEnglishThrowback { get; private set; }


    internal static ManualLogSource Log { get; private set; }


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
        Debug = Config.Bind("00. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {Order = 12}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        QuietMusicInGuiConfig = Config.Bind("02. Audio", "Quiet Music In GUI", true, new ConfigDescription("Lower the music volume when in-game menus are open.", null, new ConfigurationManagerAttributes {Order = 29}));

        CondenseXpBarConfig = Config.Bind("03. UI", "Condense XP Bar", true, new ConfigDescription("Reduce the size of the XP bar in the user interface.", null, new ConfigurationManagerAttributes {Order = 28}));
        HideCreditsButtonOnMainMenuConfig = Config.Bind("03. UI", "Hide Credits Button On Main Menu", true, new ConfigDescription("Remove the credits button from the main menu.", null, new ConfigurationManagerAttributes {Order = 27}));
        CinematicLetterboxingConfig = Config.Bind("03. UI", "Remove Cinematic Letterboxing", true, new ConfigDescription("Remove black bars during cinematic cutscenes.", null, new ConfigurationManagerAttributes {Order = 25}));

        HalloweenNowConfig = Config.Bind("04. Gameplay", "Halloween Now", false, new ConfigDescription("Activate Halloween mode at any time.", null, new ConfigurationManagerAttributes {Order = 24}));
        SkipIntroVideoOnNewGameConfig = Config.Bind("04. Gameplay", "Skip Intro Video On New Game", false, new ConfigDescription("Skip the intro video when starting a new game.", null, new ConfigurationManagerAttributes {Order = 23}));
        LessenFootprintImpactConfig = Config.Bind("04. Gameplay", "Lessen Footprint Impact", false, new ConfigDescription("Reduce the impact of footprints on the environment.", null, new ConfigurationManagerAttributes {Order = 22}));
        RemovePrayerOnUseConfig = Config.Bind("04. Gameplay", "Remove Prayer On Use", false, new ConfigDescription("Prayers are removed after use.", null, new ConfigurationManagerAttributes {Order = 21}));
        AddCoalToTavernOvenConfig = Config.Bind("04. Gameplay", "Add Coal To Tavern Oven", true, new ConfigDescription("Allow coal to be used as fuel in the tavern oven.", null, new ConfigurationManagerAttributes {Order = 20}));
        AddZombiesToPyreAndCrematoriumConfig = Config.Bind("04. Gameplay", "Add Zombies To Pyre And Crematorium", true, new ConfigDescription("Enable the option to burn zombies at the pyre and crematorium.", null, new ConfigurationManagerAttributes {Order = 19}));

        ModifyPlayerMovementSpeedConfig = Config.Bind("05. Movement", "Modify Player Movement Speed", true, new ConfigDescription("Allow modification of the player's movement speed.", null, new ConfigurationManagerAttributes {Order = 18}));
        PlayerMovementSpeedConfig = Config.Bind("05. Movement", "Player Movement Speed", 1.0f, new ConfigDescription("Set the player's movement speed.", new AcceptableValueRange<float>(1.0f, 100f), new ConfigurationManagerAttributes {Order = 17}));
        ModifyPorterMovementSpeedConfig = Config.Bind("05. Movement", "Modify Porter Movement Speed", true, new ConfigDescription("Allow modification of the porter's movement speed.", null, new ConfigurationManagerAttributes {Order = 16}));
        PorterMovementSpeedConfig = Config.Bind("05. Movement", "Porter Movement Speed", 1.0f, new ConfigDescription("Set the porter's movement speed.", new AcceptableValueRange<float>(1.0f, 100f), new ConfigurationManagerAttributes {Order = 15}));
        KitsuneKitoModeConfig = Config.Bind("06. Misc", "KitsuneKito Mode", false, new ConfigDescription("Discord user request. Drops a blue xp point when adding a basic fence to a grave.", null, new ConfigurationManagerAttributes {Order = 14}));
        OldEnglishThrowback = Config.Bind("06. Misc", "Old English Throwback", false, new ConfigDescription("Discord user request. Modifies a sermon sentence.", null, new ConfigurationManagerAttributes {Order = 13}));

        Config.Bind("09. Church", "Evict All Church Visitors", true,
            new ConfigDescription("Force any stuck church visitors to vacate the premise.", null,
                new ConfigurationManagerAttributes {Order = 5, HideDefaultButton = true, CustomDrawer = EvictVisitorsButton}));
    }

    private static bool _showEvictConfirmation;

    private static void EvictVisitorsButton(ConfigEntryBase entry)
    {
        if (_showEvictConfirmation)
        {
            Lang.Reload();
            GUILayout.Label(Lang.Get("EvictConfirmText"));
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button(Lang.Get("EvictYes"), GUILayout.ExpandWidth(true)))
                {
                    EvictVisitors();
                    _showEvictConfirmation = false;
                }
                if (GUILayout.Button(Lang.Get("EvictNo"), GUILayout.ExpandWidth(true)))
                {
                    _showEvictConfirmation = false;
                }
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (GUILayout.Button(Lang.Get("EvictButton"), GUILayout.ExpandWidth(true)))
            {
                _showEvictConfirmation = true;
            }
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }

    private static void EvictVisitors()
    {
        var churchVisitors = WorldMap._objs.FindAll(a => a.obj_id.Contains("npc_church_visitor"));
        if (churchVisitors.Count == 0)
        {
            Log.LogWarning("No church visitors to evict.");
            return;
        }

        Log.LogInfo($"Evicting {churchVisitors.Count} church visitors.");

        var zones = new List<WorldZone>();

        foreach (var visitor in churchVisitors.Where(visitor => visitor.obj_def.IsNPC()))
        {
            zones.Add(visitor._zone);

            if (visitor.is_removed) continue;

            visitor.components.craft.enabled = false;
            visitor.components.timer.enabled = false;
            visitor.components.hp.enabled = false;
            ChunkManager.OnDestroyObject(visitor);
            if (visitor._bubble != null)
            {
                InteractionBubbleGUI.RemoveBubble(visitor.unique_id, true);
                visitor._bubble = null;
            }

            visitor.UnlinkWithSpawnerIfExists();
            visitor.is_removed = true;
            UnityEngine.Object.Destroy(visitor.gameObject);
            if (!visitor._was_ever_active)
            {
                visitor.OnDestroy();
            }
        }

        foreach (var zone in zones.Distinct())
        {
            zone.Recalculate();
        }
    }
}