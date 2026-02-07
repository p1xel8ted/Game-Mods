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

## Feature: Mass Action Costs (COMPLETE)

### Goal
Add configurable costs (gold, time, faith reduction) to mass actions so they don't trivialize game balance.

### Config (in "── Mass Action Costs ──" section)
- `MassActionGoldCost` (int, 0-50, default 0) — gold per target affected
- `MassActionTimeCost` (int, 0-120, default 0) — game minutes per target affected
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
