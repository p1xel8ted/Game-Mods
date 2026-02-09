# CultOfQoL Work Summary - 2026-02-09

## Session Goal
Split fill-to-capacity and mass fill into separate configs, add new silo mass fill features, re-add costs to follower mass actions, rename cost mode, speed up bed collection, and normalize collection speed inconsistencies.

---

## Split Fill-to-Capacity and Mass Fill Configs (COMPLETE)

### Problem
`MassFillTroughs`, `MassFillToolsheds`, and `MassFillMedicStations` each combined two behaviors under one toggle:
1. Fill the interacted structure to capacity in one click
2. Mass fill all other structures of the same type (cost-gated)

### Fix
Split each into parent/child configs using the existing `DispName = "    └ ..."` visual hierarchy pattern.

**New parent configs** (fill single structure to capacity, always free):
- `FillTroughToCapacity` (MassAnimalSection, Order 1)
- `FillToolshedToCapacity` (MassFarmSection, Order 11)
- `FillMedicToCapacity` (MassFarmSection, Order 9)

**Updated child configs** (mass fill all others, cost-gated):
- `MassFillTroughs` → Order 0, `DispName = "    └ Mass Fill Troughs"`
- `MassFillToolsheds` → Order 10, `DispName = "    └ Mass Fill Carpentry Stations"`
- `MassFillMedicStations` → Order 8, `DispName = "    └ Mass Fill Medic Stations"`

**Code logic**: Postfix guards check `FillXToCapacity || MassFillX`. Fill methods run fill-to-capacity unconditionally, then check `MassFillX` before the mass fill section.

### Files Modified
- `CultOfQoL/Configs.cs` — Added 3 parent properties
- `CultOfQoL/Plugin.cs` — Added 3 parent bindings, updated 3 existing with DispName, renumbered MassFarmSection (2→14)
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Updated 3 postfix guards, added mass fill guards in 3 fill methods

---

## Feature: Mass Fill Seed Silos & Fertilizer Silos (COMPLETE)

### What's New
New mass fill features for Seed Silos (`Interaction_SiloSeeder`) and Fertilizer Silos (`Interaction_SiloFertilizer`), built with parent/child split from the start.

### Implementation
Same selector-hook pattern as troughs:
- Postfix on `OnInteract`, hook `UIItemSelectorOverlayController.OnItemChosen`
- Fill triggered silo to capacity via `Structure.DepositInventory()` loop
- Mass fill all other silos via static instance lists (`SiloSeeders`, `SiloFertilizers`)
- Hide selector if triggered silo is at capacity
- Capacity check: `GetCompostCount() < StructureBrain.Capacity` (no in-flight counter for direct deposits)

### Config
- `FillSeedSiloToCapacity` (MassFarmSection, Order 7)
- `MassFillSeedSilos` (MassFarmSection, Order 6, child)
- `FillFertilizerSiloToCapacity` (MassFarmSection, Order 5)
- `MassFillFertilizerSilos` (MassFarmSection, Order 4, child)

### Files Modified
- `CultOfQoL/Configs.cs` — Added 4 properties
- `CultOfQoL/Plugin.cs` — Added 4 bindings
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Added `#region Mass Fill Seed Silos` and `#region Mass Fill Fertilizer Silos`
- `CultOfQoL/Patches/Gameplay/InteractionPatches.cs` — Added 2 cached counts, 2 switch branches, 2 refresh entries

---

## Change: Cost Preview Only Shows in PerObject Mode (COMPLETE)

### Rationale
In `PerMassAction` mode, cost is a flat fee regardless of count — the preview count adds no useful information. Label preview only makes sense in `PerObject` mode where cost scales.

### Implementation
Added early return in `GetCostPreviewTextForCount()`: `if (mode != PerObject) return null;`

### Files Modified
- `CultOfQoL/Core/MassActionCosts.cs` — Added mode check

---

## Change: Renamed PerFollower → PerObject (COMPLETE)

### Rationale
"PerFollower" no longer makes sense since costs now apply to structures (troughs, silos, toolsheds, medics) too.

### Changes
- Enum value: `MassActionCostMode.PerFollower` → `MassActionCostMode.PerObject`
- Config description: "Per Follower = cost multiplied by number of followers affected" → "Per Object = cost multiplied by number of objects affected"
- Default value: `CultOfQoL.MassActionCostMode.PerObject`

### Files Modified
- `CultOfQoL/Configs.cs` — Enum definition
- `CultOfQoL/Plugin.cs` — Config binding
- `CultOfQoL/Core/MassActionCosts.cs` — All references (replace_all)

---

## Re-added Costs to Mass Level Up & Mass Sin Extract (COMPLETE)

