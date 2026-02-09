# Work Summary - MysticAssistantRedux - 2026-02-08

## Fix: Fleece and Clothing Collection Counter Bugs

### Status: COMPLETE

**User Report:** Game's collection counters show wrong totals. Fleeces show "31/31" (should be 32/32 to include Apple fleece 680). Clothes show "16/1" (denominator is wrong — should be 16/16).

### Bug 1: Fleece Counter (31/31 instead of 32/32)

**Root Cause:** `UIPlayerUpgradesMenuController.Start()` (game code line 183-204) iterates `_fleeceItems` and counts active/unlocked items to set `_fleeceCount.text`. MAR's `UIPlayerUpgradesMenuController_Start_Postfix` runs AFTER `Start()`, adding fleece 680 to `_fleeceItems`. The counter was already written before the Apple fleece was added.

**Fix:** After adding fleece 680 to `_fleeceItems`, re-iterate the array and recalculate `__instance._fleeceCount.text` using `LocalizeIntegration.FormatCurrentMax()` (same format the game uses).

### Bug 2: Clothing Counter ("/1" denominator)

**Root Cause:** The Apple clothing injection in `TailorManager_ClothingData_Postfix` used a static `_appleClothingInjected` boolean flag. Once set to `true`, the postfix never re-injected — even if the backing field `TailorManager.clothingData` was later reset to `null` (causing the getter to reload vanilla data from `Resources.LoadAll`). The game's counter methods (`TailorManager.GetClothingCount`, `TailorManager.GetUnlockedClothingCount`) iterate `TailorManager.ClothingData`, which would then return vanilla data without Apple clothing, producing a wrong denominator.

**Fix:** Replaced `_appleClothingInjected` boolean with `_lastInjectedClothingArray` (a `ClothingData[]` reference). The postfix now uses reference-equality (`__result == _lastInjectedClothingArray`) to skip only when the exact same array is returned. If the backing field is replaced with a fresh array, reference-equality fails, and the postfix checks for `Apple_1` in the array — re-injecting if missing. This is O(1) on the fast path.

### Files Modified

| File | Changes |
|------|---------|
| `MysticAssistantRedux/Patches/MysticShopPatches.cs` | `UIPlayerUpgradesMenuController_Start_Postfix`: added fleece counter recalculation after adding fleece 680 |
| `MysticAssistantRedux/Patches/MysticShopPatches.cs` | Replaced `_appleClothingInjected` boolean with `_lastInjectedClothingArray` reference. Updated `TailorManager_ClothingData_Postfix` to use reference-equality check. |

### Key Game Code References

| Code | Location | Relevance |
|------|----------|-----------|
| `UIPlayerUpgradesMenuController.Start()` | UIPlayerUpgradesMenuController.cs:183-204 | Fleece counter calculation — iterates `_fleeceItems`, counts active/unlocked, writes `_fleeceCount.text` |
| `UIPlayerUpgradesMenuController._fleeceCount` | UIPlayerUpgradesMenuController.cs:61 | TMPro text field for fleece counter display |
| `LocalizeIntegration.FormatCurrentMax()` | Game utility | Formats "X/Y" counter with RTL support |
| `TailorManager.ClothingData` (property) | TailorManager.cs:24-32 | Lazy-loads from Resources, caches in static field `clothingData` |
| `TailorManager.GetClothingCount()` | TailorManager.cs:44-63 | Counter denominator — iterates `ClothingData`, filters by category flags |
| `TailorManager.GetUnlockedClothingCount()` | TailorManager.cs:65-84 | Counter numerator — same iteration, also checks `UnlockedClothing.Contains()` |
| `UITailorMenuController.Show()` | UITailorMenuController.cs:94-121 | Tailor menu clothing counters (normal/special/winter/DLC tabs) |
| `UIAppearanceMenuController_Outfit.OnShowStarted()` | UIAppearanceMenuController_Outfit.cs:116-129 | Follower appearance outfit counters (normal/special) |

### Architecture Notes

**Fleece counter fix pattern:** When a game method calculates UI text during its body, and a Harmony postfix adds new data, the UI text is stale. Fix: re-run the same counting logic after modification. This is cleaner than transpiling the counter out of Start().

**Reference-equality vs boolean flag:** Static boolean flags that guard "inject once" logic are fragile when the guarded data can be independently replaced (e.g., `TailorManager.clothingData` reloaded from `Resources.LoadAll`). Storing a reference to the last injected array provides O(1) staleness detection — if the game replaces the array, the reference won't match, triggering re-injection.

---

## Localization: Shop Labels and Interaction Prompt

### Status: COMPLETE

**Problem:** 6 user-facing strings were hardcoded English — shop item labels in `InventoryInfo.cs` and the "Mystic Assistant" interaction prompt in `MysticShopPatches.cs`. Config UI was already localized, but these in-game strings were not.

### Strings Localized

| Key | English | Used In |
|-----|---------|---------|
| `MysticAssistantLabel` | "Mystic Assistant" | `MysticShopPatches.cs` — SecondaryLabel interaction prompt |
| `ShopLabelRelic` | "Relic" | `InventoryInfo.cs` — shop label for SOUL_FRAGMENT |
| `ShopLabelSkinApple` | "Follower Skin (Apple)" | `InventoryInfo.cs` — shop label |
| `ShopLabelDecoApple` | "Decoration (Apple)" | `InventoryInfo.cs` — shop label |
| `ShopLabelOutfitApple` | "Outfit (Apple)" | `InventoryInfo.cs` — shop label |
| `ShopLabelSkinBoss` | "Follower Skin (Boss)" | `InventoryInfo.cs` — shop label |

6 keys × 15 languages = 90 new dictionary entries in `Localization.cs`.

### Files Modified

| File | Changes |
|------|---------|
| `MysticAssistantRedux/Localization.cs` | Added 6 property accessors + 90 dictionary entries across 15 languages |
| `MysticAssistantRedux/InventoryInfo.cs` | Replaced 5 hardcoded strings with `Localization.*` calls in `GetShopLabelByItemType()` |
| `MysticAssistantRedux/Patches/MysticShopPatches.cs` | Replaced `"Mystic Assistant"` with `Localization.MysticAssistantLabel` |
| `Thunderstore/mysticassistantredux/CHANGELOG.md` | Broadened localization line to include shop labels and interaction prompts |
| `Thunderstore/mysticassistantredux/README.md` | Added "Localization" subsection |
| `Thunderstore/mysticassistantredux/nexusmods_description.txt` | Added matching BBCode localization section |

### Notes

- Config descriptions (`DLCNecklacesDesc`, `BossSkinsDesc`, `GodTearCostDesc`) and `DispName` values are evaluated at `Awake()` time — before I2 Localization initializes — so they always resolve to English. This is the same accepted limitation as Namify.
- Character safety audit confirmed no BepInEx config-breaking characters in any language (Turkish special chars, Arabic RTL, CJK, accented Latin all safe for their usage contexts).
- Format string placeholders (`{0}`, `{1}`, `{2}`) verified correct in all 15 languages.
- Apple fleece label was already localized via `LocalizationManager.GetTranslation("TarotCards/Fleece680/Name")` — no change needed.
