namespace QueueEverything;

[Harmony]
public static class Patches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameSave), nameof(GameSave.GlobalEventsCheck))]
    public static void GameSave_GlobalEventsCheck_DebugWarning()
    {
        Plugin.ShowDebugWarningOnce();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;

        if (CraftComponentPatches.PendingReapply && Plugin.CcAlreadyRun)
        {
            CraftComponentPatches.PendingReapply = false;
            try
            {
                CraftComponentPatches.ApplyCraftMutations();
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"[MainGame.Update] deferred ApplyCraftMutations failed: {ex}");
            }

            if (!Plugin.AnyAutoCraftCategoryEnabled())
            {
                if (Plugin.DebugEnabled && Plugin.CurrentlyCrafting.Count > 0)
                {
                    Plugin.WriteLog($"[MainGame.Update] auto-craft fully disabled; clearing {Plugin.CurrentlyCrafting.Count} in-flight craftery tracker entries.");
                }
                Plugin.CurrentlyCrafting.Clear();
            }
        }

        if (!Plugin.AnyAutoCraftCategoryEnabled()) return;
        if (Plugin.CraftsStarted) return;

        foreach (var wgo in MainGame.me.world.GetComponentsInChildren<WorldGameObject>(true))
        {
            if (wgo != null && wgo.components.craft.is_crafting && !wgo.has_linked_worker && wgo.linked_worker == null)
            {
                Plugin.CurrentlyCrafting.Add(wgo);
            }
        }

        if (Plugin.DebugEnabled) Plugin.WriteLog($"[MainGame.Update] initial scan found {Plugin.CurrentlyCrafting.Count} in-flight unmanned crafteries.");

        Plugin.CraftsStarted = true;
    }

    [HarmonyAfter("p1xel8ted.gyk.fastercraftreloaded")]
    [HarmonyPostfix, HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open()
    {
        Plugin.FasterCraftEnabled = false;
        Plugin.FasterCraftReloaded = false;
        Plugin.ExhaustlessEnabled = false;

        Plugin.FasterCraftReloaded = Harmony.HasAnyPatches("p1xel8ted.gyk.fastercraftreloaded");
        Plugin.ExhaustlessEnabled = Harmony.HasAnyPatches("p1xel8ted.gyk.exhaust-less");

        if (Plugin.FasterCraftReloaded)
        {
            Plugin.TimeAdjustment = Plugin.FcTimeAdjustment.Value;
            Plugin.FasterCraftEnabled = true;
            if (Plugin.DebugEnabled) Plugin.WriteLog($"FasterCraft Reloaded! detected, using its config.");
        }

        if (Plugin.ExhaustlessEnabled)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"Exhaust-less! detected, using its config.");
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.Redraw))]
    public static void CraftItemGUI_Redraw(CraftItemGUI __instance)
    {
        var canCraftMultiple = __instance.craft_definition.CanCraftMultiple();
        if (__instance.craft_definition is ObjectCraftDefinition)
        {
            Plugin.Log.LogInfo($"[QE-Diag] Redraw_Prefix on ObjectCraftDefinition '{__instance.craft_definition.id}' before logic; _amount={__instance._amount}, CanCraftMultiple={canCraftMultiple}, end_script='{__instance.craft_definition.end_script}', change_wgo='{__instance.craft_definition.change_wgo}', enqueue_type={__instance.craft_definition.enqueue_type}");
        }
        if (!canCraftMultiple)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Redraw] {__instance.craft_definition.id}: skip (CanCraftMultiple=false) → _amount=1");
            __instance._amount = 1;
            return;
        }

        if (Plugin.AlreadyRun)
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Redraw] {__instance.craft_definition.id}: skip (AlreadyRun)");
            return;
        }

        if (__instance.craft_definition.id.Contains("fire") || __instance.craft_definition.id.Contains("fuel"))
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Redraw] {__instance.craft_definition.id}: skip (fire/fuel craft) → _amount=1");
            __instance._amount = 1;
            return;
        }

        var multiInventory = !GlobalCraftControlGUI.is_global_control_active
            ? MainGame.me.player.GetMultiInventoryForInteraction()
            : GUIElements.me.craft.multi_inventory;

        var craftInfo = GetCraftInfo(__instance, multiInventory);

        if (Plugin.AutoMaxMultiQualCrafts.Value && craftInfo.IsMultiQualCraft)
        {
            __instance._amount = craftInfo.Min;
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Redraw] {__instance.craft_definition.id}: auto-max multi-qual → _amount={craftInfo.Min}");
        }

        if (Plugin.AutoMaxNormalCrafts.Value && !craftInfo.IsMultiQualCraft && craftInfo.NotCraftable.Count == 0)
        {
            __instance._amount = craftInfo.Min;
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[Redraw] {__instance.craft_definition.id}: auto-max normal → _amount={craftInfo.Min}");
        }
    }

    private static CraftInfo GetCraftInfo(CraftItemGUI __instance, MultiInventory multiInventory)
    {
        List<int> craftable = [];
        List<int> notCraftable = [];
        var isMultiQualCraft = false;
        var bCraftable = 0;
        var sCraftable = 0;
        var gCraftable = 0;

        var firstNeedId = __instance.current_craft.needs.Count > 0 ? __instance.current_craft.needs[0].id : null;
        var allSameItem = __instance.current_craft.needs.Count > 1 &&
                          __instance.current_craft.needs.All(n => n.id == firstNeedId);
        var sameItemHandled = false;

        for (var i = 0; i < __instance._multiquality_ids.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(__instance._multiquality_ids[i]))
            {
                var itemCount = multiInventory.GetTotalCount(__instance.current_craft.needs[i].id);
                var itemNeed = __instance.current_craft.needs[i].value;
                var itemCraftable = itemCount / itemNeed;
                if (itemCraftable != 0)
                    craftable.Add(itemCraftable);
                else
                    notCraftable.Add(itemCraftable);
            }
            else
            {
                isMultiQualCraft = true;
                var itemID = __instance.current_craft.needs[i].id;
                var bStarItem = multiInventory.GetTotalCount(itemID + ":1");
                var sStarItem = multiInventory.GetTotalCount(itemID + ":2");
                var gStarItem = multiInventory.GetTotalCount(itemID + ":3");

                if (allSameItem)
                {
                    if (!sameItemHandled)
                    {
                        sameItemHandled = true;
                        var totalNeeded = __instance.current_craft.needs.Sum(n => n.value);

                        var gMaxCrafts = gStarItem / totalNeeded;
                        var sMaxCrafts = sStarItem / totalNeeded;
                        var bMaxCrafts = bStarItem / totalNeeded;
                        var totalAvailable = bStarItem + sStarItem + gStarItem;
                        var mixedMaxCrafts = totalAvailable / totalNeeded;

                        int maxCrafts;

                        if (Plugin.AutoSelectHighestQualRecipe.Value)
                        {
                            if (gMaxCrafts > 0) { maxCrafts = gMaxCrafts; }
                            else if (sMaxCrafts > 0) { maxCrafts = sMaxCrafts; }
                            else if (bMaxCrafts > 0) { maxCrafts = bMaxCrafts; }
                            else { maxCrafts = mixedMaxCrafts; }
                        }
                        else
                        {
                            maxCrafts = mixedMaxCrafts;
                        }

                        if (maxCrafts > 0)
                        {
                            craftable.Add(maxCrafts);
                        }
                        else
                        {
                            notCraftable.Add(0);
                        }
                    }
                }
                else
                {
                    var itemValueNeeded = __instance.current_craft.needs[i].value;
                    bCraftable = bStarItem / itemValueNeeded;
                    sCraftable = sStarItem / itemValueNeeded;
                    gCraftable = gStarItem / itemValueNeeded;

                    if (bCraftable != 0) craftable.Add(bCraftable);
                    if (sCraftable != 0) craftable.Add(sCraftable);
                    if (gCraftable != 0) craftable.Add(gCraftable);

                    if (bCraftable + sCraftable + gCraftable > 0)
                    {
                        if (Plugin.AutoSelectHighestQualRecipe.Value)
                        {
                            if (gCraftable > 0)
                                __instance._multiquality_ids[i] = itemID + ":3";
                            else if (sCraftable > 0)
                                __instance._multiquality_ids[i] = itemID + ":2";
                            else
                                __instance._multiquality_ids[i] = itemID + ":1";
                        }
                    }
                }
            }
        }

        var m1 = craftable.Count > 0 ? craftable.Min() : 0;
        var multiMin = sameItemHandled ? 0 : Mathf.Max(bCraftable, sCraftable, gCraftable);
        var min = multiMin <= 0 ? m1 : Math.Min(m1, multiMin);

        return new CraftInfo { Min = min, IsMultiQualCraft = isMultiQualCraft, NotCraftable = notCraftable };
    }

    private class CraftInfo
    {
        public int Min;
        public bool IsMultiQualCraft;
        public List<int> NotCraftable;
    }
}


