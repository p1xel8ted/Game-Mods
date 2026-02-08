# CultOfQoL Work Summary - 2026-02-07

## Task: Mass Action Notification Consolidation

### Goal
When mass follower actions affect many followers, consolidate per-follower notifications into a single summary notification instead of flooding the notification panel.

### Changes Made

**New config:**
- `MassNotificationThreshold` (int, default 3, range 0-50) in Mass Follower section
- When affected count > threshold, individual notifications suppressed, single summary shown
- Set to 0 to always show summary

**Implementation:**
- Uses game's own `NotificationCentre.NotificationsEnabled` static flag (same pattern as `BlizzardMonster`)
- `NotifySuppressBegin`/`NotifySuppressEnd` helper methods in `FollowerPatches`
- All 10 mass action loops + mass level up coroutine wrapped with suppression
- Summary shows e.g. "Blessed 15 followers" with Positive flair

### Files Modified
- `CultOfQoL/Configs.cs` — Added `MassNotificationThreshold` property
- `CultOfQoL/Plugin.cs` — Bound config (Order 14, top of Mass Follower section)
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — Added helpers, wrapped all 11 mass action loops
- `Thunderstore/cult/CHANGELOG.md` — Added to 2.4.1 entry
- `Thunderstore/cult/README.md` — Added to Mass Actions feature list
- `Thunderstore/cult/nexusmods_description.txt` — Added to Mass Actions feature list (BBCode)

### Key Decisions
- Default threshold of 3 (most useful with larger cults)
- Faith modifications still apply correctly — only UI notification display is suppressed
- Lazy LINQ evaluation changed to `.ToList()` to get count before loop starts
- No restart required for threshold changes (applies immediately)

### Open Issues
- None

---

## Documentation Updates (Later Session)

### Changelog
- Updated 2.4.1 date from 05/02/2026 to 07/02/2026

### Cross-Mod Links
- Added Mystic Assistant Redux and Quick Menus to "My Other Mods" in README.md and nexusmods_description.txt
- Final order: CultOfQoL, MAR, Rebirth, Namify, SkipOfTheLamb, SkipOfTheLambLite, TraitControl, QuickMenus, GlyphOverride (9 entries)

### Thunderstore Manifest
- Added `p1xel8ted-BepInEx_Configuration_Manager-18.4.1` dependency

### Files Modified
- `Thunderstore/cult/CHANGELOG.md` — date fix
- `Thunderstore/cult/README.md` — added MAR + QM to My Other Mods
- `Thunderstore/cult/nexusmods_description.txt` — added MAR + QM to My Other Mods
- `Thunderstore/cult/manifest.json` — added CM dependency

---

## Feature: Rot Fertilizer Decay (COMPLETE - code written, needs testing)

### Goal
Make rot fertilizer's ground warming effect temporary instead of permanent, adding strategic depth to winter farming.

### How It Works
- `POOP_ROTSTONE` sets `StructuresData.DefrostedCrop = true` — vanilla never resets this
- New feature: tracks when each plot was defrosted, expires warming after configurable days
- When warming expires during winter: ground re-freezes, growing crops wither (unless near Crop Grower)

### Config
- `RotFertilizerDecay` (bool, default false) in new "── Farm ──" section
- `RotFertilizerDuration` (int, default 5, range 1-30 days)

### Implementation
- `Dictionary<int, int>` maps structure ID → day defrosted
- Postfix on `AddFertilizer`: records defrost day when POOP_ROTSTONE applied
- Postfix on `OnNewPhaseStarted`: checks if `CurrentDay - defrostDay >= duration`, resets `DefrostedCrop = false`
- Persistence: JSON file per save slot in `BepInEx/config/CultOfQoL/defrost_{SAVE_SLOT}.json`
- Save/Load/Delete hooks for persistence
- Pre-existing defrosted plots (before feature enabled) get timer set to current day on first check
- `FarmPlot.UpdateWatered()` called to refresh visuals when warming expires
- Uses `Plugin.LogType.Error` (not `LogType.Error`) to avoid ambiguity with `UnityEngine.LogType`
- `DeleteSaveSlot` param name is `saveSlot` (method parameter, not `___slot` field injection)

### Files Modified
- `CultOfQoL/Configs.cs` — Added `RotFertilizerDecay`, `RotFertilizerDuration` properties
- `CultOfQoL/Plugin.cs` — Added `FarmSection` constant, bound 2 config entries between Mass Farm and Mass Follower

### Files Created
- `CultOfQoL/Patches/Structures/FarmPlotPatches.cs` — All decay logic + JSON persistence + save hooks

### Key Game Code References
- `Structures_FarmerPlot.AddFertilizer()` — sets `DefrostedCrop = true` for POOP_ROTSTONE (line 259-269)
- `Structures_FarmerPlot.IsGroundFrozen` — returns false when DefrostedCrop is true (line 40-46)
- `Structures_FarmerPlot.CanGrow()` — returns true when DefrostedCrop is true (line 57-62)
- `FarmPlot.GetFarmPlot(int ID)` — static lookup by structure ID
- `TimeManager.CurrentDay` — backed by `DataManager.Instance.CurrentDayIndex`

