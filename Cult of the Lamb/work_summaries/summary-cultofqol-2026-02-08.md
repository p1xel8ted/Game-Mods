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

## Open Issues
- All changes need in-game testing

## Commit History
- `3aeeb626` — Mass action cost preview, pet refinement, furnace scaling, translation dump
- CHANGELOG date updated from 07/02/2026 to 08/02/2026 for release
