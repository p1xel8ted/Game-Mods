namespace RestInPatches.Patches;

// ItemsDurabilityManager.DoRecalcStep iterates WorldMap._drop_items every 10 frames and
// calls UpdateDurability on each. Item.UpdateDurability (Item.cs:1266) short-circuits
// immediately for non-durable items (and for Rat-typed containers iterates their
// children). The list therefore carries many entries that always no-op.
//
// The inconsistency: WorldMap.OnNewDropItem (WorldMap.cs:110) adds every non-tech drop
// to the list, while WorldMap.RescanDropItemsList (WorldMap.cs:171) only keeps drops
// whose definition has has_durability. So right after a rescan the list is lean, but
// every subsequent drop pickup can re-inflate it with throw-away entries.
//
// Strategy: mirror the rescan's filter at insertion time. Skip adding drops that
// don't have durability. Tech-point drops are already filtered by the original. The
// original then falls through to Add only for durable items — preserving every
// downstream behaviour (OnDropItemRemoved still works because it no-ops when the item
// wasn't added). A defensive null-guard on the definition keeps parity with the rescan.
[Harmony]
public static class DropDurabilityPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnNewDropItem))]
    public static bool WorldMap_OnNewDropItem_Prefix(Item drop_item)
    {
        if (drop_item == null)
        {
            return false;
        }

        var definition = drop_item.definition;
        if (definition == null || !definition.has_durability)
        {
            // Skip the Add — the item has no durability state to tick, so the durability
            // manager has nothing to do for it. Matches RescanDropItemsList's filter.
            return false;
        }

        return true; // fall through to original Add path (which still handles the tech-point and already-contains guards).
    }
}
