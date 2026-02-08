# GYK Mod Audit — Work Summary 2026-02-07

## Progress
34 of 39 mods complete. Sessions today completed #27-#34.

## Session Work

### #27 Exhaustless (3.4.9 → 3.5.0)
- Removed GYKHelper: `Actions.GameBalanceLoad` → `[HarmonyPostfix]` on `GameBalance.LoadGameBalance`
- ConfigurationManagerAttributes → `<Compile Include>` link
- **Bug fix**: `WaitingGUI_Update_Postfix` had `max_hp`/`max_energy` swapped — auto-wake compared energy to health max and vice versa. Bug existed since original QMod version. Fixed to match game code (`WaitingGUI.cs:88,97`).
- **Bug fix**: `MainGame_OnEquippedToolBroken` foreach loop equipped ALL matching tools → changed to `FirstOrDefault` + single equip
- Added `__instance._is_player` guard to `GetParam_Postfix` (performance — only player accumulates tiredness)
- Removed unnecessary `ref` on reference-type params

### #28 MaxButtonControllerSupport (1.3.7 → 1.3.8)
- Removed GYKHelper: `Actions.WorldGameObjectInteractPrefix` → `[HarmonyPrefix]` on `WorldGameObject.Interact`
- No ConfigurationManagerAttributes needed (no config bindings)
- Removed unnecessary `ref` across all 6 files (Harmony patches + MaxButtonCrafting + MaxButtonVendor internal methods)
- No bugs found — functional verification passed

### #29 BringOutYerDead (0.2.0 → 0.2.1)
- Removed GYKHelper: `Actions.EndOfDayPrefix` → `[HarmonyPrefix]` on `EnvironmentEngine.OnEndOfDay`
- `CrossModFields.Culture` / `GetLocalizedString` → own `GameCulture` + `SetUICulture()`
- `Tools.TutorialDone()` → local `TutorialDone()` with 14-quest list
- `Tools.ShowMessage(msg, pos, sayAsPlayer: true)` → inline `player.Say()`
- ConfigurationManagerAttributes → `<Compile Include>` link
- Removed unnecessary `ref` on 3 Harmony postfix/prefix params
- **Optimization**: Removed per-frame `LogicData("donkey")` allocation from `Update()`

### #30 DecompDelight (0.1.2 → 0.1.3)
- Removed stale `[BepInDependency("p1xel8ted.gyk.gykhelper")]` — no actual GYKHelper code used
- Changed `[Harmony]` → `[HarmonyPatch]`
- Removed `ref` from all 4 postfix params (ItemDefinition, Item, bool, List)
- No bugs found — functional verification passed (tooltip insertion, element mappings, survey checks all correct)

### #31 PrayTheDayAway (0.3.2 → 0.3.3)
- Removed GYKHelper: `CrossModFields.Culture` → local `GameCulture`, `Tools.ShowMessage` → inline `player.Say()` with `GJL.IsEastern()` check, `Tools.ShowLootAddedIcon` → inline `DropCollectGUI.OnDropCollected` + size swap + sound
- ConfigurationManagerAttributes → `<Compile Include>` link
- **Bug fix**: `PrayCraftGUI_OnFinishedPrayBuffAnimation` patched wrong method — both postfixes targeted `OnMiddlePrayBuffAnimation`. Fixed to target `OnFinishedPrayBuffAnimation` for the "lost prayer" notification.
- Removed `ref` from 4 params, fixed `[Harmony]` → `[HarmonyPatch]`

### #32 MiscBitsAndBobs (2.2.9 → 2.3.0)
- Removed GYKHelper (7 replacements): `Actions.GameStartedPlaying` → postfix on `GameSave.GlobalEventsCheck`, `CrossModFields.Lang` → `GJL.GetCurLng()`, `CrossModFields.Culture` → local `GameCulture`, `CrossModFields.GerryCinematicPlaying` removed (no mod sets it), `Tools.ShowAlertDialog` → `GUIElements.me.dialog.OpenOK`, `Tools.ModLoaded` removed (dead code), `Tools.WorkerHasBackpack` → inline inventory check
- ConfigurationManagerAttributes → `<Compile Include>` link
- Fixed typo: `[HarmonyAfter("pp1xel8ted.gyk.gykhelper")]` → removed (GYKHelper going away)
- Fixed `[HarmonyAfter("p1xel8ted.GraveyardKeeper.PrayTheDayAway")]` → `[HarmonyAfter("p1xel8ted.gyk.praythedayaway")]`
- Fixed version mismatch: csproj 1.6.6 + Plugin 2.2.9 → both 2.3.0
- Simplified Sprint detection: removed dead `Tools.ModLoaded` + `SprintTools` field, kept only `Harmony.HasAnyPatches`
- Removed `ref` from 7 params, changed `[Harmony]` → `[HarmonyPatch]`
- **Note**: `CinematicLetterboxingConfig` logic may be inverted — `!Plugin.CinematicLetterboxingConfig.Value` means the removal fires when config is FALSE. Pre-existing, not fixed in this pass.

