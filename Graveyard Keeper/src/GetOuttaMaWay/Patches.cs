namespace GetOuttaMaWay;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.RescanWGOsList))]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.OnPlayerEnter))]
    public static void WorldMap_RescanWGOsList()
    {
        Plugin.GameStartedPlaying();
    }

    // Option 1: when a heavy is spawned with Direction.ToPlayer (the default for crafts/harvests that
    // return drops to the player), redirect it to Direction.None so it scatters near the origin
    // instead of flying at the player's feet. The vanilla None-path still applies kick physics and
    // a small randomised offset, so logs still tumble — they just don't home in.
    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.Drop),
        [typeof(Vector3), typeof(Item), typeof(Transform), typeof(Direction),
         typeof(float), typeof(int), typeof(bool), typeof(bool)])]
    public static void DropResGameObject_Drop_Prefix(Item item, ref Direction direction)
    {
        if (!Plugin.DropHeaviesAwayFromPlayer.Value)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[Drop_Prefix] skip (DropHeaviesAwayFromPlayer=false), item='{item?.id ?? "null"}', dir={direction}");
            return;
        }
        if (direction != Direction.ToPlayer)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[Drop_Prefix] skip (dir={direction} not ToPlayer), item='{item?.id ?? "null"}'");
            return;
        }
        if (item == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[Drop_Prefix] skip (item is null)");
            return;
        }
        if (item.definition == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[Drop_Prefix] skip (item.definition null), item.id='{item.id ?? "null"}'");
            return;
        }
        if (!item.definition.is_big)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[Drop_Prefix] skip (not heavy), item='{item.id}', item_size={item.definition.item_size}");
            return;
        }
        if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[Drop_Prefix] redirecting heavy '{item.id}' ToPlayer -> None");
        direction = Direction.None;
    }

    // Option 2: after DoDrop finishes wiring up the heavy (res item + CapsuleCollider2D enabled),
    // briefly ignore collision between the heavy and the player so the drop can't interrupt
    // the animation. A coroutine restores normal collision after the configured window.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DropResGameObject), nameof(DropResGameObject.DoDrop),
        [typeof(Item), typeof(int), typeof(bool)])]
    public static void DropResGameObject_DoDrop_Postfix(DropResGameObject __instance)
    {
        if (!Plugin.HeavyCollisionGracePeriod.Value)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo("[DoDrop_Postfix] skip (HeavyCollisionGracePeriod=false)");
            return;
        }
        if (__instance == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[DoDrop_Postfix] skip (__instance is null)");
            return;
        }
        if (__instance.res == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[DoDrop_Postfix] skip (res is null)");
            return;
        }
        if (__instance.res.definition == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[DoDrop_Postfix] skip (res.definition null), res.id='{__instance.res.id ?? "null"}'");
            return;
        }
        if (!__instance.res.definition.is_big)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[DoDrop_Postfix] skip (not heavy), res='{__instance.res.id}', item_size={__instance.res.definition.item_size}");
            return;
        }
        if (MainGame.me == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[DoDrop_Postfix] skip (MainGame.me is null)");
            return;
        }
        if (MainGame.me.player == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[DoDrop_Postfix] skip (MainGame.me.player is null)");
            return;
        }

        var heavyCollider = __instance.GetComponent<CapsuleCollider2D>();
        if (heavyCollider == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning($"[DoDrop_Postfix] skip (CapsuleCollider2D missing on heavy '{__instance.res.id}')");
            return;
        }
        var playerCollider = MainGame.me.player.GetComponentInChildren<CircleCollider2D>();
        if (playerCollider == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning("[DoDrop_Postfix] skip (player CircleCollider2D missing)");
            return;
        }

        var seconds = Plugin.GracePeriodSeconds.Value;
        Physics2D.IgnoreCollision(heavyCollider, playerCollider, true);
        if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[DoDrop_Postfix] grace started on heavy '{__instance.res.id}' for {seconds:0.##}s");
        MainGame.me.StartCoroutine(RestoreHeavyCollisionAfterDelay(heavyCollider, playerCollider, seconds, __instance.res.id));
    }

    private static IEnumerator RestoreHeavyCollisionAfterDelay(Collider2D heavy, Collider2D player, float seconds, string heavyId)
    {
        yield return new WaitForSeconds(seconds);
        if (heavy == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[RestoreGrace] heavy '{heavyId}' already destroyed — nothing to restore");
            yield break;
        }
        if (player == null)
        {
            if (Plugin.DebugEnabled) Plugin.LOG.LogWarning($"[RestoreGrace] player collider gone when restoring grace for heavy '{heavyId}'");
            yield break;
        }
        Physics2D.IgnoreCollision(heavy, player, false);
        if (Plugin.DebugEnabled) Plugin.LOG.LogInfo($"[RestoreGrace] grace ended for heavy '{heavyId}', collision restored");
    }
}
