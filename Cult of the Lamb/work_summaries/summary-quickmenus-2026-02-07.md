# QuickMenus - Work Summary 2026-02-07

## Current State
QuickMenus v0.1.0 — hotkey-based quick access to 4 game menus (F1-F4). Not yet published to Thunderstore. All menus work anywhere (not just at base). Builds successfully with 0 warnings/errors.

## What Was Done This Session

### 1. Initial Cleanup
- Removed `IsAtBase()` method and all commented-out call sites
- Removed "(base camp only)" and "(requires tailor building)" from config descriptions
- Commented out tailor structure gate (still attempts to find tailor, passes null if not found)

### 2. Hide Non-functional UI Elements
Each menu had elements hidden that imply full functionality outside their normal context:

**Build Menu (`OpenBuildMenu`)**
- Hidden: accept/confirm button prompt via `_controlPrompts.HideAcceptButton()`
- Hidden: "Edit Buildings" text — original GO destroyed, field replaced with dummy GO so `Update()`'s per-frame `SetActive()` doesn't NRE
- Title changed from "Build" to localized "Structures" — finds `I2.Loc.Localize` on GO named `"Header Text"`, disables it, sets `TextMeshProUGUI.text` from `StructuresTranslations` dictionary

**Tailor Menu (`OpenTailorMenu`)**
- Forced to Customize tab only (tab index 1) — `DefaultTabIndex = 1` set before `Show()`
- All 3 tab buttons hidden (`tabs[0/1/2].gameObject.SetActive(false)`)
- Tab navigation arrows hidden via `SetNavigationVisibility(false)`
- Title: cloned `"Header Text"` GO from build menu template into tab navigator, set to localized "Clothing" via `ClothingTranslations` dictionary
- Hidden: `_cookButton`, `_cookButtonRectTransform`, `_tailorQueue`, `addToQueueButton`, `removeToQueueButton`
- Scroll view extended downward: `scrollRt.offsetMin.y = 0f` to fill space from hidden craft queue

**Player Upgrades Menu (`OpenPlayerUpgradesMenu`) — now Fleeces-only**
- Hidden: `_ritualItem` + `_ritualItemAlert` (doctrine stones)
- Hidden: `_crystalDoctrineItem` + `_crystalDoctrineItemAlert`
- Hidden: `"Ritual Content"` container (found by GO name, prefab-only element)
- Hidden: `_crownHeader`, `_crownContainer`, `_crownDLCContainer`, `_crownAbilityCount` — uses `OnShow` callback to re-hide after `Start()` re-enables them next frame
- Accept button: `_acceptPromptContainer` destroyed (not just hidden) — `ShowAcceptButton()` null-checks before `SetActive(true)`, so destroying makes all future Show calls no-ops. Prevents `FocusCardFleece` and `FocusCard` cancel paths from re-showing it.
- Title: finds `Localize` on GO named `"Header"`, copies `mTerm` from `_fleeceHeader`'s Localize via `SetTerm()` — preserves I2 localization natively
- Hidden: `_fleeceHeader` (redundant after title rename)
- Fleece equipping works (clicking unlocked fleece = equip + close). Purchasing locked fleeces left functional (only works at temple altar).
- Fleece customization (left/right variant switching) fixed via Harmony prefix — see section 6 below.

**Follower Forms Menu** — no changes needed, already view-only

### 3. Toggle & Switch Hotkeys
- Hotkeys are now toggles: press F1 to open, press F1 again to close
- Pressing a different hotkey while a menu is open closes the current and opens the new one
- Implementation: `_activeMenu` (UIMenuBase) and `_activeMenuKey` (string) track current state
- `HandleKey(string key, Action open)` dispatches: same key = `Hide(true)` (toggle off), different key = `Hide(true)` then `open()` (switch)
- `Hide(true)` = immediate/synchronous close, fires `OnHidden` → `ResumeGame()` which clears state
- `ResumeGame()` clears `_activeMenu` and `_activeMenuKey`

### 4. Localization
- Build menu: `StructuresTranslations` dictionary with all 15 game languages, `GetLocalizedStructures()` helper
- Tailor menu: `ClothingTranslations` dictionary, `GetLocalizedClothing()` helper
- Player upgrades: Uses I2 natively — copies `mTerm` from fleece header's Localize to menu title's Localize via `SetTerm()`, no manual dictionary needed

### 5. Project Changes
- Added `Unity.TextMeshPro` reference to `QuickMenus.csproj`
- Added `global using Lamb.UI.Rituals;` and `global using TMPro;` to `GlobalUsings.cs`

### 6. Fleece Customization Fix (Harmony Patch)
- `FleeceInfoCard.Update()` uses `Time.deltaTime` for `selectionDelay` countdown — returns 0 when `Time.timeScale = 0` (our paused state), so left/right input never fires
- Fix: Harmony prefix on `FleeceInfoCard.Update()` subtracts `Time.unscaledDeltaTime` when timeScale is 0. Original method then subtracts `Time.deltaTime` (0), net effect = real-time countdown.
- Added `Harmony.CreateAndPatchAll()` to `Awake()`
- Added global usings: `HarmonyLib`, `System.Reflection`, `src.UI.InfoCards`

