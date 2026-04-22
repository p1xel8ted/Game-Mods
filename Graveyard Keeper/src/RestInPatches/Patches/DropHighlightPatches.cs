namespace RestInPatches.Patches;

// DropsList.SetHighlighted is called every interaction tick from
// InteractionComponent.UpdateComponent (InteractionComponent.cs:115):
//
//     this._drops_list.SetHighlighted(this.nearest_drop);
//
// The original iterates every drop in the world and sets its interaction highlight,
// even when the highlight target hasn't changed since last tick — which is the common
// case during normal movement. With hundreds of accumulated ground drops this becomes
// a significant O(N_total_drops) pass per interaction tick.
//
// Strategy: remember the last drop we highlighted. When SetHighlighted is called with
// the same target (including both-null transitions), skip the work entirely. When the
// target changes, fall through to the original method which correctly flips every
// drop's state. This preserves edge cases — newly-spawned drops start un-highlighted
// so we don't need to touch them, and destroyed drops don't need un-highlighting.
//
// Per-DropsList state rather than a single static — DropsList is a singleton in
// practice but ConditionalWeakTable future-proofs against multi-instance edge cases.
[Harmony]
public static class DropHighlightPatches
{
    private static readonly ConditionalWeakTable<DropsList, Box> LastTargets = new();

    private sealed class Box
    {
        public DropResGameObject Target;
        public bool Set;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(DropsList), nameof(DropsList.SetHighlighted))]
    public static bool DropsList_SetHighlighted_Prefix(DropsList __instance, DropResGameObject drop)
    {
        var box = LastTargets.GetOrCreateValue(__instance);

        // ReferenceEquals deliberately — we only want to skip when the exact same drop
        // instance is re-targeted. Unity's operator== adds a null-check that treats
        // a destroyed-but-referenced object as equal to null, which would cause us to
        // skip un-highlighting a stale drop. ReferenceEquals ignores that quirk.
        if (box.Set && ReferenceEquals(box.Target, drop))
        {
            return false;
        }

        box.Set = true;
        box.Target = drop;
        return true; // fall through to original; it handles both null-drop (all off) and specific-drop paths.
    }
}