---

## Fix: God Tear Double-Collection Bug (COMPLETE)

### Problem
"Collect All God Tears At Once" was collecting double tears (e.g. 2 from a shrine with only 1). The old postfix on `GiveGodTearIE` ran after deferred `SoulCustomTarget` callbacks (from instant devotion collection) could increment `UpgradeSystem.AbilityPoints` via level-ups, causing it to see an inflated count.

### Fix
Replaced `GiveGodTearIE` postfix with `OnInteract` prefix on `BuildingShrine`. The prefix runs before any other code can modify AbilityPoints, snapshots the count, and pre-collects extras. Guards: `!Activating`, `!HasUnlockAvailable`, `DeathCatBeaten`.

### Files Modified
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs`

---

## Change: Mass Action Costs Float with 0.25 Stepping (COMPLETE)

### What Changed
- `MassActionGoldCost` and `MassActionTimeCost` changed from `ConfigEntry<int>` to `ConfigEntry<float>`
- Added `RoundToQuarter` helper and `SettingChanged` handlers for 0.25 stepping
- `MassActionCosts.TryDeductCosts` uses `Mathf.CeilToInt` for gold deduction

### Files Modified
- `CultOfQoL/Configs.cs` — int → float type change
- `CultOfQoL/Plugin.cs` — float bindings, AcceptableValueRange<float>, SettingChanged handlers, RoundToQuarter
- `CultOfQoL/Core/MassActionCosts.cs` — Mathf.CeilToInt, float comparisons

---

## Logging Standardization (COMPLETE)

### What Changed
Audited all 195 log calls across 24 files. Established consistent format:
- Runtime: `Plugin.WriteLog("[Tag] message")`
- Transpilers: `Plugin.Log.LogInfo/Warning("[Transpiler] message")`

Fixed 21 untagged calls across 7 files. Added `[CallerMemberName]` to `MassActionCosts.TryDeductCosts` and `GetFaithMultiplier` for automatic caller identification. Added detailed cost logging (gold check/deduction, time advancement, faith multiplier).

### Files Modified
- `CultOfQoL/Core/MassActionCosts.cs` — [CallerMemberName], detailed logging
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — [ElderWork]/[MassAction]/[LevelUp] tags
- `CultOfQoL/Patches/Gameplay/InteractionPatches.cs` — [Interaction]/[MassWater]/[MassFertilize] tags
- `CultOfQoL/Patches/Gameplay/PickUps.cs` — [PickUps] tags
- `CultOfQoL/Patches/Gameplay/RitualEnrichmentNerf.cs` — [EnrichmentNerf] tag
- `CultOfQoL/Patches/Structures/FarmPlotPatches.cs` — [RotFertilizer] tags
- `CultOfQoL/Patches/Structures/StructurePatches.cs` — [ResourceProduction]/[StationAge] tags
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — [Collect All God Tears] logging

---

## Feature: Mass Action Costs (COMPLETE)

### Goal
Add configurable costs (gold, time, faith reduction) to mass actions so they don't trivialize game balance.

### Config (in "── Mass Action Costs ──" section)
- `MassActionGoldCost` (float, 0-50, default 0, 0.25 stepping) — gold per target affected
- `MassActionTimeCost` (float, 0-120, default 0, 0.25 stepping) — game minutes per target affected
- `MassFaithReduction` (int, 0-100, default 0) — % faith reduction for mass Bless/Inspire

All default to 0 (free), preserving existing behavior unless user opts in.

### Implementation
- `Core/MassActionCosts.cs` — helper class with `TryDeductCosts(count)` and `GetFaithMultiplier()`
- Gold: total = goldPerTarget * count; checks affordability before loop, shows "Not enough gold" notification and skips mass action if insufficient (original single interaction still works)
- Time: `TimeManager.CurrentGameTime += timePerTarget * count`
- Faith: multiplier passed to `CultFaithManager.AddThought()` overload; skipped entirely at 0%
- Cost check inserted after eligible target filtering but before action loop in all 17 mass actions:
  - 10 follower actions (Reassure, Reeducate, Bully, Romance, Pet, Extort, Inspire, Intimidate, Bless, Bribe)
  - 5 animal actions (Clean, Feed, Milk, Shear, Pet Animals)
  - 2 farm actions (Water, Fertilize)
- NOT applied to mass collect/fill/level up/scarecrows

### Code Cleanup (same changeset)
- `FastCollectingPatches.cs`: Refactored Clean/Feed/Milk/Shear from imperative foreach+counter to LINQ `.Where().ToList()` (-50 lines)
- `Shared/ConfigurationManagerAttributes.cs`: Simplified `System.Action`/`System.Func` to `Action`/`Func`
- `Shared/Helpers.cs`: Removed unused `using BepInEx`/`using BepInEx.Logging`

### Files Modified
- `CultOfQoL/Configs.cs` — 3 new config properties + 2 rot fertilizer properties
- `CultOfQoL/Plugin.cs` — `MassActionCostsSection` + `FarmSection` constants, 5 new config bindings
- `CultOfQoL/Patches/Followers/FollowerPatches.cs` — Cost check in 10 mass action blocks
- `CultOfQoL/Patches/Followers/MassActionEffects.cs` — Faith multiplier in ApplyBless + ApplyInspire
- `CultOfQoL/Patches/Gameplay/InteractionPatches.cs` — Cost check in mass water + fertilize
- `CultOfQoL/Patches/Structures/StructurePatches.cs` — Cost check in mass pet animals
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Cost check + LINQ refactor in 4 animal actions
- `Shared/ConfigurationManagerAttributes.cs` — Simplified type references
- `Shared/Helpers.cs` — Removed unused imports

### Files Created
- `CultOfQoL/Core/MassActionCosts.cs` — Cost deduction helper class

### Key Game Code References
- `CultFaithManager.AddThought(thought, followerID, faithMultiplier)` — line 253: `data.Modifier *= faithMultiplier`
- `Inventory.GetItemQuantity(ITEM_TYPE.BLACK_GOLD)` / `Inventory.ChangeItemQuantity(ITEM_TYPE.BLACK_GOLD, -amount)`
- `TimeManager.CurrentGameTime` — advanced by `SimulationManager.Update()` every frame; phase transitions fire at 240 min boundaries

---

## Feature: Disable Soul Camera Shake (COMPLETE)

### Goal
Stop the camera shaking when devotion orbs hit the shrine during follower worship.

### Implementation
- Transpiler on `SoulCustomTarget.CollectMe()` redirects `CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.2f)` to `ConditionalSoulShake`
- Wrapper checks `Plugin.DisableSoulCameraShake.Value` before calling the real method
- Game has global `SettingsManager.Settings.Accessibility.ScreenShake` but that kills ALL shake including combat — this is targeted
- `ShakeCameraForDuration` has 4 params (float, float, float, bool=true) — wrapper must match all 4 since compiler pushes defaults onto IL stack

### Config
- `DisableSoulCameraShake` (bool, default false) in Collection section (Order 0)

### Files Modified
- `CultOfQoL/Configs.cs` — Added property
- `CultOfQoL/Plugin.cs` — Added binding
- `CultOfQoL/Patches/Systems/FastCollectingPatches.cs` — Transpiler + `ConditionalSoulShake` wrapper

---

## Feature: Suppress Notifications On Load (COMPLETE)

### Goal
Prevent the flood of individual notifications when loading a save.

### Implementation
- Static constructor subscribes to `SaveAndLoad.OnLoadComplete` (same pattern as `FarmPlotPatches`)
- On load: sets `NotificationCentre.NotificationsEnabled = false`
- Coroutine waits 3 seconds, calls `ClearNotifications()`, re-enables `NotificationsEnabled`
- `NotificationsEnabled` only gates static notifications (`PlayFollowerNotification`, etc.)
- Dynamic notifications (`UIDynamicNotificationCenter`) rebuild from game state independently — unaffected

### Config
- `SuppressNotificationsOnLoad` (bool, default false) in Notifications section (Order 8)

### Files Modified
- `CultOfQoL/Configs.cs` — Added property
- `CultOfQoL/Plugin.cs` — Added binding
- `CultOfQoL/Patches/UI/Notifications.cs` — Static ctor + `OnLoadComplete` + `ReEnableNotificationsAfterDelay` coroutine

---

## Feature: Refinery Poop to Rot Fertilizer (COMPLETE)

### Goal
Add a refinery recipe: 10 POOP → 1 POOP_ROTSTONE.

### Implementation
Two patches:
1. Modified existing `Structures_Refinery_GetCost` postfix — added `InventoryItem.ITEM_TYPE Item` parameter, injects recipe at top before cost-halving logic (so "Halve Refinery Costs" applies automatically)
2. New prefix on `UIRefineryMenuController.OnShowStarted` — appends `POOP_ROTSTONE` to `_refinableResources` array before the method iterates it to populate the UI

### Key Game Code
- `UIRefineryMenuController._refinableResources` (line 45) — hardcoded `ITEM_TYPE[]` array iterated in `OnShowStarted` (line 83)
- `Structures_Refinery.GetCost()` — switch statement returning `List<StructuresData.ItemCost>` for each recipe
- `Structures_Refinery.GetAmount()` — returns 1 for non-BLACK_GOLD items (so 1 POOP_ROTSTONE per cycle)
- `_dlcResources` — DLC-gated items; POOP_ROTSTONE is NOT in this list

### Config
- `RefineryPoopToRotPoop` (bool, default false) in Structures section (Order 28)

### Files Modified
- `CultOfQoL/Configs.cs` — Added property
- `CultOfQoL/Plugin.cs` — Added binding
- `CultOfQoL/Patches/Structures/StructurePatches.cs` — Modified GetCost postfix + added OnShowStarted prefix

---

## Documentation Updates (this session)
- `Thunderstore/cult/CHANGELOG.md` — Added 3 features to 2.4.1 entry
- `Thunderstore/cult/README.md` — Added to Collection, Menu & UI, Crafting sections
- `Thunderstore/cult/nexusmods_description.txt` — Same (BBCode)

## Open Issues
- All features need in-game testing
- Changes not yet committed
