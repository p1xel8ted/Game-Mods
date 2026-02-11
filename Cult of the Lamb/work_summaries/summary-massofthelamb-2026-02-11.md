# MassOfTheLamb Extraction - Work Summary 2026-02-11

## Session Goal
Continue the MassOfTheLamb extraction from CultOfQoL — previous session created the new mod scaffolding and patch files but didn't finish cleanup. This session added it to the solution, fixed compilation errors, and completed the CultOfQoL cleanup.

---

## What MassOfTheLamb Is
A standalone mod extracted from CultOfQoL containing ALL mass action features: mass follower commands, mass animal actions, mass collect, mass farm, mass fill structures, mass action costs, and cost preview UI.

## Project Structure
```
MassOfTheLamb/
├── Plugin.cs              # Entry point, config bindings (partial class)
├── Configs.cs             # Config properties, MassWolfTrapMode & MassActionCostMode enums
├── GlobalUsings.cs        # Global usings
├── MassOfTheLamb.csproj   # References: Sirenix.*, DOTween, spine-unity, etc.
├── Core/
│   └── MassActionCosts.cs # Cost calculation, preview text, target counting
└── Patches/
    ├── MassActionEffects.cs       # Visual/gameplay effects for mass actions
    ├── MassAnimalPatches.cs       # Pet, feed, milk, shear, clean animals
    ├── MassCollectPatches.cs      # Beds, shrines, outhouses, compost, totems, poop, vomit
    ├── MassCostPreviewPatches.cs  # Interaction.Label postfix for cost previews
    ├── MassFarmPatches.cs         # Water, fertilize, plant seeds
    ├── MassFillPatches.cs         # Troughs, silos, toolsheds, medics, refinery, kitchen, pub, cooking fire
    ├── MassFollowerPatches.cs     # Level up, pet, sin extract, bless, inspire, bribe, etc.
    └── WheelCostPreviewPatch.cs   # Command wheel GetDescription postfix
```

## Plugin Details
- **GUID**: `p1xel8ted.cotl.MassOfTheLamb`
- **Name**: Mass of the Lamb
- **Version**: 0.1.0
- **Solution GUID**: `{D5E6F7A8-B9C0-1234-5678-9ABCDEF01234}`
- **Thunderstore dir**: `massofthelamb`
- **Dependencies**: BepInEx, ConfigurationManager 18.4.1

## Config Sections
- `── Mass Animal ──` (pet, clean, feed, milk, shear, trough fill)
- `── Mass Action Costs ──` (cost mode, preview, gold/time costs, faith reduction)
- `── Mass Collect ──` (god tears, beds, outhouses, shrines, compost, totems, poop, vomit)
- `── Mass Farm ──` (plant seeds, fertilize, water, toolsheds, medics, silos, scarecrows, wolf traps)
- `── Mass Follower ──` (notification threshold, bribe/bless/extort/intimidate/inspire/romance/bully/reassure/reeducate/level up/pet/sin extract)
- `── Mass Fill (Structures) ──` (refinery, cooking fire, kitchen, pub)

## Shared Code Links (.csproj)
- `../Shared/ConfigurationManagerAttributes.cs` → `Util/`
- `../Shared/Extensions.cs` → `Util/`
- `../Shared/PopupManager.cs` → `Core/`
- `../Shared/Helpers.cs` → auto-included via `Directory.Build.props`

---

## Compilation Fixes Applied This Session

### Added to Solution
- Added MassOfTheLamb to `CultOfTheLambMods.sln` with Debug/Release/Release-Thunderstore configs

### MassOfTheLamb Fixes
1. Added `Sirenix.Serialization.dll` reference to `.csproj` (needed by MassFillPatches)
2. Added `using MassOfTheLamb.Patches;` to `Core/MassActionCosts.cs` (namespace resolution)
3. Fixed `MassCollectPatches.FeedCommands` → `MassAnimalPatches.FeedCommands` (wrong class name)

### CultOfQoL Cleanup (removed extracted code)
**Files deleted:**
- `CultOfQoL/Core/MassActionCosts.cs`
- `CultOfQoL/Patches/UI/WheelCostPreviewPatch.cs`
- `CultOfQoL/Patches/Followers/MassActionEffects.cs`

