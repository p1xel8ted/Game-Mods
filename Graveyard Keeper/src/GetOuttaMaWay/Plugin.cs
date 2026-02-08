namespace GetOuttaMaWay;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.getouttamyway";
    private const string PluginName = "Get Outta My Way!";
    private const string PluginVer = "0.1.2";
    private const string Donkey = "donkey";
    private const string NpcPrefix = "[wgo] ";
    private static ManualLogSource LOG { get; set; }
    private static ConfigEntry<bool> NpcCollision { get; set; }

    private void Awake()
    {
        LOG = Logger;
        NpcCollision = Config.Bind("01. General", "NPC", false, "Toggle NPC collision on or off. When disabled, NPCs will no longer block your path.");
        NpcCollision.SettingChanged += (_, _) => GameStartedPlaying();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        SceneManager.sceneLoaded += (_, _) => GameStartedPlaying();
        GYKHelper.StartupLogger.PrintModLoaded(PluginName, LOG);
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
