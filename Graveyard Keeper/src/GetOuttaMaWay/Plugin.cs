namespace GetOuttaMaWay;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection = "── Advanced ──";
    private const string GeneralSection  = "── General ──";
    private const string UpdatesSection  = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["00. Advanced"] = AdvancedSection,
        ["01. General"]  = GeneralSection,
    };

    private const string Donkey = "donkey";
    private const string NpcPrefix = "[wgo] ";
    internal static ManualLogSource LOG { get; private set; }
    internal static bool DebugEnabled;
    private static ConfigEntry<bool> Debug { get; set; }
    private static ConfigEntry<bool> NpcCollision { get; set; }
    internal static ConfigEntry<bool> DropHeaviesAwayFromPlayer { get; private set; }
    internal static ConfigEntry<bool> HeavyCollisionGracePeriod { get; private set; }
    internal static ConfigEntry<float> GracePeriodSeconds { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        MigrateRenamedSections();

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log. Turn on before reporting bugs.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        NpcCollision = Config.Bind(GeneralSection, "NPC", false,
            new ConfigDescription("Allow NPCs to block your path. Turn off to walk straight through every NPC instead of nudging around them.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        NpcCollision.SettingChanged += (_, _) => GameStartedPlaying();

        DropHeaviesAwayFromPlayer = Config.Bind(GeneralSection, "Drop Heavies Away From Player", true,
            new ConfigDescription("Logs, stones, and mined blocks land next to the tree or rock instead of flying at your feet.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        HeavyCollisionGracePeriod = Config.Bind(GeneralSection, "Heavy Drop Grace Period", true,
            new ConfigDescription("A freshly-dropped log, stone, or block can't push you for a short moment after it lands. You can still push them around afterwards.", null,
                new ConfigurationManagerAttributes {Order = 80}));

        GracePeriodSeconds = Config.Bind(GeneralSection, "Grace Period Seconds", 1.5f,
            new ConfigDescription("Seconds before a freshly-dropped log, stone, or block can push you again.",
                new AcceptableValueRange<float>(0.25f, 5f),
                new ConfigurationManagerAttributes {Order = 79, ShowRangeAsPercent = false, DispName = "    └ Grace Period Seconds"}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 0}));

        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
        SceneManager.sceneLoaded += (_, _) => GameStartedPlaying();
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
            LOG.LogWarning($"[Migration] Could not read {path} for section rename: {ex.Message}");
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
            LOG.LogWarning($"[Migration] Could not write {path} after section rename: {ex.Message}");
            return;
        }

        LOG.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    internal static void GameStartedPlaying()
    {
        if (!MainGame.game_started) return;
        if (!MainGame.me.player) return;

        var allNpc = WorldMap._npcs;
        var playerCollider = MainGame.me.player.GetComponentInChildren<CircleCollider2D>();

        if (!playerCollider)
        {
            LOG.LogWarning("Player collider not found!");
            return;
        }

        foreach (var npc in allNpc)
        {
            var name = npc.name;
            if (name.StartsWith(NpcPrefix))
            {
                name = name.Substring(NpcPrefix.Length);
            }
            name = name.Trim();

            if (name.Equals(Donkey))
            {
                HandleDonkeyCollision(npc, playerCollider);
            }
            else
            {
                HandleCollision(npc.gameObject.GetComponentInChildren<CircleCollider2D>(), playerCollider, NpcCollision.Value);
            }
        }
    }

    private static void HandleDonkeyCollision(WorldGameObject npc, Collider2D playerCollider)
    {
        var boxCollider = npc.gameObject.GetComponentInChildren<BoxCollider2D>();
        var capsuleCollider = npc.gameObject.GetComponentInChildren<CapsuleCollider2D>();

        if (!boxCollider || !capsuleCollider) return;

        Physics2D.IgnoreCollision(boxCollider, playerCollider, NpcCollision.Value);
        Physics2D.IgnoreCollision(capsuleCollider, playerCollider, NpcCollision.Value);
    }

    private static void HandleCollision(Collider2D npcCollider, Collider2D playerCollider, bool collisionValue)
    {
        if (!npcCollider) return;

        Physics2D.IgnoreCollision(npcCollider, playerCollider, !collisionValue);
    }
}
