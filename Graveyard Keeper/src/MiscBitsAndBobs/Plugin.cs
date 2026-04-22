namespace MiscBitsAndBobs;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    // Section names: plain "── Name ──" style. CM renders sections in Config.Bind call order,
    // so Advanced appears first by virtue of Debug being the first bind below. Legacy section
    // names get rewritten to these by MigrateRenamedSections() on first launch so existing
    // user customisations survive the rename.
    private const string AdvancedSection = "── Advanced ──";
    private const string AudioSection    = "── Audio ──";
    private const string UISection       = "── UI ──";
    private const string GameplaySection = "── Gameplay ──";
    private const string MovementSection = "── Movement ──";
    private const string ChurchSection   = "── Church ──";
    private const string MiscSection     = "── Misc ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"]      = AdvancedSection,
        ["02. Audio"]         = AudioSection,
        ["03. UI"]            = UISection,
        ["04. Gameplay"]      = GameplaySection,
        ["05. Movement"]      = MovementSection,
        ["06. Misc"]          = MiscSection,
        ["09. Church"]        = ChurchSection,
        ["── 1. Advanced ──"] = AdvancedSection,
        ["── 2. Audio ──"]    = AudioSection,
        ["── 3. UI ──"]       = UISection,
        ["── 4. Gameplay ──"] = GameplaySection,
        ["── 5. Movement ──"] = MovementSection,
        ["── 6. Church ──"]   = ChurchSection,
        ["── 7. Misc ──"]     = MiscSection,
        ["── 8. Updates ──"]  = UpdatesSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
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
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }


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

    // Rewrites old "[04. Gameplay]" style headers to "[── 4. Gameplay ──]" in the .cfg
    // file so existing user values survive the section rename. Idempotent — re-running
    // on an already-migrated file is a no-op (no old headers left to find).
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
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        // ── 2. Audio ──
        QuietMusicInGuiConfig = Config.Bind(AudioSection, "Quiet Music In GUI", true,
            new ConfigDescription("On: the music drops in volume whenever you open an in-game menu so it doesn't drown out the UI. Off: music stays at full volume during menus.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        // ── 3. UI ──
        CondenseXpBarConfig = Config.Bind(UISection, "Condense XP Bar", true,
            new ConfigDescription("On: the red/green/blue XP numbers on the HUD compact to a shorter form once they hit four digits (for example 12450 shows as 12.4K). Off: the full numbers are shown.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        HideCreditsButtonOnMainMenuConfig = Config.Bind(UISection, "Hide Credits Button On Main Menu", true,
            new ConfigDescription("On: the Credits button is removed from the main menu. Off: the Credits button is visible as normal.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        CinematicLetterboxingConfig = Config.Bind(UISection, "Remove Cinematic Letterboxing", true,
            new ConfigDescription("On: the black bars shown during cutscenes (sermons, cinematics) are removed so the full screen stays visible. Off: cutscenes play with the original letterboxing.", null,
                new ConfigurationManagerAttributes {Order = 98}));

        // ── 4. Gameplay ──
        HalloweenNowConfig = Config.Bind(GameplaySection, "Halloween Now", false,
            new ConfigDescription("On: the Halloween event runs year-round. Off: the event is limited to its normal late-October schedule.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        SkipIntroVideoOnNewGameConfig = Config.Bind(GameplaySection, "Skip Intro Video On New Game", false,
            new ConfigDescription("On: the opening cinematic is skipped when you start a new game and you drop straight into play. Off: the intro video plays as normal.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        LessenFootprintImpactConfig = Config.Bind(GameplaySection, "Lessen Footprint Impact", false,
            new ConfigDescription("On: new footprints are drawn at roughly half their normal dirt intensity so trails look subtler. Off: footprints leave the game's default amount of dirt.", null,
                new ConfigurationManagerAttributes {Order = 98}));

        RemovePrayerOnUseConfig = Config.Bind(GameplaySection, "Remove Prayer On Use", false,
            new ConfigDescription("On: each prayer item is consumed when you pray, so stockpiled prayers become single-use. Off: prayers stay in your inventory after praying like they normally do.", null,
                new ConfigurationManagerAttributes {Order = 97}));

        AddCoalToTavernOvenConfig = Config.Bind(GameplaySection, "Add Coal To Tavern Oven", true,
            new ConfigDescription("On: the tavern oven accepts coal as a fuel source alongside its normal firewood. Off: only the vanilla fuel is accepted.", null,
                new ConfigurationManagerAttributes {Order = 96}));

        AddZombiesToPyreAndCrematoriumConfig = Config.Bind(GameplaySection, "Add Zombies To Pyre And Crematorium", true,
            new ConfigDescription("On: working zombies and bodies can be loaded into the pyre and crematorium for disposal. Off: only the vanilla inputs are accepted.", null,
                new ConfigurationManagerAttributes {Order = 95}));

        // ── 5. Movement ──
        ModifyPlayerMovementSpeedConfig = Config.Bind(MovementSection, "Modify Player Movement Speed", true,
            new ConfigDescription("On: the player's walking speed is scaled by the multiplier below. Off: the player moves at the game's default speed.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        PlayerMovementSpeedConfig = Config.Bind(MovementSection, "Player Movement Speed", 1.0f,
            new ConfigDescription("How much faster the player walks when the toggle above is on. 1 is vanilla speed, 2 is twice as fast, and so on. Don't combine with other speed mods.",
                new AcceptableValueRange<float>(1.0f, 100f),
                new ConfigurationManagerAttributes {Order = 99, DispName = "    └ Player Movement Speed"}));

        ModifyPorterMovementSpeedConfig = Config.Bind(MovementSection, "Modify Porter Movement Speed", true,
            new ConfigDescription("On: porter (backpack-wearing zombie worker) walking speed is overridden by the multiplier below. Off: porters move at their normal game speed.", null,
                new ConfigurationManagerAttributes {Order = 98}));

        PorterMovementSpeedConfig = Config.Bind(MovementSection, "Porter Movement Speed", 1.0f,
            new ConfigDescription("How fast porters walk when the toggle above is on. 1 is their vanilla speed.",
                new AcceptableValueRange<float>(1.0f, 100f),
                new ConfigurationManagerAttributes {Order = 97, DispName = "    └ Porter Movement Speed"}));

        // ── 6. Church ──
        Config.Bind(ChurchSection, "Evict All Church Visitors", true,
            new ConfigDescription("Click to force any stuck church visitors to leave immediately. Use this when villagers have lingered inside and are blocking the pulpit or benches.", null,
                new ConfigurationManagerAttributes {Order = 100, HideDefaultButton = true, CustomDrawer = EvictVisitorsButton}));

        // ── 7. Misc ──
        KitsuneKitoModeConfig = Config.Bind(MiscSection, "KitsuneKito Mode", false,
            new ConfigDescription("A Discord-requested tweak: adding the basic wooden fence to a grave drops a single blue XP orb. Off by default.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        OldEnglishThrowback = Config.Bind(MiscSection, "Old English Throwback", false,
            new ConfigDescription("A Discord-requested tweak: changes the sermon line 'Our church is great!' to 'Our church great!' in English. Has no effect in other languages.", null,
                new ConfigurationManagerAttributes {Order = 99}));

        // ── 8. Updates ──
        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));
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

    private static void EvictVisitors()
    {
        var churchVisitors = WorldMap._objs.FindAll(a => a.obj_id.Contains("npc_church_visitor"));
        if (churchVisitors.Count == 0)
        {
            if (DebugEnabled) Helpers.Log("[Evict] No church visitors found to evict.");
            return;
        }

        if (DebugEnabled) Helpers.Log($"[Evict] Evicting {churchVisitors.Count} church visitor(s).");

        var zones = new List<WorldZone>();

        foreach (var visitor in churchVisitors.Where(visitor => visitor.obj_def.IsNPC()))
        {
            zones.Add(visitor._zone);

            if (visitor.is_removed)
            {
                if (DebugEnabled) Helpers.Log($"[Evict] Skipping {visitor.obj_id} — already marked removed.");
                continue;
            }

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

            if (DebugEnabled) Helpers.Log($"[Evict] Removed visitor {visitor.obj_id} from zone {visitor._zone?.id ?? "<null>"}.");
        }

        foreach (var zone in zones.Distinct())
        {
            if (DebugEnabled) Helpers.Log($"[Evict] Recalculating zone '{zone.id}' after visitor eviction.");
            zone.Recalculate();
        }
    }
}
