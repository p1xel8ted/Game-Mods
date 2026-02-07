# Work Summary - MysticAssistantRedux - 2026-02-07

## Feature: Boss Skin "New" Alert Badge Fix

### Status: COMPLETE

**User Report:** Boss skin "new" alert badge doesn't appear in the follower forms menu after purchasing from the Mystic Assistant shop.

### Root Causes (Three Layers)

#### 1. Invariant skins filtered from forms menu
`WorshipperData.GetSkinsFromLocation()` unconditionally excludes `Invariant=true` skins. Three boss skins (Boss Death Cat, Boss Aym, Boss Baal) are Invariant, so no `IndoctrinationFormItem` was ever created for them — no UI element means no badge.

#### 2. `_singleAlerts` persistence blocks `AddOnce`
`AlertCategory<T>.AddOnce()` calls `Add()`, which rejects alerts already in `_singleAlerts` (the permanent "shown once" tracker). Once a boss skin alert was registered even once, `_singleAlerts` persisted it across sessions via MessagePack serialization, permanently blocking future `AddOnce` calls — even after `ReplaceDeprication` cleared the alert from `_alerts`.

#### 3. Auto-selection removes badge instantly
`UIFollowerFormsMenuController.OnShowStarted()` auto-selects the first form item via `OverrideDefaultOnce`. Boss skins (not in `MiscOrder`) sort to index -1 (before named entries), making them the first item. `IndoctrinationFormItem.OnSelect` fires `TryRemoveAlert()`, killing the badge in the same frame it was created.

### Fixes Applied

#### 1. `GetSkinsFromLocation_Postfix` — Include unlocked Invariant boss skins in forms menu
Postfix on `WorshipperData.GetSkinsFromLocation` that adds back unlocked Invariant boss skins. Scoped to skins where `Invariant=true`, `Skin[0].Skin.Contains("Boss")`, unlocked, and matching the queried `DropLocation`.

#### 2. `RegisterSkinAlert` — Clear `_singleAlerts` before `AddOnce`
Removes the alert key from `_singleAlerts` before calling `AddOnce`, ensuring purchase-time alerts always register regardless of previous session state.