[HarmonyPatch(typeof(CraftComponent))]
public static class CraftComponentPatches
{
    private sealed class CraftSnapshot
    {
        public bool IsAuto;
        public CraftDefinition.EnqueueType EnqueueType;
        public SmartExpression Energy;
        public SmartExpression CraftTime;
        public bool ForceMultiCraft;
        public bool DisableMultiCraft;
        public Dictionary<int, int> FireNeedValues;
        public Dictionary<int, int> ResearchOutputValues;

        public bool IsUnsafe;
        public CraftCategory Category;
        public SmartExpression CachedAutoCraftTime;
    }

    private static readonly Dictionary<string, CraftSnapshot> Snapshots = new(StringComparer.Ordinal);
    private static readonly HashSet<string> WarnedUncategorized = new(StringComparer.Ordinal);
    private static readonly SmartExpression ZeroEnergyExpr = SmartExpression.ParseExpression("0");

    internal static bool PendingReapply;

    [HarmonyPostfix, HarmonyPatch(nameof(CraftComponent.FillCraftsList))]
    public static void FillCraftsList_Postfix()
    {
        if (Plugin.CcAlreadyRun) return;
        Plugin.CcAlreadyRun = true;

        var enabledCategories = string.Join(",", Plugin.CategoryToggles.Where(kv => kv.Value.Value).Select(kv => kv.Key.ToString()));
        if (string.IsNullOrEmpty(enabledCategories)) enabledCategories = "(none)";
        Plugin.Log.LogInfo($"[QE-Diag] First FillCraftsList. EnableAutoCraft={Plugin.EnableAutoCraft.Value}, ForceMultiCraft={Plugin.ForceMultiCraft.Value}, HalfFireRequirements={Plugin.HalfFireRequirements.Value}, HalfCraftOutputs={Plugin.HalfCraftOutputs.Value}, AutoMaxNormalCrafts={Plugin.AutoMaxNormalCrafts.Value}, AutoMaxMultiQualCrafts={Plugin.AutoMaxMultiQualCrafts.Value}, categoriesEnabled=[{enabledCategories}]");

        CaptureSnapshots();
        ApplyCraftMutations();
    }