### Context
Previously kept free because "no UI to preview cost." But both have visible hover labels:
- Level up: "Collect Loyalty Reward" (when `CanLevelUp()`)
- Sin extract: "Absolve [Name]'s Sin" (when `CanGiveSin()` or floating)

The `Interaction.Label` getter postfix intercepts these since `interaction_FollowerInteraction` extends `Interaction`.

### Implementation
**Cost preview labels**: Added switch branches in `Interaction_Label_Getter` for both:
- `interaction_FollowerInteraction fi when MassLevelUp && fi.follower.Brain.CanLevelUp()` → `CachedLevelableFollowers - 1`
- `interaction_FollowerInteraction fi when MassSinExtract && (Floating || CanGiveSin())` → `CachedSinExtractableFollowers - 1`

Cache-then-subtract pattern: cache total eligible count globally, subtract 1 in the branch (the hovered follower is always eligible).

**Cost deduction**:
- `MassLevelUp_Postfix`: added `MassActionCosts.TryDeductCosts(eligibleCount)` after count check
- `Follower_GiveSinToPlayer_Postfix`: added pre-loop count + `TryDeductCosts(eligible)` guard

### Files Modified
- `CultOfQoL/Patches/Gameplay/InteractionPatches.cs` — +2 cached counts, +2 switch branches, +2 refresh entries
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — +1 line (TryDeductCosts)
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Restructured sin extract postfix with count + cost guard

---

## Normalized Bed/Totem Collection Speeds (COMPLETE)

### Problem
Three overlapping speed mechanisms for bed soul collection:
1. `SpeedUpBedReward` (was gated on `MassCollectFromBeds`) → 0.1f → 0.05f per soul
2. `Interaction_Filter` (gated on `FastCollecting`) → removes all WaitForSeconds → instant
3. When both on, Filter dominated and SpeedUpBedReward's work was wasted

Additionally, mass collection loops were inconsistent — beds and harvest totems used parallel (all at once), while everything else used sequential (0.05f between structures).

### How shrine/bed collection speed actually works (verified against game code)
All three shrine types use `Delay` field as cooldown between souls flying to the player:
- `BuildingShrine.cs:718-722` — `Delay` resets to `ReduceDelay` (starts 0.1f, accelerates)
- `BuildingShrinePassive.cs:231-239` — `Delay` resets to fixed `0.1f`
- `Interaction_DiscipleCollectionShrine.cs:200-210` — `Delay` = `0.1f - AccelerateCollection`

`FastCollecting` sets `Delay = 0` every frame → instant drain. For beds, `GiveReward()` uses `WaitForSeconds(0.1f)` per soul.

### Fix
1. `SpeedUpBedReward` gate: fires when EITHER `FastCollecting` OR `MassCollectFromBeds` is on (0.05f per soul)
2. `Interaction_Filter` still patches `Interaction_Bed.GiveReward` under `FastCollecting` (strips all waits → instant)
3. When both on: SpeedUpBedReward wraps to 0.05f, then Filter strips that → instant
4. When only MassCollectFromBeds: 0.05f per soul (practical speed for mass collecting)
5. `CollectBeds` changed from parallel to sequential with 0.05f between beds
6. `CollectAllHarvestTotems` changed from parallel to sequential with 0.05f between totems

### Final behavior matrix
| Config | Bed soul drain | Between beds |
|--------|---------------|-------------|
| Neither | 0.1f (vanilla) | N/A |
| MassCollectFromBeds only | 0.05f | 0.05f sequential |
| FastCollecting only | instant | N/A |
| Both | instant | 0.05f sequential |

### Files Modified
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — SpeedUpBedReward guard, CollectBeds sequential, CollectAllHarvestTotems sequential

---

## Documentation Updates (COMPLETE)

### Config descriptions updated (Plugin.cs)
- `FastCollecting`: "Speeds up soul drain from shrines (instant) and beds (2x faster). Also removes delays when collecting from resource chests."
- `MassCollectFromBeds`: "...all beds are collected from at once. Also speeds up the per-soul drain to 2x."
- `ShowMassActionCostPreview`: "...in the interaction label... Only visible when Cost Mode is set to Per Object."

### Changelog, README, NexusMods updated
- Added fill-to-capacity toggles, mass fill silos, mass plant seeds
- Added cost preview labels, costs on level up/sin extract
- Renamed Per Follower → Per Object
- Updated Speed Up Collection and Mass Collect From Beds descriptions
- Added "Mass Fill" section to README/NexusMods

---

## Debug Helpers
- F10 key fills all bed souls to max (DEBUG build only, `FastCollectingPatches.cs`)

## Open Items
- All new features need in-game testing (bed speed confirmed working)
- No commits made this session
- Previous session (2026-02-08) features also uncommitted
