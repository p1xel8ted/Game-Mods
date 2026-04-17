namespace GetOuttaMaWay;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
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

        Debug = Config.Bind("00. Advanced", "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log. Turn on before reporting bugs.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        NpcCollision = Config.Bind("01. General", "NPC", false,
            new ConfigDescription("Toggle NPC collision on or off. When disabled, NPCs will no longer block your path.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        NpcCollision.SettingChanged += (_, _) => GameStartedPlaying();

        DropHeaviesAwayFromPlayer = Config.Bind("01. General", "Drop Heavies Away From Player", true,
            new ConfigDescription("Logs, stones, and mined blocks land next to the tree or rock instead of flying at your feet.", null,
                new ConfigurationManagerAttributes {Order = 90}));

        HeavyCollisionGracePeriod = Config.Bind("01. General", "Heavy Drop Grace Period", true,
            new ConfigDescription("A freshly-dropped log, stone, or block can't push you for a short moment after it lands. You can still push them around afterwards.", null,
                new ConfigurationManagerAttributes {Order = 80}));

        GracePeriodSeconds = Config.Bind("01. General", "Grace Period Seconds", 1.5f,
            new ConfigDescription("Seconds before a freshly-dropped log, stone, or block can push you again.",
                new AcceptableValueRange<float>(0.25f, 5f),
                new ConfigurationManagerAttributes {Order = 79, ShowRangeAsPercent = false, DispName = "    └ Grace Period Seconds"}));

        CheckForUpdates = Config.Bind("── Updates ──", "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 0}));

        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
        SceneManager.sceneLoaded += (_, _) => GameStartedPlaying();
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