    private static void CaptureSnapshots()
    {
        Snapshots.Clear();
        foreach (var craft in GameBalance.me.craft_data)
        {
            if (craft == null || string.IsNullOrEmpty(craft.id))
            {
                continue;
            }

            var snap = new CraftSnapshot
            {
                IsAuto = craft.is_auto,
                EnqueueType = craft.enqueue_type,
                Energy = craft.energy,
                CraftTime = craft.craft_time,
                ForceMultiCraft = craft.force_multi_craft,
                DisableMultiCraft = craft.disable_multi_craft,
                FireNeedValues = null,
                ResearchOutputValues = null,
                IsUnsafe = Plugin.IsUnsafeDefinition(craft),
                Category = CraftCategories.Classify(craft.craft_in),
                CachedAutoCraftTime = null,
            };

            for (var i = 0; i < craft.needs_from_wgo.Count; i++)
            {
                if (craft.needs_from_wgo[i].id != "fire") continue;
                snap.FireNeedValues ??= new Dictionary<int, int>();
                snap.FireNeedValues[i] = craft.needs_from_wgo[i].value;
            }

            for (var i = 0; i < craft.output.Count; i++)
            {
                var id = craft.output[i].id;
                if (id is not ("r" or "g" or "b")) continue;
                snap.ResearchOutputValues ??= new Dictionary<int, int>();
                snap.ResearchOutputValues[i] = craft.output[i].value;
            }

            Snapshots[craft.id] = snap;
        }

        if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[Snapshots] captured {Snapshots.Count} craft definitions");
        }
    }

    internal static void ApplyCraftMutations()
    {
        if (!Plugin.CcAlreadyRun) return;

        var startTicks = Stopwatch.GetTimestamp();

        var converted = 0;
        var skippedAutoAlready = 0;
        var skippedUnsafe = 0;
        var skippedCategoryOff = 0;
        var halved = 0;
        var fireAdjusted = 0;
        var forcedMulti = 0;

        foreach (var craft in GameBalance.me.craft_data)
        {
            if (craft == null || !Snapshots.TryGetValue(craft.id, out var snap))
            {
                continue;
            }

            RestoreFromSnapshot(craft, snap);

            if (TryAdjustFireRequirements(craft))
            {
                fireAdjusted++;
            }

            if (TryApplyForcedMultiCraft(craft, snap))
            {
                forcedMulti++;
            }

            var result = TryMakeCraftAuto(craft, snap);
            switch (result)
            {
                case AutoResult.Converted: converted++; break;
                case AutoResult.AlreadyAuto: skippedAutoAlready++; break;
                case AutoResult.Unsafe: skippedUnsafe++; break;
                case AutoResult.CategoryDisabled: skippedCategoryOff++; break;
            }

            if (result == AutoResult.Converted && TryAdjustCraftOutput(craft))
            {
                halved++;
            }
        }

        var elapsedMs = (Stopwatch.GetTimestamp() - startTicks) * 1000.0 / Stopwatch.Frequency;
        Plugin.Log.LogInfo(
            $"[ApplyCraftMutations] elapsed={elapsedMs:F2}ms converted={converted} halved={halved} fireAdjusted={fireAdjusted} forcedMulti={forcedMulti} skipped(alreadyAuto={skippedAutoAlready}, unsafe={skippedUnsafe}, categoryOff={skippedCategoryOff})");
    }

    private enum AutoResult { Converted, AlreadyAuto, Unsafe, CategoryDisabled }

    private static void RestoreFromSnapshot(CraftDefinition craft, CraftSnapshot snap)
    {
        craft.is_auto = snap.IsAuto;
        craft.enqueue_type = snap.EnqueueType;
        craft.energy = snap.Energy;
        craft.craft_time = snap.CraftTime;
        craft.force_multi_craft = snap.ForceMultiCraft;
        craft.disable_multi_craft = snap.DisableMultiCraft;

        if (snap.FireNeedValues != null)
        {
            foreach (var kv in snap.FireNeedValues)
            {
                if (kv.Key < craft.needs_from_wgo.Count)
                {
                    craft.needs_from_wgo[kv.Key].value = kv.Value;
                }
            }
        }

        if (snap.ResearchOutputValues != null)
        {
            foreach (var kv in snap.ResearchOutputValues)
            {
                if (kv.Key < craft.output.Count)
                {
                    craft.output[kv.Key].value = kv.Value;
                }
            }
        }
    }

    private static bool TryAdjustFireRequirements(CraftDefinition craft)
    {
        if (!Plugin.HalfFireRequirements.Value) return false;

        var touched = false;
        foreach (var item in craft.needs_from_wgo.Where(item => item.id == "fire"))
        {
            item.value = Mathf.CeilToInt(item.value / 2f);
            touched = true;
        }

        return touched;
    }

    private static bool TryApplyForcedMultiCraft(CraftDefinition craft, CraftSnapshot snap)
    {
        if (!Plugin.ForceMultiCraft.Value || snap.IsAuto || snap.IsUnsafe) return false;

        craft.force_multi_craft = true;
        craft.disable_multi_craft = false;
        return true;
    }

    private static AutoResult TryMakeCraftAuto(CraftDefinition craft, CraftSnapshot snap)
    {
        if (snap.IsAuto) return AutoResult.AlreadyAuto;
        if (snap.IsUnsafe) return AutoResult.Unsafe;

        WarnIfUncategorized(craft, snap);

        if (!Plugin.IsCategoryEnabled(snap.Category))
        {
            if (Plugin.DebugEnabled)
            {
                Plugin.WriteLog($"[MakeCraftAuto] skip '{craft.id}' (category={snap.Category} disabled)");
            }
            return AutoResult.CategoryDisabled;
        }

        if (snap.CachedAutoCraftTime == null)
        {
            var craftEnergyTime = snap.Energy.EvaluateFloat(MainGame.me.player);
            craftEnergyTime *= 1.50f;
            craftEnergyTime = Mathf.CeilToInt(craftEnergyTime);
            snap.CachedAutoCraftTime = SmartExpression.ParseExpression(craftEnergyTime.ToString(CultureInfo.InvariantCulture));
        }

        craft.craft_time = snap.CachedAutoCraftTime;
        craft.energy = ZeroEnergyExpr;
        craft.is_auto = true;
        craft.enqueue_type = CraftDefinition.EnqueueType.CanEnqueue;

        if (Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[MakeCraftAuto] convert '{craft.id}' (category={snap.Category})");
        }

        return AutoResult.Converted;
    }

    private static void WarnIfUncategorized(CraftDefinition craft, CraftSnapshot snap)
    {
        if (snap.Category != CraftCategory.Misc) return;
        if (craft.craft_in == null || craft.craft_in.Count == 0) return;

        foreach (var craftIn in craft.craft_in)
        {
            if (string.IsNullOrEmpty(craftIn) || !WarnedUncategorized.Add(craftIn))
            {
                continue;
            }

            Plugin.Log.LogWarning($"[QueueEverything] Uncategorized hand craftery: '{craftIn}' (sample craft: '{craft.id}'). Falling back to 'Misc'. If this should be in a named category, tell p1xel8ted.");
        }
    }

    private static bool TryAdjustCraftOutput(CraftDefinition craft)
    {
        if (!Plugin.HalfCraftOutputs.Value) return false;

        var touched = false;
        foreach (var output in craft.output)
        {
            if (output.id is not ("r" or "g" or "b")) continue;
            output.value /= 2;
            output.value = output.value < 1 ? 1 : Mathf.CeilToInt(output.value);
            touched = true;
        }

        if (touched && Plugin.DebugEnabled)
        {
            Plugin.WriteLog($"[AdjustCraftOutput] halved r/g/b outputs on '{craft.id}'");
        }

        return touched;
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CraftComponent.CraftReally))]
    public static void CraftReally_Prefix()
    {
        if (!MainGame.game_started || !Plugin.AnyAutoCraftCategoryEnabled()) return;

        var beforeCount = Plugin.CurrentlyCrafting.Count;
        Plugin.CurrentlyCrafting.RemoveAll(wgo => wgo == null || !wgo.components.craft.is_crafting || wgo.has_linked_worker || wgo.linked_worker != null);

        if (Plugin.DebugEnabled && Plugin.CurrentlyCrafting.Count > 0)
        {
            Plugin.WriteLog($"[CraftReally] pumping {Plugin.CurrentlyCrafting.Count} in-flight crafteries (pruned {beforeCount - Plugin.CurrentlyCrafting.Count}).");
        }

        foreach (var wgo in Plugin.CurrentlyCrafting)
        {
            wgo.OnWorkAction();
        }
    }
}