**StructurePatches.cs** — removed mass pet animals, refinery/kitchen/pub/cooking fire mass fill, commented-out mass nurture (~215 lines)

**Configs.cs** — removed:
- `MassWolfTrapMode` enum
- `MassActionCostMode` enum
- ~60 mass action config properties (all Mass* entries, Fill*ToCapacity, CollectAllGodTearsAtOnce, etc.)
- Restored `using CultOfQoL.Core;` (still needed for `WriteOnce<T>`)

**Plugin.cs** — removed:
- 5 mass action section constants (MassAnimalSection, MassActionCostsSection, MassCollectSection, MassFarmSection, MassFollowerSection)
- All mass action config bindings (~240 lines across Mass Animal, Mass Action Costs, Mass Collect, Mass Farm, Mass Follower sections)
- 4 structure mass fill bindings (RefineryMassFill, CookingFireMassFill, KitchenMassFill, PubMassFill)

---

## Cross-Mod Compatibility Analysis

### Both mods installed = identical behavior to pre-split CultOfQoL

**Overlapping Harmony patches (all safe):**

| Patch Target | CultOfQoL | MassOfTheLamb | Why Safe |
|---|---|---|---|
| `Interaction_Bed.GiveReward` | Postfix: strips waits (FastCollecting) | Postfix: halves to 0.05f (MassCollectFromBeds) | Enumerator wrapping composes correctly |
| `BuildingShrine.OnInteract` | Postfix: instant collect all souls | Prefix: collect all god tears | Different patch types, different features |
| `DiscipleCollectionShrine.OnInteract` | Postfix (Priority.High): instant drain | Postfix: mass collect all shrines | CultOfQoL drains current, MassOfTheLamb triggers others |
| `LevelUpRoutine` | Postfix: cleanse illness/exhaustion | Postfix: mass level up all | Independent guards, both run |
| `OnFollowerCommandFinalized` | Prefix: elder work mode (GiveWorkerCommand_2/MakeDemand only) | Postfix: mass follower commands | Prefix only skips elder work, never mass commands |

**Bed collection speed matrix (preserved post-split):**
| Config | Per-soul delay | Between beds |
|---|---|---|
| Neither | 0.1f (vanilla) | N/A |
| MassCollectFromBeds only | 0.05f | 0.05f sequential |
| FastCollecting only | instant | N/A |
| Both | instant | 0.05f sequential |

---

## Still TODO
- Create Thunderstore directory (`Thunderstore/massofthelamb/`)
- Create manifest.json, README.md, CHANGELOG.md, nexusmods_description.txt
- No commits made this session
- In-game testing needed
- CultOfQoL changelog/README needs updating to note mass action features moved to separate mod

---

## Follow-Up Update (2026-02-11)

### Post-Review Fixes Applied
1. **Follower command safety hardening**
   - `MassOfTheLamb/Patches/MassFollowerPatches.cs` now guards `followerCommands` null/empty before accessing index 0.
   - Matching defensive guard added in `CultOfQoL/Patches/Followers/FollowerPatches.cs` for the elder-work prefix.

2. **Thunderstore packaging completed**
   - Added `Thunderstore/massofthelamb/manifest.json`
   - Added `Thunderstore/massofthelamb/README.md`
   - Added `Thunderstore/massofthelamb/CHANGELOG.md`
   - Added `Thunderstore/massofthelamb/nexusmods_description.txt`
   - Added `Thunderstore/massofthelamb/icon.png`

### Build/Packaging Status (Updated)
- `Debug` build: passes
- `Release` build: passes
- `Release-Thunderstore` build: passes
- Thunderstore artifact generated (version updated to 0.1.0)

### TODO Status
- ✅ Create Thunderstore directory (`Thunderstore/massofthelamb/`)
- ✅ Create manifest.json, README.md, CHANGELOG.md, nexusmods_description.txt
- ⏳ In-game runtime testing still recommended
- ⏳ CultOfQoL changelog/README should still be updated to note mass action extraction
