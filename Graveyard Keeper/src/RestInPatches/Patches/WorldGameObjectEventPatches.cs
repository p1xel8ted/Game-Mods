namespace RestInPatches.Patches;

// WorldGameObject.UpdateDelayedEvents runs per WGO per frame via CustomUpdateManager.
// Original: when the delayed-event list is empty (the usual case), it still calls
//   this.GetComponent<ChunkedGameObject>().active_now_because_of_events = false;
// which pays a Unity GetComponent roundtrip every frame for every WGO in the world.
// With hundreds of WGOs registered that's hundreds of no-op component lookups per frame.
//
// This patch fully replaces the method: caches the ChunkedGameObject reference per WGO
// and only writes the flag when it actually changes. Semantics are preserved.
[Harmony]
public static class WorldGameObjectEventPatches
{
    private static readonly ConditionalWeakTable<WorldGameObject, ChunkedGameObjectRef> ChunkedRefs = new();

    private sealed class ChunkedGameObjectRef
    {
        public ChunkedGameObject Value;
        public bool Resolved;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.UpdateDelayedEvents))]
    public static bool WorldGameObject_UpdateDelayedEvents_Prefix(WorldGameObject __instance, float delta_time)
    {
        var events = __instance._events;
        if (events == null)
        {
            return false;
        }

        var ids = events.event_ids;
        var delays = events.event_delays;

        for (var i = 0; i < ids.Count; i++)
        {
            delays[i] -= delta_time;
            if (delays[i] > 0f)
            {
                continue;
            }

            __instance.FireEvent(ids[i]);
            ids.RemoveAt(i);
            delays.RemoveAt(i);
            --i;
        }

        if (ids.Count != 0)
        {
            return false;
        }

        var cached = ChunkedRefs.GetValue(__instance, _ => new ChunkedGameObjectRef());
        if (!cached.Resolved)
        {
            cached.Value = __instance.GetComponent<ChunkedGameObject>();
            cached.Resolved = true;
        }

        var chunked = cached.Value;
        if (chunked != null && chunked.active_now_because_of_events)
        {
            chunked.active_now_because_of_events = false;
        }

        return false;
    }
}
