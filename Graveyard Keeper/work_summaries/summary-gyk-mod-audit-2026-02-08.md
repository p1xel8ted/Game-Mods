# GYK Mod Audit — Work Summary 2026-02-08

## Progress
39 of 39 mods complete. **AUDIT COMPLETE.**

## Session Work

### #37 GerrysJunkTrunk (1.9.0 → 1.9.1) — COMPLETE
- Removed GYKHelper (10 references): `Actions.WorldGameObjectInteractPrefix` → `[HarmonyPrefix]` on `WorldGameObject.Interact`, `Actions.GameBalanceLoad` → `[HarmonyPostfix]` on `GameBalance.LoadGameBalance`, `Actions.EndOfDayPostfix` → `[HarmonyPostfix]` on `EnvironmentEngine.OnEndOfDay`, `Actions.GameStartedPlaying` → `[HarmonyPostfix]` on `GameSave.GlobalEventsCheck`, `CrossModFields.Culture` (6x) → local `GameCulture` property, `Tools.ShowCinematic`/`HideCinematic` → local methods with `_cinematicPlaying` guard, `Tools.NameSpawnedGerry` → inline tag assignment
- ConfigurationManagerAttributes → `<Compile Include>` link
- Removed `[BepInDependency]`, removed GYKHelper ProjectReference
- Deleted `TargetPatches.cs` (entirely commented-out dead code)
- Removed all commented-out code (GetLocalizedString, ShowSummary, Vendor_CanTradeItem)
- Changed `[Harmony]` → `[HarmonyPatch]`, removed `ref` from 14 reference-type params
- **Bug fix**: `UnlockedFullPrice()` contained `MainGame.me.save.unlocked_techs.Add("my perk")` — debug line that added a bogus tech entry to the save on every call. Called many times per frame when trunk UI was open, accumulating duplicates in the save file's unlocked_techs list.
- **Bug fix**: `EnvironmentEngine_OnEndOfDay` called `Stats.PlayerAddMoney(earnings, strings.Header)` — `Stats.PlayerAddMoney` is a telemetry stub that does nothing (returns immediately if amount > 0, only logs warning if amount <= 0). Removed dead call; the actual money addition is `MainGame.me.player.data.money += earnings`.
- **Bug fix**: `StartGerryRoutine` had a redundant `_shippingBox.data.inventory.Clear()` in gerry2's callback — inventory was already cleared by `EnvironmentEngine_OnEndOfDay` before the routine started.
- **Verified NOT bugs**: `GetPrice(totalCount) * totalCount` is correct (GetPrice returns per-item price, confirmed against `Vendor.GetSingleItemPrice`); `ExcludeItems` logic correctly allows quest-marked tools/armor to be sold; `StackSizeBackups` never cleared is by design (TryAdd prevents overwriting with inflated values)

### #38 WheresMaStorage (2.1.8 → 2.1.9) — COMPLETE
#### GYKHelper Removal (16 references):
- `Actions.GameStartedPlaying` → `[HarmonyPostfix]` on `GameSave.GlobalEventsCheck`
- `Actions.GameBalanceLoad` → `[HarmonyPostfix]` on `GameBalance.LoadGameBalance`
- `CrossModFields.Is*` (9 flags) + `CurrentWgoInteraction` → local `Fields.*` properties + own `[HarmonyPrefix]` on `WorldGameObject.Interact` + `[HarmonyPostfix]` on `BaseGUI.Hide`
- `CrossModFields.Culture` (2x) → local `GameCulture` property
- `CrossModFields.WorldObjects` → `WorldMap._objs`
- `Tools.ShowAlertDialog` → `GUIElements.me.dialog.OpenOK`
- `Tools.GetWorldDrops` → `GetComponentsInChildren<DropResGameObject>(true)`
- `Tools.CanCollectDrop` → inline `definition is {item_size: > 1}` check
- `Extensions.TryAdd` → local `TryAdd<TKey,TValue>` extension in Helpers.cs
- Removed `[BepInDependency]`, replaced GYKHelper ProjectReference with ConfigurationManagerAttributes `<Compile Include>` link

#### Bug Fixes:
- **BUG 1**: `LoadInventories` zone skip had inverted Contains: `a.Contains(zone.id)` → `zone.id.Contains(a)`. "refugees_camp" was never skipped because `"refugees".Contains("refugees_camp")` = false.
- **BUG 2**: Custom toolbelt in `LoadInventories` had `id = null`, couldn't match tools filter. Added `id = "Toolbelt"`. Also added `!multi_inventory.all.Contains(tools[0])` guard in DoOpening prefix to prevent duplicate insertion.
- **BUG 3**: `SetInventorySizeText` called `MakeInventoryCopy()` (deep-copies entire inventory) just to read size and count. Replaced with direct property access: `inventory_data.inventory_size` and `inventory_data.inventory.Count`.

#### Dead Code Removed:
- `Fields.RefugeeMi` (created but never read)
- `Fields.TimeSix/TimeEight/TimeNine` (never used)
- `Fields.OldDrops` + `WorldMap_RescanDropItemsList` postfix (set but never read)
- `Fields.LogGap` (never used)
- Commented-out `ToolItems` array
- Commented-out `Tools.TutorialDone()` lines (3 in Transpilers.cs)
- `System.Threading.Tasks` global using