[HarmonyPatch(typeof(CraftDefinition))]
public static class CraftDefinitionPatches
{
    [HarmonyPostfix, HarmonyPatch(nameof(CraftDefinition.CanCraftMultiple))]
    public static void CraftDefinition_CanCraftMultiple(CraftDefinition __instance, ref bool __result)
    {
        var origResult = __result;
        if (!Plugin.ForceMultiCraft.Value || Plugin.IsUnsafeDefinition(__instance))
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[CanCraftMultiple] {__instance.id}: unsafe/force-off → {__result} (timeZero={__instance.craft_time_is_zero})");
            return;
        }

        if (Plugin.DebugEnabled) Plugin.WriteLog($"[CanCraftMultiple] {__instance.id}: forced → true (timeZero={__instance.craft_time_is_zero})");
        __result = true;

        if (__instance is ObjectCraftDefinition)
        {
            Plugin.Log.LogInfo($"[QE-Diag] CanCraftMultiple FORCED true on ObjectCraftDefinition '{__instance.id}' (was {origResult}, end_script='{__instance.end_script}', change_wgo='{__instance.change_wgo}', enqueue_type={__instance.enqueue_type}, one_time={__instance.one_time_craft}, force_multi={__instance.force_multi_craft}, disable_multi={__instance.disable_multi_craft})");
        }
    }

    [HarmonyPostfix, HarmonyPatch(nameof(CraftDefinition.GetSpendTxt))]
    public static void CraftDefinition_GetSpendTxt(CraftDefinition __instance, WorldGameObject wgo, ref string __result,
        int multiplier = 1)
    {
        var text = "";
        int num;
        if (GlobalCraftControlGUI.is_global_control_active)
            num = __instance.gratitude_points_craft_cost is not { has_expression: true }
                ? 0
                : Mathf.CeilToInt(__instance.gratitude_points_craft_cost.EvaluateFloat(wgo));
        else
            num = __instance.energy is not { has_expression: true }
                ? 0
                : Mathf.CeilToInt(__instance.energy.EvaluateFloat(wgo));

        if (num != 0)
        {
            var toolK = 1f;
            if (wgo?.obj_def?.tool_actions != null)
            {
                foreach (var actionTool in wgo.obj_def.tool_actions.action_tools)
                {
                    if (actionTool == ItemDefinition.ItemType.Hand) continue;
                    var equippedTool = MainGame.me.player.GetEquippedTool(actionTool);
                    if (equippedTool?.definition?.tool_energy_k is { has_expression: true })
                    {
                        var k = equippedTool.definition.tool_energy_k.EvaluateFloat(wgo, MainGame.me.player);
                        if (k < toolK) toolK = k;
                    }
                }
            }

            if (!toolK.EqualsTo(1f, 0.01f)) num = Mathf.CeilToInt(num * toolK);

            if (GlobalCraftControlGUI.is_global_control_active)
            {
                var cost = Plugin.ExhaustlessEnabled ? Mathf.CeilToInt(num * multiplier / 2f) : num * multiplier;
                var hasEnough = MainGame.me.player.gratitude_points >= (__instance.gratitude_points_craft_cost?.EvaluateFloat(MainGame.me.player) ?? 0f);
                text += hasEnough
                    ? $"[c](gratitude_points)[/c]{cost}"
                    : $"(gratitude_points)[c][ff1111]{cost}[/c]";
            }
            else
            {
                var cost = Plugin.ExhaustlessEnabled ? Mathf.CeilToInt(num / 2f) * multiplier : Mathf.CeilToInt(num * multiplier);
                text += $"[c](en)[/c]{cost}";
            }
        }

        if (__instance.is_auto)
        {
            var craftTime = __instance.craft_time.EvaluateFloat(wgo);
            if (Plugin.DebugEnabled) Plugin.WriteLog($"[CraftText]: Craft: {__instance.id}, BaseTime: {craftTime}");

            if (craftTime != 0)
            {
                if (Plugin.FasterCraftEnabled)
                    craftTime = Plugin.TimeAdjustment < 0 ? craftTime * Plugin.TimeAdjustment : craftTime / Plugin.TimeAdjustment;

                craftTime = Mathf.CeilToInt(craftTime);
                if (craftTime > 0) craftTime *= multiplier;

                var timeSpan = TimeSpan.FromSeconds(craftTime);
                text = text.ConcatWithSeparator(timeSpan.Hours >= 1
                    ? $"[c](time)[/c]{timeSpan.Hours:0}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}"
                    : $"[c](time)[/c]{timeSpan.Minutes:0}:{timeSpan.Seconds:00}");
            }
        }

        foreach (var item in __instance.needs_from_wgo.Where(item => item.id == "fire"))
        {
            var text2 = $"[c](fire2)[/c]{item.value * multiplier:0}";
            if (!wgo!.data.IsEnoughItems(item, "", 0, multiplier)) text2 = $"[ff1111]{text2}[/c]";
            text = text.ConcatWithSeparator(text2);
        }

        if (wgo?.obj_def?.tool_actions != null && !__instance.is_auto)
        {
            for (var j = 0; j < wgo.obj_def.tool_actions.action_tools.Count; j++)
            {
                var itemType = wgo.obj_def.tool_actions.action_tools[j];
                if (itemType == ItemDefinition.ItemType.Hand) continue;
                var toolName = itemType.ToString().ToLower();
                var efficiency = Mathf.FloorToInt(100f * wgo.obj_def.tool_actions.action_k[j]);
                var equippedTool = MainGame.me.player.GetEquippedTool(itemType);
                if (equippedTool == null)
                {
                    text += $"\n[c][ff1111]({toolName}_s)[-][/c]";
                }
                else
                {
                    efficiency = Mathf.FloorToInt(efficiency * equippedTool.definition.efficiency);
                    text += $"\n[c]({toolName}_s)[/c]\n{efficiency}%";
                }
            }
        }

        __result = text;
    }
}