#### 3. `ReplaceDeprication` Prefix+Postfix — Preserve boss skin alerts across save/load
Prefix snapshots active boss skin alerts (containing "Boss") from `_alerts` before `ReplaceDeprication` clears them. Postfix restores them directly to `_alerts`, bypassing the `_singleAlerts` check. Only restores alerts that were active before cleanup (user hasn't viewed them yet).

#### 4. `OnSelect` Suppression — Prevent auto-select from killing badges
Prefix on `UIFollowerFormsMenuController.OnShowStarted` sets a suppression flag. Prefix on `IndoctrinationFormItem.OnSelect` skips `TryRemoveAlert` while the flag is set. Postfix on `OnShowStarted` starts a coroutine to clear the flag after one frame.

#### 5. Cleaned up all diagnostic logging
Removed `[SkinAlert]`, `[SkinForms]`, `[BadgeDebug]`, and `[ClothingDebug]` verbose diagnostic logging.

### Files Modified

| File | Changes |
|------|---------|
| `MysticAssistantRedux/Patches/MysticShopPatches.cs` | All 5 fixes above |
| `MysticAssistantRedux/InventoryManager.cs` | Cleaned up `PopulateBossSkins` diagnostic logging (earlier session) |

### Architecture Notes

**Four-layer fix:**
1. **Data layer** (`GetSkinsFromLocation_Postfix`) — Makes Invariant boss skins visible in forms menus
2. **Alert layer** (`RegisterSkinAlert`) — Clears `_singleAlerts` blocker, registers correct alert key
3. **Persistence layer** (`ReplaceDeprication` prefix+postfix) — Preserves alerts across save/load via snapshot+restore
4. **UI layer** (`OnSelect` suppression) — Prevents auto-selection from removing badges on menu open

**Why patching GetSkinsFromLocation is the right chokepoint:**
- Single method used by all 3 forms menu controllers
- Also affects `SetUnlockedText` counters (X/Y collected)
- Postfix scoped to boss skins only via `Contains("Boss")` check

### Key Game Code References

| Code | Location | Relevance |
|------|----------|-----------|
| `WorshipperData.GetSkinsFromLocation()` | WorshipperData.cs:175-192 | Invariant filter |
| `AlertCategory<T>.Add()` | AlertCategory\`1.cs:34-43 | Checks both `_alerts` AND `_singleAlerts` |
| `AlertCategory<T>.AddOnce()` | AlertCategory\`1.cs:45-51 | Adds to `_singleAlerts` permanent tracker |
| `AlertBadge<T>.Configure()` | AlertBadge\`1.cs:24-35 | Badge show/hide logic |
| `IndoctrinationFormItem.OnSelect()` | IndoctrinationFormItem.cs:39 | Calls `TryRemoveAlert()` |
| `DataManager.ReplaceDeprication()` | DataManager.cs:4508-4511 | Strips Invariant alerts |
| `UIFollowerFormsMenuController.OnShowStarted()` | UIFollowerFormsMenuController.cs:204-264 | Auto-selects first item |

---

## Feature: Gate Non-Wheel Content Behind Config Options

### Status: COMPLETE

DLC necklaces and boss skins are opt-in via `ConfigEntry<bool>` (default: off).

### Changes Made

| File | Changes |
|------|---------|
| `Plugin.cs` | Added `EnableDLCNecklaces` and `EnableBossSkins` config entries (section "01. Extra Content", Order 2/1) |
| `InventoryInfo.cs` | DLC necklaces gated behind `Plugin.EnableDLCNecklaces.Value && DataManager.Instance.MAJOR_DLC`; boss skin shop item gated behind `Plugin.EnableBossSkins.Value` |
| `InventoryManager.cs` | `PopulateBossSkins()` early-returns when config off |
| `MysticShopPatches.cs` | All 6 boss skin patches gated: `GetSkinsFromLocation_Postfix`, `ReplaceDeprication` prefix/postfix, `OnShowStarted` prefix/postfix, `OnSelect` prefix |

Config is fully dynamic — shop inventory rebuilt each open. Descriptions note "Re-open the shop for changes to take effect."

---

## Feature: Localized Config UI

### Status: COMPLETE

### Changes Made

| File | Changes |
|------|---------|
| `Localization.cs` | **NEW** — 15-language dictionary (same pattern as `Namify/Localization.cs`). Keys: `DLCNecklacesName`, `DLCNecklacesDesc`, `BossSkinsName`, `BossSkinsDesc`. Uses `LocalizationManager.CurrentLanguage` with English fallback. |
| `Plugin.cs` | Config bindings use `Localization.*` for `DispName` and description via `ConfigurationManagerAttributes` |

Languages: English, Japanese, Russian, French, German, Spanish, Portuguese (Brazil), Chinese (Simplified), Chinese (Traditional), Korean, Italian, Dutch, Turkish, French (Canadian), Arabic

---

## Feature: Documentation Updates

### Status: COMPLETE

| File | Changes |
|------|---------|
| `Thunderstore/mysticassistantredux/CHANGELOG.md` | Reorganized 0.1.1: exclusive content (Apple skins/clothing/fleece) separate from optional config content (DLC necklaces, boss skins). Removed dev-only bug fix lines. Added localization line. Removed Apple decorations (not working). |
| `Thunderstore/mysticassistantredux/README.md` | Split into "Exclusive Content" (Apple skins/clothing/fleece) and "Optional Content (Config)" (DLC necklaces, boss skins). Removed Apple decorations. |
| `Thunderstore/mysticassistantredux/nexusmods_description.txt` | Same changes in BBCode format. |

---

## Feature: Apple Decoration Placement Fix

### Status: COMPLETE (tested, working in-game)

**Problem:** Apple decorations (Bush, Lantern, Statue, Vase, Well) crash with `InvalidKeyException` when placed from build menu. The `Addr_PlacementObject` GUIDs in `TypeAndPlacementObjects` scene data only resolve on Apple Arcade builds.

### Root Cause

Crash flow: `BuildMenuItem.OnButtonClicked()` → `Interaction_PlacementRegion.PlaceBuilding()` line 224 → `TypeAndPlacementObjects.GetByType(type).PlacementObject` → lazy-loads via `Addressables.LoadAssetAsync(Addr_PlacementObject)` → GUID doesn't exist in PC catalog → `InvalidKeyException` → null → `Instantiate(null)` → crash.

Runtime probe confirmed all 5 Apple decoration PREFABS load fine on PC — only the placement ghost GUIDs are broken.

### Failed Approaches

1. **TypeAndPlacementObjects.Awake() postfix** — `WaitForCompletion()` deadlocks during scene init
2. **Donor decoration ghost** — Worked but showed generic footprint instead of actual decoration
3. **Inactive runtime ghost template** — `Object.Instantiate` preserves active state → inactive clone → game freezes (TimeScale=0, no UI)
4. **Active template with empty ToBuildAsset** — `PlaceObject()` line 782 waits for `GetComponentInChildren<Structure>()` in infinite loop (timeout uses `Time.deltaTime` which is 0 at timeScale=0)
5. **Active template WITH ToBuildAsset** — `PlacementObject.Start()` modifies path in-place → clone inherits modified path → double-prefix

### Final Fix: Runtime ghost + PlacementObject.Start() prefix

Three-part approach:
1. **`PlaceBuilding_Prefix`** — Creates active `PlacementObject` ghost template (empty `ToBuildAsset`, `Bounds(1,1)`, `RotatedObject` child), caches per-type in `_ghostCache`, assigns to `entry._placementObject`
2. **`PlacementObject_Start_Prefix`** — Detects clone instances by `"(Clone)"` in name, injects correct `ToBuildAsset` path from `ExclusiveContent.AppleDecorationPrefabPaths`
3. Original `PlacementObject.Start()` runs → modifies path → loads real Apple decoration prefab → `Structure` component appears → `PlaceObject()` wait loop exits

### Files Modified

| File | Changes |
|------|---------|
| `MysticShopPatches.cs` | `PlaceBuilding_Prefix`, `CreateAppleDecorationGhost()`, `PlacementObject_Start_Prefix` |
| `ExclusiveContent.cs` | Added `AppleDecorationPrefabPaths` dictionary, updated doc comment |

### Key Game Code References

| Code | Location | Relevance |
|------|----------|-----------|
| `TypeAndPlacementObject.PlacementObject` | TypeAndPlacementObject.cs:26-40 | Lazy-load from Addr_PlacementObject, checks `_placementObject != null` |
| `PlacementRegion.PlayRoutine()` | PlacementRegion.cs:351 | `Object.Instantiate(PlacementGameObject)` |
| `PlacementObject.Start()` | PlacementObject.cs:40-66 | Modifies ToBuildAsset in-place, loads via Addressables |
| `PlacementRegion.PlaceObject()` | PlacementRegion.cs:782 | Waits for `GetComponentInChildren<Structure>()` — infinite loop without it |

---

## Fix: SecondaryLabel Garbled Text

### Status: COMPLETE

Changed `SecondaryLabel` from `DataManager.Instance.MysticKeeperName + "'s assistant"` to fixed string `"Mystic Assistant"`. The dynamic NPC name could contain arbitrary user-entered text from save data.

| File | Changes |
|------|---------|
| `MysticShopPatches.cs` | Line 17: `SecondaryLabel = "Mystic Assistant"` |

---

## Defensive Fix: Item Selector Stale Text Prevention

### Status: COMPLETE

Added `ItemSelector_OnShowStarted_Postfix` on `UIItemSelectorOverlayController.OnShowStarted` that calls `RefreshContextText()` when items exist. The vanilla game never calls `RefreshContextText()` during `OnShowStarted` — `OverrideDefault()` sets the default item but doesn't trigger `OnItemSelected()`. This means `_buttonPromptText` retains stale text from whatever last used the component. The postfix ensures the text is initialized correctly on open.

| File | Changes |
|------|---------|
| `MysticShopPatches.cs` | `ItemSelector_OnShowStarted_Postfix` |

---

## Feature: Configurable God Tear Cost with Per-Save Lock

### Status: COMPLETE

Configurable cost per item (1-10, default 3) with a one-way ratchet per save slot.

### Implementation

- `ConfigEntry<int> GodTearCost` with `AcceptableValueRange(1, 10)`, default 3
- **Button-only lock**: Cost is NOT auto-locked. User must explicitly click "Lock Cost for Save" button in Config Manager
- **One-way ratchet**: `GetEffectiveCost()` returns `Math.Max(configValue, lockedValue)` — cost can only increase
- **Per-save persistence**: `PlayerPrefs` keyed by `MysticAssistant_Cost_{SaveAndLoad.SAVE_SLOT}`
- **Save cleanup**: Postfix patches on `SaveAndLoad.DeleteSaveSlot` and `SaveAndLoad.ResetSave` clear the key
- **Confirmation popup**: Custom drawer button shows `PopupManager.ShowConfirmation()` dialog before locking
- **Config UI button states**: Shows "Lock at X" (unlocked) or "Locked at X" with optional "Increase to Y" (locked, config > locked)

### Files Modified

| File | Changes |
|------|---------|
| `Plugin.cs` | `GodTearCost` config, `GetEffectiveCost()`, `ClearCostForSlot()`, `DrawLockCostButton()` custom drawer, PopupManager setup |
| `InventoryInfo.cs` | `CreateTraderItem()` uses `Plugin.GetEffectiveCost()` |
| `Localization.cs` | `GodTearCostName`, `GodTearCostDesc` in 15 languages |
| `MysticShopPatches.cs` | `DeleteSaveSlot_Postfix`, `ResetSave_Postfix` for cleanup |
| `Shared/PopupManager.cs` | **NEW** — Generic popup with confirmation support, moved from CultOfQoL |
| `CultOfQoL/Core/PopupManager.cs` | **DELETED** — Replaced by shared version |
| `CultOfQoL/Plugin.cs` | Added `PopupManager.Title = PluginName` |
| `CultOfQoL/CultOfQoL.csproj` | Added shared PopupManager link |
| `MysticAssistantRedux.csproj` | Added shared PopupManager link |