#### Cleanup:
- Replaced duplicate local `AlwaysSkip` arrays → `Fields.AlwaysSkipZones` + `Fields.AlwaysSkipInventories`
- Removed duplicate `AlwaysSkip` check in `BaseCraftGUI_multi_inventory` (identical check on lines 181 and 190)
- Removed `ref` from reference-type params (OrganEnhancerGUI_Open, WorldGameObject_ReplaceWithObject, InventoryPanelGUI_DoOpening_Postfix, InventoryPanelGUI_Redraw, InventoryWidget_FilterItems, BaseCraftGUI_multi_inventory, CraftDefinition_takes_item_durability)
- Merged identical Inactive/Hide switch cases in InventoryWidget_FilterItems
- Removed unnecessary `return;` before local function in SetInventorySizeText
- Changed CollectDrops logging from `Plugin.Log.LogInfo` → `Helpers.Log` (respects debug flag)
- Added `System.Globalization` global using

### #39 BeamMeUpGerry (3.1.0 → 3.1.1) — COMPLETE
#### GYKHelper Removal (15 references):
- `Actions.GameStartedPlaying += Patches.UpdateZoneUpdaters` → `[HarmonyPostfix]` on `GameSave.GlobalEventsCheck`
- `CrossModFields.IsInDungeon` (2x) → local `Helpers.IsInDungeon` + `[HarmonyPrefix]` on `WorldGameObject.Interact`
- `CrossModFields.Culture` → local `GameCulture` property
- `Tools.ShowAlertDialog` → `GUIElements.me.dialog.OpenOK`
- `Tools.PlayerDisabled()` → local `PlayerDisabled()` (inline `is_dead || !IsPlayerEnable()`)
- `Tools.NameSpawnedGerry` (3x) → local `NameSpawnedGerry()` (sets tag/custom_tag to `"mod_gerry"`)
- `Tools.ShowCinematic` → local `ShowCinematic()` with `_cinematicPlaying` guard
- `Tools.HideCinematic` (3x) → local `HideCinematic()` with `_cinematicPlaying` guard
- `Tools.TutorialDone()` → local `TutorialDone()` with 18-quest check
- `Tools.PlayerHasSeenZone` → inline `known_world_zones.Exists`
- `Tools.PlayerKnowsNpcPartial` (2x) → inline `known_npcs.npcs.Exists`
- `Extensions.TryAdd<T>` (List) → local extension method
- `Extensions.TryAddComponent<T>` → local extension method
- `GYKHelper.Plugin.Log` → `Plugin.Log`
- Removed `[BepInDependency]`, replaced GYKHelper ProjectReference with ConfigurationManagerAttributes `<Compile Include>` link

#### Dead Code Removed:
- Commented-out `LocationsToReturnFalse` array (45 lines)
- Commented-out `LocationTranslationMap` dictionary (12 lines)
- Commented-out `CustomLocations` list
- Commented-out `ModEnabled` config
- Commented-out log line in Helpers.cs

#### Cleanup:
- Removed `ref` from `Item __instance` (2 patches — reference type)
- Removed unused `ref List<AnswerVisualData> answers` param from `MultiAnswerGUI_ShowAnswers`

### Follow-up: Remove Remaining GYKHelper Dependencies (4 mods)
After completing the audit, a final verification grep found 4 mods still had GYKHelper dependencies from early audits (before the removal pattern was established). All fixed:

#### AddStraightToTable (2.4.7 → 2.4.8)
- Removed stale `[BepInDependency("p1xel8ted.gyk.gykhelper")]` (no code usage existed)

#### LostToothUnlost (0.1.0 → 0.1.1)
- Replaced `Actions.GameStartedPlaying += OnGameStartedPlaying` → `[HarmonyPostfix]` on `GameSave.GlobalEventsCheck`
- Removed GYKHelper ProjectReference, removed `[BepInDependency]`
- Added `Harmony.CreateAndPatchAll` (was missing — relied on GYKHelper's Actions subscription)

#### GraveChangesRedux (0.1.2 → 0.1.3)
- Replaced `Actions.GameBalanceLoad += GameBalanceLoad` → `[HarmonyPostfix]` on `GameBalance.LoadGameBalance`
- Added local `TryAdd<TKey,TValue>` method (netstandard2.0 lacks built-in Dictionary.TryAdd)
- Removed GYKHelper ProjectReference, added CMA `<Compile Include>` link, removed `[BepInDependency]`

#### WheresMaPoints (0.3.0 → 0.3.1)
- Replaced `Tools.PlayerDisabled()` → `(MainGame.me.player.is_dead || !GS.IsPlayerEnable())` (2 call sites)
- Removed GYKHelper ProjectReference, added CMA `<Compile Include>` link, removed `[BepInDependency]`

### Final Verification
```
grep -r "ProjectReference.*GYKHelper" src/ → No matches found
grep -r "BepInDependency.*gykhelper" src/ → No matches found
```
All 39 mods are now fully standalone from GYKHelper.
