# Graveyard Keeper Optimization Analysis

## Scope

This note captures validated optimization opportunities found in the decompiled game code, with current focus on item, loot, and leftover-drop behavior.

## Findings

### High: Ground loot pickup does a full-world scan

`DropCollectorComponent.UpdateComponent()` calls `DropsList.CheckDrops(this.wgo)` on a short interval. `DropsList.CheckDrops()` then iterates the entire global `drops` list and checks distance against every drop.

Impact:
- Pickup cost rises with total leftover ground loot anywhere in the world.
- Cost is paid repeatedly during normal movement near pickups.

Code references:
- `Assembly-CSharp/DropCollectorComponent.cs:20`
- `Assembly-CSharp/DropsList.cs:54`

### High: Drop highlighting is another O(total_drops) pass

`InteractionComponent.UpdateComponent()` always calls `SetHighlighted(this.nearest_drop)`. `DropsList.SetHighlighted()` loops the full drop list to update highlight state, even when the highlighted drop has not changed.

Impact:
- Normal interaction polling scales with total drop count.
- Old world loot inflates per-frame interaction overhead.

Code references:
- `Assembly-CSharp/InteractionComponent.cs:106`
- `Assembly-CSharp/DropsList.cs:64`

### High: Kick checks scale with all registered drop kick components

The player's `KickComponent.UpdateComponent()` iterates the global `_all_kicks` collection. Ground drops keep `KickComponent` instances registered there, so old loot increases kick-query cost globally.

Impact:
- Per-frame kick logic grows with leftover drop count.
- Cost is unrelated to whether those drops are actually nearby.

Code references:
- `Assembly-CSharp/KickComponent.cs:78`
- `Assembly-CSharp/KickComponent.cs:190`

### Medium: Durability manager tracks more drops than needed

`WorldMap.OnNewDropItem()` adds every non-tech drop to `_drop_items`, but `RescanDropItemsList()` only rebuilds the list from durable drops. `ItemsDurabilityManager` then iterates `_drop_items` every 10 frames and calls `UpdateDurability()` on many items that immediately return.

Impact:
- Unnecessary periodic work on non-durable drop items.
- Cost persists until a rescan trims the list.

Code references:
- `Assembly-CSharp/WorldMap.cs:110`
- `Assembly-CSharp/WorldMap.cs:171`
- `Assembly-CSharp/ItemsDurabilityManager.cs:42`
- `Assembly-CSharp/Item.cs:1266`

### Medium: Leftover loot increases save and load cost

Ordinary drops are serialized through `DropsList.ToGameSave()` / `FromGameSave()`. Tech-point drops are serialized through `SerializableGameMap`. I did not find a normal age-out path for ordinary ground loot.

Impact:
- Save size and load work grow with accumulated world loot.
- Persistent leftovers keep feeding the runtime global-list costs above.

Code references:
- `Assembly-CSharp/DropsList.cs:85`
- `Assembly-CSharp/SerializableGameMap.cs:65`
- `Assembly-CSharp/TechPointDrop.cs:56`

### Medium: Bulk item routing does repeated list building and inventory scans

`WorldZone.PutToAllPossibleInventoriesSmart()` rebuilds and sorts a candidate list for each item transfer. Candidate scoring calls `GetItemsCount()`, which scans inventory contents again.

Impact:
- More relevant to porters, automated outputs, and bulk transfers than manual play.
- Could become visible in heavily automated saves.

Code references:
- `Assembly-CSharp/WorldZone.cs:505`
- `Assembly-CSharp/WorldGameObject.cs:3585`
- `Assembly-CSharp/Item.cs:1319`

## Related Non-Loot Finding

### High: Dynamic light and shadow range checks

`DynamicLights.Update()` does per-frame light work and calls into shadow range checks that scale with active dynamic lights and shadow objects.

Code references:
- `Assembly-CSharp/DynamicLights.cs:86`
- `Assembly-CSharp/ObjectDynamicShadow.cs:27`

## Not Recommended As A Standalone Optimization Patch

The per-frame `QualitySettings.vSyncCount = 1` assignment in `MainGame.Update()` is redundant because fullscreen handling already applies v-sync when mode changes, but this is likely only a micro-optimization and not a meaningful performance win on its own.

Code references:
- `Assembly-CSharp/MainGame.cs:238`
- `Assembly-CSharp/PlatformSpecific.cs:322`

## Recommended Patch Order

1. `DropsList.SetHighlighted()` short-circuit or localize highlight updates.
2. `DropsList.CheckDrops()` replace full-list scan with a local query or a narrower candidate set.
3. Remove or localize drop participation in the global kick scan.
4. Filter `_drop_items` to durable entries only at insertion time.
5. Investigate loot cleanup, aging, or caps only if behavior changes are acceptable.
6. Investigate dynamic-light throttling separately from item and loot work.

## Notes

The main pattern is that persistent ground loot participates in several global collections, so old leftovers create costs in pickup checks, highlight checks, kick checks, and save data. The cleanest behavior-preserving wins appear to be the highlight path, pickup path, and global kick participation.