### #33 AppleTreesEnhanced (2.7.8 → 2.7.9)
- Removed GYKHelper: `Actions.GameStartedPlaying` → postfix on `GameSave.GlobalEventsCheck`, `CrossModFields.Culture` (2 usages) → local `GameCulture` property
- ConfigurationManagerAttributes → `<Compile Include>` link
- Changed `CleanUpTrees` from `private` to `internal` (called from Patches postfix)
- Removed `ref` from 4 params, changed `[Harmony]` → `[HarmonyPatch]`
- No bugs found — functional verification passed

### #34 AutoLootHeavies (3.4.9 → 3.5.0)
- Removed GYKHelper (3 replacements): `Actions.WorldGameObjectInteract` → postfix on `WorldGameObject.Interact`, `CrossModFields.WorldObjects` → `WorldMap._objs`, `Tools.ShowMessage(msg, pos)` → local `ShowMessage` helper (Eastern: player.Say, non-Eastern: EffectBubblesManager)
- ConfigurationManagerAttributes → `<Compile Include>` link
- Removed `ref` from 4 params
- **Bug fix**: `DropOverheadItem` prefix missing null guard — `OverheadItemIsHeavy(__instance.overhead_item)` throws NRE when `overhead_item` is null. Game calls `DropOverheadItem` from 6 sites (attack, damage, docking) without pre-checking `has_overhead`. Added `!__instance.has_overhead` guard matching game's own pattern (`BaseCharacterComponent.cs:1110`).
- **Bug fix**: `ShowLootAddedIcon` hardcoded `item.definition.item_size = 2` on restore — changed to save/restore original value

## New Patterns Discovered

### #17: Tools.TutorialDone() inlining
- 14-quest list + `!IsInTutorial()` — used by BringOutYerDead, BeamMeUpGerry, SaveNow

### #18: Tools.ShowMessage sayAsPlayer simplification
- `sayAsPlayer: true` + defaults → `player.Say(msg, null, false, Think, None, true)` (Eastern path identical)

### #19-#20: Actions event replacements (already in MEMORY.md)

### #21: Tools.ShowMessage with custom speechBubbleType
- Eastern path IGNORES speechBubbleType (uses Think). Non-Eastern uses provided type.
- Inline: `var bubbleType = GJL.IsEastern() ? Think : [custom]; player.Say(msg, null, false, bubbleType, None, true)`

### #22: Tools.ShowLootAddedIcon inlining
- Temporarily sets `item.definition.item_size = 1`, calls `DropCollectGUI.OnDropCollected(item)`, restores original size, plays "pickup" sound

### #23: Tools.ModLoaded — dead code
- GYKHelper's `LoadedModsById/Name/FileName` lists are never populated → always returns false
- Use `Harmony.HasAnyPatches(guid)` for mod detection

### #24: Tools.ShowAlertDialog
- Wrapper for `GUIElements.me.dialog.OpenOK(text1, null, text2, separateWithStars, text3)`

### #25: CrossModFields.GerryCinematicPlaying
- After full audit: no mod sets this flag → remove checks

### #26: Actions.WorldGameObjectInteract
- Postfix on `WorldGameObject.Interact` with `(WorldGameObject __instance)` — fires AFTER interaction
- Different from `WorldGameObjectInteractPrefix` which is a prefix with `(__instance, other_obj)`

### #27: Tools.ShowMessage(msg, pos) with defaults (sayAsPlayer: false)
- Eastern: `player.Say(msg, null, false, Think, None, true)` (ignores pos)
- Non-Eastern: `EffectBubblesManager.ShowImmediately(pos, msg, Relation, true, 3f)` with zero-pos fallback to player.pos3 + 125f y

### #28: CrossModFields.WorldObjects
- `WorldMap._objs` — trivial property wrapper