[HarmonyPatch(typeof(CraftGUI))]
public static class CraftGUIPatches
{
    [HarmonyPostfix, HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.ExpandItem))]
    public static void ExpandItem_Postfix(CraftGUI __instance, CraftItemGUI craft_item_gui)
    {
        if (Plugin.IsUnsafeDefinition(craft_item_gui.craft_definition)) return;
        if (Plugin.AutoSelectCraftButtonWithController.Value && LazyInput.gamepad_active)
        {
            foreach (var uiButton in craft_item_gui.GetComponentsInChildren<UIButton>().Where(button => button.name.Contains("craft")))
            {
                __instance.gamepad_controller.SetFocusedItem(uiButton.GetComponent<GamepadNavigationItem>());
                uiButton.gameObject.SetActive(true);
            }
        }
    }

    [HarmonyPostfix, HarmonyPatch(nameof(CraftGUI.Open))]
    public static void Open_Postfix()
    {
        Plugin.AlreadyRun = true;
        var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
        if (crafteryWgo?.obj_def?.interaction_type == ObjectDefinition.InteractionType.Builder)
        {
            Plugin.Log.LogInfo($"[QE-Diag] CraftGUI.Open on BUILD DESK '{crafteryWgo.obj_id}' (interaction_type=Builder)");
        }
        if (Plugin.DebugEnabled) Plugin.WriteLog($"Keeper interacted with: {crafteryWgo.obj_id}.");
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CraftGUI.Open))]
    public static void Open_Prefix()
    {
        Plugin.AlreadyRun = false;
    }

    [HarmonyPostfix, HarmonyPatch(nameof(CraftGUI.SwitchTab))]
    public static void SwitchTab_Postfix()
    {
        Plugin.AlreadyRun = true;
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CraftGUI.SwitchTab))]
    public static void SwitchTab_Prefix()
    {
        Plugin.AlreadyRun = false;
    }
}


