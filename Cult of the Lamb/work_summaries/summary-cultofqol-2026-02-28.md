# CultOfQoL Work Summary — 2026-02-28

## Task: Resource Drop Multiplier Feature

### Goal
Add a configurable multiplier for resource drop quantities, covering all world drops (natural resources, enemy loot, dungeon chests, destructibles). Uses a hybrid approach: spawn physical pickups up to a configurable cap, add overflow directly to inventory.

### Changes Made

#### New File: `CultOfQoL/Patches/Gameplay/ResourceMultiplierPatches.cs`
- Harmony prefix on `InventoryItem.Spawn(ITEM_TYPE, int, Vector3, float, Action<PickUp>)` — the single bottleneck for ALL physical drops
- `[ThreadStatic]` recursion guard allows prefix to call original method without re-entry
- `HashSet<ITEM_TYPE> ExcludedTypes` blacklist (~120 items): hearts, tarot, relics, necklaces, weapons, animals, quest items, meta-types, souls
- Hybrid logic: `toSpawn = Min(total, cap)`, `toInventory = total - toSpawn`
- Overflow added via `Inventory.AddItem()` with `ResourceCustomTarget.Create()` visual feedback (capped at 5 sprites)
- Separate prefix on `InventoryItem.SpawnBlackSoul` — opt-in toggle, simple `ref quantity` multiplication

#### Modified: `CultOfQoL/Configs.cs`
- Added `ResourceDropMultiplier` (float), `ResourceDropSpawnCap` (int), `ResourceDropMultiplierBlackSouls` (bool)

#### Modified: `CultOfQoL/Plugin.cs`
- Added 3 config bindings in Loot section (Orders 6/5/4)
- Renumbered existing `AllLootMagnets` (→ Order 3) and `MagnetRangeMultiplier` (→ Order 2)
- Float multiplier uses 0.25 increment rounding pattern

### Key Decisions
- **Prefix over transpiler**: `Spawn()` is 700+ lines; we need to split one call into Spawn + AddItem (can't express as IL modification). Original method runs in full via recursion guard — no logic skipped.
- **Blacklist over whitelist**: New game resource types auto-included without mod update. Only unique/progression items excluded.
- **Coin stacking**: Our multiplier compounds with game's internal `GetCoinsDropMultiplier()` tarot bonus (intentional).
- **Black souls separate**: Already have their own `GetBlackSoulsMultiplier()` tarot system + 20-object cap, so opt-in only.

### Open Issues
- Not yet tested in-game
- No changelog entry added (not yet versioned/released)
- No README/documentation updates yet

### Files Modified
- `CultOfQoL/Patches/Gameplay/ResourceMultiplierPatches.cs` (NEW)
- `CultOfQoL/Configs.cs`
- `CultOfQoL/Plugin.cs`
