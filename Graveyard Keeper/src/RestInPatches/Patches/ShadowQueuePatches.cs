namespace RestInPatches.Patches;

// ObjectDynamicShadowsManager.Update runs every frame while shadows are being
// initialised (which happens in bursts during scene load / zone entry). The original
// uses LINQ's Enumerable.First() to pull the next item out of a Dictionary<Action, Action>
// up to four times per frame. First() on IEnumerable<T> allocates a boxed enumerator.
//
// This patch replaces the method body: Dictionary<TKey, TValue>.GetEnumerator() is a
// struct; iterating it with foreach+break is zero-alloc. Semantics (pull up to four
// entries per frame and fire them) are preserved.
[Harmony]
public static class ShadowQueuePatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(ObjectDynamicShadowsManager), nameof(ObjectDynamicShadowsManager.Update))]
    public static bool ObjectDynamicShadowsManager_Update_Prefix(ObjectDynamicShadowsManager __instance)
    {
        __instance._queue_size = __instance._queue.Count;
        if (__instance._queue_size == 0)
        {
            return false;
        }

        var iterations = __instance._queue_size > 4 ? 4 : __instance._queue_size;

        for (var i = 0; i < iterations; i++)
        {
            Action key = null;
            Action done = null;
            foreach (var kv in __instance._queue)
            {
                key = kv.Key;
                done = kv.Value;
                break;
            }

            if (key == null)
            {
                break;
            }

            __instance._queue.Remove(key);
            key();
            done?.Invoke();
        }

        return false;
    }
}
