

// ReSharper disable SuggestBaseTypeForParameter

namespace AutoTools;

[Harmony]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class Patches
{
    private static Dictionary<EnemyAI, float> EnemyAIDictionary { get; } = new();

    internal static bool EnemyInArea => EnemyAIDictionary.Any();

    internal static float ClosestDistance
    {
        get
        {
            UpdateDistances();
            return EnemyAIDictionary.Any() ? EnemyAIDictionary.Min(a => a.Value) : float.MaxValue;
        }
    }

    // Cursor-based detection — runs every frame in Player.Update
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    private static void Player_Update(Player __instance)
    {
        if (Plugin.ToolDetectionMode.Value != DetectionMode.Cursor) return;
        if (__instance == null || Player.Instance == null || __instance != Player.Instance) return;

        Tools.DetectToolAtCursor();
    }

    // Reset cursor detection on scene change
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScenePortalSpot), nameof(ScenePortalSpot.OnTriggerEnter2D))]
    private static void ScenePortalSpot_OnTriggerEnter2D(ScenePortalSpot __instance)
    {
        UpdateDistances();
        Tools.ResetCursorDetection();
    }

    // Enemy tracking — shared by both modes
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.FixedUpdateOLD))]
    private static void EnemyAI_FixedUpdate(EnemyAI __instance)
    {
        if (__instance is NPCAI || !__instance.SameScene || __instance.freezeMovementAnimation) return;
        EnemyAIDictionary.RemoveAll(a => a == null || a._dead || a.gameObject.activeSelf == false);
        EnemyAIDictionary[__instance] = Utils.GetDistance(__instance);
    }

    private static void UpdateDistances()
    {
        var keysToRemove = EnemyAIDictionary.Keys
            .Where(enemy => enemy == null || enemy._dead || !enemy.gameObject.activeSelf)
            .ToList();

        foreach (var key in keysToRemove)
        {
            EnemyAIDictionary.Remove(key);
        }

        foreach (var enemy in EnemyAIDictionary.Keys.ToList())
        {
            var newDistance = Utils.GetDistance(enemy);
            EnemyAIDictionary[enemy] = newDistance;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Awake))]
    private static void EnemyAI_Awake(ref EnemyAI __instance)
    {
        if (__instance is NPCAI) return;
        var distance = Utils.GetDistance(__instance);
        EnemyAIDictionary.TryAdd(__instance, distance);
        var ai = __instance;
        __instance.onDie += () => { EnemyAIDictionary.RemoveAll(a => a == ai); };
        __instance.onDestinationReached += () => { EnemyAIDictionary[ai] = Utils.GetDistance(ai); };
        __instance.onFinishedPath += () => { EnemyAIDictionary[ai] = Utils.GetDistance(ai); };
    }

    // Legacy proximity-based detection — only active in Proximity mode
    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(PlayerInteractions __instance, Collider2D collider)
    {
        if (Plugin.ToolDetectionMode.Value != DetectionMode.Proximity) return;
        if (!Plugin.EnableAutoTool.Value) return;

        if (!Tools.EnableToolSwaps(__instance, collider)) return;

        if (Utils.IsInFarmTile() && !Plugin.EnableAutoToolOnFarmTiles.Value) return;

        Tools.UpdateColliders(collider);

        Tools.RunToolActions(collider);
    }
}
