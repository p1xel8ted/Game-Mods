# CultOfQoL Work Summary - 2026-02-08

## Context: User Feedback from KingJoshington on Unreleased 2.4.1

Addressing feedback on mass action costs, rot fertilizer, mass pet bug, and a new furnace feature request. Mass action cost changes (flat fee) and mass pet bug fix deferred for further discussion.

---

## Fix: Rot Fertilizer Decay Not Stopping Growth (COMPLETE)

### Problem
When rot fertilizer decay expired (`DefrostedCrop = false`), crops showed frozen ground visually but kept growing. Root cause: the POOP_ROTSTONE fertilizer item stays on the plot, and `CanGrow()` has a separate branch `HasFertilized() && type == 187` that enables growth independently of `DefrostedCrop`.

The `SetWithered()` guard at line 329 also checks `GetFertilizer().type == 187`, preventing withering. This is desirable — crops should freeze, not die.

### Fix
Added postfix on `Structures_FarmerPlot.CanGrow()` that blocks growth when:
- Feature is enabled (`RotFertilizerDecay`)
- Result would be true
- `DefrostedCrop` is false (warming expired)
- POOP_ROTSTONE is the active fertilizer
- No other growth enabler is active (Crop Grower, winter-growing seed)

Crops freeze in place and can be re-fertilized to resume growth. The POOP_ROTSTONE stays on the plot so `SetWithered()` guard still prevents withering.

### Files Modified
- `CultOfQoL/Patches/Structures/FarmPlotPatches.cs` — Added `CanGrow_Postfix`

### Key Game Code References
- `Structures_FarmerPlot.CanGrow()` (line 57-62) — four OR-ed conditions for growth
- `Structures_FarmerPlot.SetWithered()` (line 324-338) — guard prevents withering when POOP_ROTSTONE present
- `Structures_FarmerPlot.OnNewPhaseStarted()` (line 182-226) — calls SetWithered during blizzards when CanGrow is false

---

## Change: Rot Fertilizer Refinery Recipe (COMPLETE)

### What Changed
Recipe updated from `10 POOP → 1 POOP_ROTSTONE` to `10 POOP + 5 SOOT → 1 POOP_ROTSTONE`.

SOOT is the internal name for what players call "Rotgrit" — a byproduct of the Level 3 Furnace that refines back into Rotburn (MAGMA_STONE). Adding it gives Rotgrit a secondary use.

### Files Modified
- `CultOfQoL/Patches/Structures/StructurePatches.cs` — Updated ItemCost list in `Structures_Refinery_GetCost` postfix

---

## Feature: Furnace Heater Fuel Scaling (COMPLETE)

### Goal
Each proximity heater increases the furnace's fuel drain during winter, adding challenge to heating large bases.