### #35 QueueEverything (2.1.7 → 2.1.8)
- Removed GYKHelper: `Tools.ModLoaded("Exhaustless", ...)` → `Harmony.HasAnyPatches` (already had fallback, just removed dead call)
- Removed `[HarmonyAfter("p1xel8ted.gyk.gykhelper")]` from MainMenuGUI_Open
- ConfigurationManagerAttributes → `<Compile Include>` link
- Removed `ref` from 5 params, changed `[Harmony]` → `[HarmonyPatch]`
- Removed commented GYKHelper references (CrossModFields.Culture, Tools.ShowMessage in Helpers.cs and Patches.cs)
- **Bug fix**: `GetCraftInfo` same-item multi-quality recipes (e.g., wine barrel needing 3x berries):
  - **Count bug**: Divided total available by per-slot need instead of total need across all slots. Example: 6 berries / 1 per slot = 6, but correct is 6 / 3 total needed = 2.
  - **Quality bug**: `AutoSelectHighestQualRecipe` set ALL slots to same highest quality regardless of count. Example: 3 slots all set to silver, but only 2 silver exist → craft fails.
  - Fix: detect all-same-item case, compute `totalNeeded = needs.Sum(n => n.value)`, per-quality maxCrafts = `count / totalNeeded`, pick highest quality with maxCrafts >= 1, set all slots to that quality. Bypass stale `multiMin` for same-item case.
- **Bug fix**: `CanCraftMultiple` postfix set `__result = false` when `ForceMultiCraft` config was OFF, disabling multi-craft for ALL recipes including ones the game normally allows. Fixed to early-return (preserve game behavior) when feature disabled or craft is unsafe.
- **Simplified**: `GetSpendTxt` postfix (~180 → ~65 lines). Removed dead code (`int % 1 == 0` always true, `.ToString("0.0")` unreachable), decompiler-style flag variables, and duplicate Exhaustless branches. Preserved all functionality: multiplier-scaled costs, Exhaustless halving, FasterCraft time adjustment, hours display.
- **Full functionality verification**: All 13 patches verified against game code. MakeCraftAuto energy→time conversion is intentional (not a bug). AdjustCraftOutput integer division works correctly. CraftReally_Prefix omitting __instance is valid HarmonyX. AlreadyRun flag is intentional once-per-open behavior.

### #36 SaveNow (2.5.5 → 2.5.6)
- Removed GYKHelper (19 references): `Actions.GameStartedPlaying` → postfix on `GameSave.GlobalEventsCheck`, `Actions.EndOfDayPostfix` → postfix on `EnvironmentEngine.OnEndOfDay` with config check, `CrossModFields.Culture` (4x) → local `GameCulture` property, `CrossModFields.IsInDungeon` (4x) → local field + `WorldGameObject.Interact` prefix, `CrossModFields.ModGerryTag` (2x) → local const, `Tools.TutorialDone` (6x) → local helper, `Tools.ShowMessage` (2x) → inline Eastern/non-Eastern, `Tools.SpawnGerry` (2x) → local helper with full cinematic logic
- ConfigurationManagerAttributes → `<Compile Include>` link
- Removed `[BepInDependency]`, removed unused Newtonsoft.Json reference
- Removed dead DialogGUI ReversePatch (DialogGUI inherits BaseGUI not BaseMenuGUI — method never existed on it)
- Removed all commented-out code (dead patches, dead fields, dead methods)
- Fixed `[Harmony]` → `[HarmonyPatch]`, removed `ref` from 2 reference-type params
- **Bug fix**: `GetLocalizedString(strings.XXX)` was a no-op — culture set AFTER string argument already evaluated. Removed method, set culture directly before accessing localized strings.
- **Bug fix**: `SortSaveGames` used `SortedDictionary<DateTime, SaveSlotData>` — `Add()` throws on duplicate timestamps. Replaced with `List<(DateTime, SaveSlotData)>` + LINQ sort.
- **Bug fix**: `InGameMenuGUI_Open` used fragile `GameObject.Find` + `_widgets[5]` to find exit button. Replaced with `__instance.label_save_and_exit.text` (game has the field directly on InGameMenuGUI).
- Simplified `SaveLocation` dictionary update: `TryGetValue` + `Remove` + `Add` → indexer upsert
- Simplified `SaveOnNewDay` config: removed dynamic subscribe/unsubscribe SettingChanged handler, postfix checks config value directly
- Fixed duplicate config Order (NewFileOnManualSave and NewFileOnNewDaySave both had Order=15) → renumbered sequentially
- **Bug fix**: Autosave timer permanently dies when guards fail — `AutoSaveIE` exits via `yield break` without calling `StartTimer()`, and GJTimer is one-shot (self-destructs after firing). Moved `StartTimer()` to `AutoSave()` callback so timer always restarts regardless of coroutine outcome.
- **Bug fix**: `StopCoroutine(AutoSaveIE())` creates a new IEnumerator that doesn't match the running one — silently does nothing. Fixed to use stored `AutoSaveCoroutine` reference.
- **Bug fix**: `StartTimer()` added new timers without killing existing ones. Fixed to call `KillTimers()` first, preventing accumulation.
- Added `Timers.Clear()` after stopping all timers in `KillTimers()`

## Next Up
#37 GerrysJunkTrunk (8 files, 1296 lines, GYKHelper: Yes)