[HarmonyPatch(typeof(CraftItemGUI))]
public static class CraftItemGUIPatches
{
    [HarmonyPostfix, HarmonyPatch(nameof(CraftItemGUI.OnCraftPressed))]
    public static void OnCraftPressed_Postfix(WorldGameObject __state)
    {
        if (__state != null && __state.obj_def?.interaction_type == ObjectDefinition.InteractionType.Builder)
        {
            Plugin.Log.LogInfo($"[QE-Diag] OnCraftPressed_Postfix on BUILD DESK '{__state.obj_id}' (anyCategoryEnabled={Plugin.AnyAutoCraftCategoryEnabled()}, linked_worker={__state.linked_worker != null}, has_linked_worker={__state.has_linked_worker})");
        }

        if (!Plugin.AnyAutoCraftCategoryEnabled() || __state == null || __state.linked_worker != null || __state.has_linked_worker) return;

        Plugin.CurrentlyCrafting.Add(__state);
        if (Plugin.DebugEnabled) Plugin.WriteLog($"[OnCraftPressed] tracking new auto-craft on '{__state.obj_id}' (total tracked: {Plugin.CurrentlyCrafting.Count})");
        __state.OnWorkAction();
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CraftItemGUI.OnCraftPressed))]
    public static void OnCraftPressed_Prefix(CraftItemGUI __instance, ref WorldGameObject __state)
    {
        try
        {
            if (Plugin.DebugEnabled) Plugin.WriteLog($"Craft: {__instance.craft_definition.id}, One time: {__instance.craft_definition.one_time_craft}");
            if (Plugin.IsUnsafeDefinition(__instance.craft_definition)) return;

            var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
            __state = crafteryWgo;

            if (crafteryWgo?.obj_def?.interaction_type == ObjectDefinition.InteractionType.Builder)
            {
                Plugin.Log.LogInfo($"[QE-Diag] OnCraftPressed_Prefix on BUILD DESK '{crafteryWgo.obj_id}' for craft '{__instance.craft_definition.id}' (type={__instance.craft_definition.GetType().Name}, _amount={__instance._amount}, IsBuildMode={GUIElements.me.craft.IsBuildMode()})");
            }

            var time = __instance.craft_definition.craft_time.EvaluateFloat(crafteryWgo);
            ApplyFasterCraft(ref time);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"[QE-Diag] OnCraftPressed_Prefix threw for craft '{__instance?.craft_definition?.id}': {ex}");
        }
    }

    private static void ApplyFasterCraft(ref float time)
    {
        if (!Plugin.FasterCraftEnabled) return;

        if (Plugin.TimeAdjustment < 0)
            time /= Plugin.TimeAdjustment;
        else
            time *= Plugin.TimeAdjustment;
    }
}
