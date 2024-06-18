

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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.FixedUpdateOLD))]
    private static void EnemyAI_FixedUpdate(EnemyAI __instance)
    {
        if (__instance is NPCAI || !__instance.SameScene || __instance.freezeMovementAnimation) return;
        EnemyAIDictionary.RemoveAll(a => a == null || a._dead || a.gameObject.activeSelf == false);
        EnemyAIDictionary[__instance] = Utilities.GetDistance(__instance);
    }


    private static void UpdateDistances()
    {
        // var s = Stopwatch.StartNew();
        // Remove all invalid or dead enemies or inactive game objects.
        var keysToRemove = EnemyAIDictionary.Keys
            .Where(enemy => enemy == null || enemy._dead || !enemy.gameObject.activeSelf)
            .ToList();

        foreach (var key in keysToRemove)
        {
            EnemyAIDictionary.Remove(key);
        }

        // Update distances for the remaining enemies.
        foreach (var enemy in EnemyAIDictionary.Keys.ToList()) // ToList creates a stable snapshot of the keys
        {
            var newDistance = Utilities.GetDistance(enemy);
            EnemyAIDictionary[enemy] = newDistance;
        }

        // s.Stop();
        // Plugin.LOG.LogWarning($"UpdateDistances took {s.ElapsedMilliseconds} ms, {s.ElapsedTicks} ticks)");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ScenePortalSpot), nameof(ScenePortalSpot.OnTriggerEnter2D))]
    private static void ScenePortalSpot_OnTriggerEnter2D(ScenePortalSpot __instance)
    {
        UpdateDistances();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Awake))]
    private static void EnemyAI_Awake(ref EnemyAI __instance)
    {
        if (__instance is NPCAI) return;
        var distance = Utilities.GetDistance(__instance);
        EnemyAIDictionary.TryAdd(__instance, distance);
        var ai = __instance;
        __instance.onDie += () => { EnemyAIDictionary.RemoveAll(a => a == ai); };
        __instance.onDestinationReached += () => { EnemyAIDictionary[ai] = Utilities.GetDistance(ai); };

        __instance.onFinishedPath += () => { EnemyAIDictionary[ai] = Utilities.GetDistance(ai); };
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(PlayerInteractions), nameof(PlayerInteractions.OnTriggerEnter2D))]
    public static void PlayerInteractions_OnTriggerEnter2D(PlayerInteractions __instance, Collider2D collider)
    {
        if (!Plugin.EnableAutoTool.Value) return;


        if (!Tools.EnableToolSwaps(__instance, collider)) return;

        if (Utilities.IsInFarmTile() && !Plugin.EnableAutoToolOnFarmTiles.Value) return;

        Tools.UpdateColliders(collider);

        Tools.RunToolActions(collider);
    }
}