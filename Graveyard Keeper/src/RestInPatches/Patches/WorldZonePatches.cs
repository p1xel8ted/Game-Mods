namespace RestInPatches.Patches;

// Two zone-system hot paths:
//
// 1) WorldZone.IsDisabled(): calls GetComponentsInParent<GDPoint>(true) per invocation,
//    which allocates an array every time. It's invoked from GetZoneByID, GetZoneOfPoint,
//    GetZoneOfObject, and the per-zone Recalculate loop — i.e. every player zone change
//    and every WGO zone-belonging check. The GDPoint ancestor chain doesn't change at
//    runtime; we cache the array once per zone and only re-check the activeSelf state.
//
// 2) WorldZone.GetZoneByID(id, ...): linear scan of WorldZone._all_zones. Many callers
//    (pathfinding, build mode, UI). Build a Dictionary<string, WorldZone> and invalidate
//    by comparing _all_zones.Count — InitZonesSystem/RefreshWGOsBelongingsToZone rebuild
//    _all_zones wholesale so count-inequality is a reliable staleness signal.
//
// Both patches preserve original semantics.
[Harmony]
public static class WorldZonePatches
{
    private static readonly ConditionalWeakTable<WorldZone, GDPoint[]> GdParentCache = new();
    private static readonly GDPoint[] EmptyGdPoints = Array.Empty<GDPoint>();

    private static Dictionary<string, WorldZone> _zoneByIdCache;
    private static int _zoneByIdCacheCount = -1;

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.IsDisabled))]
    public static bool WorldZone_IsDisabled_Prefix(WorldZone __instance, ref bool __result)
    {
        if (!GdParentCache.TryGetValue(__instance, out var parents))
        {
            parents = __instance.gameObject.GetComponentsInParent<GDPoint>(true) ?? EmptyGdPoints;
            GdParentCache.Add(__instance, parents);
        }

        for (var i = 0; i < parents.Length; i++)
        {
            var p = parents[i];
            if (p != null && !p.gameObject.activeSelf)
            {
                __result = true;
                return false;
            }
        }

        __result = false;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.GetZoneByID))]
    public static bool WorldZone_GetZoneByID_Prefix(string id, bool null_is_error, ref WorldZone __result)
    {
        var allZones = WorldZone._all_zones;
        if (allZones == null || allZones.Count == 0)
        {
            __result = null;
            return false;
        }

        if (_zoneByIdCache == null || _zoneByIdCacheCount != allZones.Count)
        {
            RebuildZoneCache(allZones);
        }

        if (id != null && _zoneByIdCache.TryGetValue(id, out var zone) && zone != null && !zone.IsDisabled())
        {
            __result = zone;
            return false;
        }

        if (null_is_error)
        {
            Debug.LogError($"Could't find zone [{id}]");
        }

        __result = null;
        return false;
    }

    // InitZonesSystem rebuilds _all_zones; hook into it to invalidate eagerly rather than
    // waiting for the next GetZoneByID call to notice the count mismatch.
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldZone), nameof(WorldZone.InitZonesSystem))]
    public static void WorldZone_InitZonesSystem_Postfix()
    {
        _zoneByIdCache = null;
        _zoneByIdCacheCount = -1;
    }

    private static void RebuildZoneCache(List<WorldZone> allZones)
    {
        _zoneByIdCache = new Dictionary<string, WorldZone>(allZones.Count, StringComparer.Ordinal);
        for (var i = 0; i < allZones.Count; i++)
        {
            var z = allZones[i];
            if (z == null || string.IsNullOrEmpty(z.id))
            {
                continue;
            }
            // First-wins mirrors the original foreach + first-matching-non-disabled behaviour.
            // (Disabled check is deferred to the consumer so late toggles are respected.)
            if (!_zoneByIdCache.ContainsKey(z.id))
            {
                _zoneByIdCache[z.id] = z;
            }
        }
        _zoneByIdCacheCount = allZones.Count;
    }
}