### 7. I2 Localization Term Investigation
- Searched `ScriptLocalization.cs` (66K+ lines) for standalone "Structures" and "Clothing" terms — none exist
- Build menu and tailor menu manual translation dictionaries are the correct approach (game has no I2 terms for these as menu titles)
- Player upgrades menu `SetTerm()` approach only works because `_fleeceHeader`'s Localize already has the right term

## Files Modified
- `QuickMenus/Plugin.cs` — all menu logic, toggle/switch system, localization, Harmony patch for fleece customization, accept button destroy pattern
- `QuickMenus/GlobalUsings.cs` — added `Lamb.UI.Rituals`, `TMPro`, `HarmonyLib`, `System.Reflection`, `src.UI.InfoCards`
- `QuickMenus/QuickMenus.csproj` — added `Unity.TextMeshPro` reference

## Key Technical Notes

### Menu Instantiation Pattern
The game always re-instantiates menus from templates — `template.Instantiate()` clones the prefab, and `OnHideCompleted()` calls `Object.Destroy(this.gameObject)`. No caching needed.

### Start() Timing Issue
`UIPlayerUpgradesMenuController.Start()` runs on the next frame after `Instantiate()`. It explicitly calls `SetActive(true)` on `_crownHeader` and `_crownContainer`, undoing any hides from the current frame. Fix: use `menu.OnShow` callback (fires from `DoShow()` coroutine after `yield return null`, which is after `Start()`).

### Build Menu Update() Gotcha
`UIBuildMenuController.Update()` sets `_editBuildingsText.SetActive(...)` every frame. Solution: destroy original GO, replace field reference with dummy GO (same pattern could apply elsewhere).

### Cloning "Header Text" from Build Menu Template
- Works for tailor menu: `Object.Instantiate(t.gameObject, menu.tabNavigator.transform)` — search by GO name `"Header Text"` on the build template
- Did NOT work for player upgrades menu when parented to `menu.transform` (rendering/positioning issues)
- Player upgrades uses `SetTerm()` on existing `Localize` instead (cleaner approach)

### Finding Prefab-only Elements
Some GOs (like `"Ritual Content"`, `"Header"`) exist only in the prefab hierarchy with no code references. Found via `GetComponentsInChildren<Transform>(true)` or `GetComponentsInChildren<I2.Loc.Localize>(true)` with name matching.

## Key Technical Notes (continued)

### Accept Button Destroy Pattern
`FocusCardFleece` and `FocusCard` coroutines call `_controlPrompts.ShowAcceptButton()` on cancel. `ShowAcceptButton()` null-checks `_acceptPromptContainer` before `SetActive(true)`. Destroying `_acceptPromptContainer` makes the null check fail silently — permanent fix for the menu instance lifetime.

### FleeceInfoCard.Update() Time.deltaTime Issue
`selectionDelay -= Time.deltaTime` — `Time.deltaTime` is 0 when `Time.timeScale = 0`. The Harmony prefix subtracts `Time.unscaledDeltaTime` instead, cooperating with the original (which subtracts 0).

### No I2 Terms for "Structures" or "Clothing"
Exhaustive search of `ScriptLocalization.cs` — no standalone terms exist. Manual translation dictionaries are correct. `SetTerm()` only viable for fleeces (via existing `_fleeceHeader` Localize).

## Open Issues / Known Limitations
- Tailor menu passes `null` if no tailor building exists — user tested, works fine
- Purchasing locked fleeces from hotkey menu would crash outside temple (`Interaction_TempleAltar.Instance`). Not prevented — user must use temple altar for purchases.
- Build menu: selecting a building fires `ChosenBuilding` → `OnBuildingChosen` (null) → menu just closes. Benign.
- `StructuresTranslations` and `ClothingTranslations` are machine-translated — may need review
- Build menu title search relies on GO name `"Header Text"` matching

## Next Steps (Potential)
- Verify translations with native speakers

---

## Documentation Updates (Later Session)

### Cross-Mod Links
- Added Quick Menus (self) to "My Other Mods" in README.md (MAR was already present)
- Added MAR and QM to nexusmods_description.txt

### Thunderstore Manifest
- Set `website_url` to `https://www.nexusmods.com/cultofthelamb/mods/64`
- Added `p1xel8ted-BepInEx_Configuration_Manager-18.4.1` dependency

### Files Modified
- `Thunderstore/quickmenus/README.md` — added QM to My Other Mods
- `Thunderstore/quickmenus/nexusmods_description.txt` — added MAR + QM to My Other Mods, NexusMods URL for QM
- `Thunderstore/quickmenus/manifest.json` — set website_url, added CM dependency