### Implementation
- Subscribes to `TimeManager.OnNewPhaseStarted` static event (since `Structures_Furnace` doesn't override `OnNewPhaseStarted`)
- Each phase during winter: counts proximity heaters, drains `heaterCount * costPerHeater` fuel from all furnaces
- Uses `StructureManager.GetAllStructuresOfType(PROXIMITY_FURNACE)` for heater count (shared cache — must read count before next call)
- Uses `StructureManager.GetAllStructuresOfType<Structures_Furnace>()` for furnace iteration (creates new list, safe)
- Calls `furnace.UpdateFuel(totalDrain)` which handles clamping, FullyFueled flag, and OnFuelModified event

### Config
- `FurnaceHeaterScaling` (bool, default false) in Structures section (Order 34)
- `FurnaceHeaterFuelCost` (int, default 500, range 100-5000) in Structures section (Order 33)

### Fuel Math
- 1 MAGMA_STONE = 14,700 fuel units
- At default 500/heater/phase with 5 heaters: 2,500/phase, ~10,000/day
- One MAGMA_STONE lasts ~1.5 days with 5 heaters at default cost

### Files Created
- `CultOfQoL/Patches/Structures/FurnacePatches.cs`

### Files Modified
- `CultOfQoL/Configs.cs` — Added `FurnaceHeaterScaling`, `FurnaceHeaterFuelCost` properties
- `CultOfQoL/Plugin.cs` — Added bindings in Structures section

---

## Documentation Updates

### Changelog (2.4.1 entry)
- Added furnace heater scaling feature entry
- Updated refinery recipe description (Poop + Rotgrit)
- Added rot fertilizer decay fix

### README + nexusmods_description.txt
- Updated refinery recipe description
- Added furnace heater scaling to Structures section

### Files Modified
- `Thunderstore/cult/CHANGELOG.md`
- `Thunderstore/cult/README.md`
- `Thunderstore/cult/nexusmods_description.txt`

---

## Mass Action Cost Mode + Preview (COMPLETE)

### What Changed
Added `MassActionCostMode` enum dropdown (PerMassAction / PerFollower) to control how costs are calculated. PerMassAction = flat fee. PerFollower = cost * count (original behavior). Added `ShowMassActionCostPreview` config to show estimated cost notification before each mass action executes.

### Implementation Notes
- Property named `MassActionCostModeEntry` (not `MassActionCostMode`) to avoid shadowing the enum type
- Default value uses fully qualified `CultOfQoL.MassActionCostMode.PerFollower` for the same reason
- Gold/time descriptions updated to remove "per follower/target" (now mode-dependent)
- Float sliders kept with 0.25 stepping — preview notification makes fractional costs clear

### Files Modified
- `CultOfQoL/Configs.cs` — Added enum, `MassActionCostModeEntry`, `ShowMassActionCostPreview` properties
- `CultOfQoL/Plugin.cs` — Added bindings (Order 5, 4), updated gold/time descriptions
- `CultOfQoL/Core/MassActionCosts.cs` — Rewrote `TryDeductCosts` for mode-based calculation + preview

---

## Mass Pet Refinement + Pet All Config (COMPLETE)

### What Changed
Mass pet now only targets dogs, Poppy, and followers with the Pettable trait by default. Added `MassPetAllFollowers` config to pet everyone (restoring the old behavior from 2.3.9).

Note: This is NOT a bug fix — the "pet all" behavior was intentionally added in 2.3.9. This is a refinement based on KingJoshington's feedback.

### Implementation
- Added `CanBePetted(Follower f)` helper that checks `MassPetAllFollowers` OR `SkinName.Contains("Dog"/"Poppy")` OR `HasTrait(Pettable)`
- Updated `ShouldMassPetFollower` count and pet execution LINQ filter to use the helper
- Shifted `MassSinExtract` from Order 0 → Order -1 to make room for new sub-option at Order 0

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassPetAllFollowers` property
- `CultOfQoL/Plugin.cs` — Added binding (Order 0, indent prefix), shifted MassSinExtract
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — Added `CanBePetted`, updated filters

---

## Furnace Heater Scaling — Open Design Questions
- User suggested "+1 rot to run" per heater, but the fuel system uses internal fuel units (14,700 per Rotburn). Current implementation uses configurable fuel units per heater per phase.
- May need user feedback on whether default cost feels right in-game.

---

## Config Description Fixes (COMPLETE)
- Changed "Crop Grower" → "Farm Station" in rot fertilizer duration config description
- Updated MassPetFollower description to reflect new behavior ("all eligible followers" + reference to sub-option)

---

## Cost Preview Timing Fix (COMPLETE)

### Problem
The `ShowMassActionCostPreview` notification fired inside `TryDeductCosts()` — too late, wheel UI was already hidden with letterboxing.

### Solution
Moved cost preview to the command wheel description text via a `GetDescription()` postfix. The radial menu's `DoWheelLoop()` calls `GetDescription()` every frame for the highlighted item, so cost text updates naturally as the player hovers.

- Removed preview notification block from `TryDeductCosts()` (kept "not enough gold" error notification)
- Added `GetCostPreviewText(FollowerCommands cmd)` — returns formatted cost string or null
- Added `GetMassActionTargetCount(FollowerCommands cmd)` — counts eligible targets per command type
- Made `CanBePetted` internal so `MassActionCosts` can use it for pet target counting

### Files Modified
- `CultOfQoL/Core/MassActionCosts.cs` — Added `GetCostPreviewText`, `GetMassActionTargetCount`, removed preview from `TryDeductCosts`
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — Changed `CanBePetted` from private to internal

### Files Created
- `CultOfQoL/Patches/UI/WheelCostPreviewPatch.cs` — `GetDescription` postfix on `UIFollowerWheelInteractionItem`

---

## Translation Dump Utility (COMPLETE)

### What It Does
Debug keybind (F9, gated behind EnableLogging) dumps all English translations from I2 Localization to `BepInEx/plugins/CultOfQoL/translations_en.txt`. Format: `term.Term = translation` per line. Shows notification with count when done. Dumped file has been copied to `libs/translations_en.txt` for dev reference.

### Files Modified
- `CultOfQoL/GlobalUsings.cs` — Added `global using I2.Loc;`
- `CultOfQoL/Configs.cs` — Added `DumpTranslationsKey` property
- `CultOfQoL/Plugin.cs` — Added binding (General section, Order 4, sub-option of EnableLogging), keybind check in `Update()`, `DumpTranslations()` method

---

## Config Description Fixes (COMPLETE)
- Fixed rot fertilizer duration description: "Farm Station" → "Thawing Harvest Totem" (correct in-game name for FARM_CROP_GROWER)
- Fixed cost preview description: "notification" → "command wheel description"
- Changed cost preview label from "Cost:" → "Mass cost:" to distinguish from single interaction

---

## CLAUDE.md Update
- Added reference to `libs/translations_en.txt` with instruction to ALWAYS check it for player-facing names in configs, changelogs, and READMEs

---

## Documentation Updates
- `Thunderstore/cult/CHANGELOG.md` — Updated cost preview wording ("in the command wheel" instead of "notification")
- `Thunderstore/cult/README.md` — Same
- `Thunderstore/cult/nexusmods_description.txt` — Same

---

## Feature: Mass Clean Poop & Vomit (COMPLETE)

### What Changed
Added two new toggles in the Mass Collect config section:
- **Mass Clean Poop** — cleaning one poop pile triggers cleanup of all others
- **Mass Clean Vomit** — cleaning one vomit puddle triggers cleanup of all others

Both default to off. No mass action costs applied (consistent with other collection actions).

### Implementation
Uses the same coroutine-based pattern as outhouse mass collection:
- Postfix on `Interaction_Poop.OnInteract` / `Vomit.OnInteract`
- Static re-entrancy guard (`CleanAllPoopRunning` / `CleanAllVomitRunning`)
- Iterates `Interaction_Poop.Poops` / `Vomit.Vomits` static lists (snapshot via `.ToList()`)
- Checks `!poop.Activating` / `!v.Activating` to skip piles already being cleaned
- Game's own `OnInteract` also checks `Activating` as a safety guard

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassCleanPoop`, `MassCleanVomit` properties
- `CultOfQoL/Plugin.cs` — Added bindings in MassCollectSection (Order 0, -1)
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Added guards, postfix patches, coroutines

---

## Change: Standardized Collection Coroutine Delays (COMPLETE)

### What Changed
All collection coroutines in `FastCollectingPatches.cs` changed from `0.10f` to `0.05f` delay between iterations for snappier feel. Matches the `0.05f` already used by farm actions (water/fertilize) in `InteractionPatches.cs`. Sin extraction delay (`0.4f`) left unchanged.

### Affected Coroutines
`CollectBeds`, `CollectAllBuildingShrines`, `CollectAllOuthouse`, `CollectAllCompostBinDeadBody`, `CollectAllHarvestTotems`, `CollectAllShrines`, `CleanAllPoop`, `CleanAllVomit`

### Files Modified
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — 8 occurrences of `0.10f` → `0.05f`

---

## Documentation Updates
- `Thunderstore/cult/CHANGELOG.md` — Added mass clean poop/vomit + delay improvement entries
- `Thunderstore/cult/README.md` — Added mass clean poop/vomit to Mass Collect section
- `Thunderstore/cult/nexusmods_description.txt` — Same (BBCode)

---

## Feature: Mass Action Cost Preview for Non-Wheel Actions (COMPLETE)

### Problem
Cost preview only showed on the follower command wheel (`UIFollowerWheelInteractionItem.GetDescription()`). Farm actions (water/fertilize) use a simple interaction label. Animal commands (pet, clean, feed, milk, shear) use the same wheel UI but `GetMassActionTargetCount()` didn't handle their FollowerCommands values.

### Fix
1. **Animal wheel commands** — Extended `GetMassActionTargetCount()` switch with: `PetAnimal`, `Clean`, `MilkAnimal`, `Harvest`, and feed commands (via `FastCollectingPatches.FeedCommands` HashSet, changed from private to internal)
2. **Farm plots** — Added `FarmPlot.GetLabel()` postfix that appends cost text when water/fertilize labels shown
3. **Refactored** — Extracted `GetCostPreviewTextForCount(int count)` from `GetCostPreviewText(cmd)` so both wheel and label paths share cost string formatting

### Files Modified
- `CultOfQoL/Core/MassActionCosts.cs` — Added animal cases, extracted `GetCostPreviewTextForCount`, added `using CultOfQoL.Patches.Systems`
- `CultOfQoL/Patches/Gameplay/InteractionPatches.cs` — Added `FarmPlot_GetLabel` postfix
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Changed `FeedCommands` private → internal

---

## Feature: DiscipleCollectionShrine Full Coverage (COMPLETE)

### Problem
`Interaction_DiscipleCollectionShrine` (Collected Shrine of Disciples, SoulMax=150) had zero mod coverage — no fast collecting, mass collect, instant collect, or SoulMax multiplier.

### Implementation
Added 4 patches mirroring existing shrine patterns:
1. **Fast collecting** — Postfix on `Update`: zeroes `Delay` and maxes `AccelerateCollection` (0.09f)
2. **Instant collect** — Postfix on `OnInteract` (HarmonyPriority.High): drains all souls at once with visual cap at 10, same soul/gold logic as `BuildingShrine_OnInteract_Postfix`
3. **Mass collect** — Postfix on `OnInteract`: iterates `Interaction_DiscipleCollectionShrine.Shrines` list, calls OnInteract on each with SoulCount > 0. Reuses `MassCollectFromPassiveShrines` config.
4. **SoulMax multiplier** — Postfix on `Structures_Shrine_Disciple_Collection.SoulMax` getter: calls existing `AdjustSoulMax()`

### Files Modified
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Added `CollectAllDiscipleShrinesRunning` guard, 3 patches + coroutine
- `CultOfQoL/Patches/Structures/StructurePatches.cs` — Added `Structures_Shrine_Disciple_Collection_SoulMax` postfix

---

## Feature: Mass Fill Troughs (COMPLETE)

### Implementation
Hooks `Interaction_RanchTrough.OnInteract`, waits one frame for `UIItemSelectorOverlayController` to appear (same pattern as wolf traps), wraps `OnItemChosen` callback:
1. Original callback fires (deposits 1 item via flying visual)
2. `FillAllTroughs()` fills triggered trough to capacity via direct `Structure.DepositInventory`
3. Iterates `Interaction_RanchTrough.Troughs`, fills each non-full/non-reserved trough to capacity
4. Hides selector if triggered trough is full

### Config
`MassFillTroughs` (bool, default false) in MassAnimalSection, Order 1

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassFillTroughs`
- `CultOfQoL/Plugin.cs` — Binding in MassAnimalSection
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Added `#region Mass Fill Troughs` with postfix, coroutine, fill method

---

## Feature: Mass Fill Toolsheds, Medic Stations, Mass Plant Seeds (COMPLETE)

All three implemented in `FastCollectingPatches.cs` using same selector-hook pattern as troughs.

### Toolshed / Carpentry Station (`Interaction_Toolshed`)
- Same pattern as troughs: hook `OnInteract`, wrap `OnItemChosen`, fill triggered + all others
- Items: LOG, STONE. Static list: `Toolsheds`. In-game name: "Carpentry Station"
- Config: `MassFillToolsheds` in MassFarmSection (Order 4)

### Medic Station (`Interaction_Medic`)
- Same pattern. Items: FLOWER_RED, CRYSTAL, MUSHROOM_SMALL. Static list: `Medics`
- Config: `MassFillMedicStations` in MassFarmSection (Order 3)

### Farm Plot Seed Planting (`FarmPlot`)
- Different pattern: `HideOnSelection = true` (single pick), one seed per plot
- Hook `OnInteract` postfix (when `CanPlantSeed()`), wrap selector `OnItemChosen`
- After original plants in triggered plot, iterate `FarmPlot.FarmPlots` for empty plots
- Direct call `StructureBrain.PlantSeed(chosenItem)` + `UpdateCropImage()` + `checkWaterIndicator()`
- Config: `MassPlantSeeds` in MassFarmSection (Order 7)

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassFillToolsheds`, `MassFillMedicStations`, `MassPlantSeeds`
- `CultOfQoL/Plugin.cs` — 3 bindings in MassFarmSection, renumbered existing orders
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Added 3 new regions

---

## Lightning Rod / Ranch II Chests — Verification Only

Both use `Interaction_CollectResourceChest` which is already auto-collected by the existing `EnableAutoCollect` prefix. No code changes needed — just verify in-game.

---

## BUG: FarmPlot.GetLabel Postfix Crash (RESOLVED)

### Problem
Accessing `FarmPlot.FarmPlots` (static field) from inside a `FarmPlot.GetLabel()` Harmony postfix caused an uncatchable crash.

### Fix (applied in later session)
Patched the base class `Interaction.Label` getter instead, with type guards. Farm plot counts cached via `FarmPlot.Update` postfix (throttled 0.25s) using `StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>()`. The `Interaction.Label` postfix reads cached counts — never touches `FarmPlot.FarmPlots` directly.

---

## Open Issues
- All new features need in-game testing (toolshed, medic, seed mass fill)
- No changelog/README updates for any of these new features yet

## Commit History
- `3aeeb626` — Mass action cost preview, pet refinement, furnace scaling, translation dump
- CHANGELOG date updated from 07/02/2026 to 08/02/2026 for release
