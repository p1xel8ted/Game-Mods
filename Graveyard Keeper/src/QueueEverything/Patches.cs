namespace QueueEverything;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyAfter("p1xel8ted.gyk.fastercraftreloaded")]
    [HarmonyPostfix, HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open()
    {
        FasterCraftEnabled = false;
        FasterCraftReloaded = false;
        //  _originalFasterCraft = false;
        ExhaustlessEnabled = false;

        FasterCraftReloaded = Harmony.HasAnyPatches("p1xel8ted.gyk.fastercraftreloaded");

        ExhaustlessEnabled = Harmony.HasAnyPatches("p1xel8ted.gyk.exhaust-less");
        
        if (FasterCraftReloaded)
        {
            TimeAdjustment = FcTimeAdjustment.Value;
            FasterCraftEnabled = true;
            WriteLog($"FasterCraft Reloaded! detected, using its config.");
        }

        if (ExhaustlessEnabled)
        {
            WriteLog($"Exhaust-less! detected, using its config.");
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.Redraw))]
    public static void CraftItemGUI_Redraw(CraftItemGUI __instance)
    {
        if (!__instance.craft_definition.CanCraftMultiple())
        {
            WriteLog($"[CraftItemRedraw]: Unsafe Returning: {__instance.craft_definition.id}");
            __instance._amount = 1;
            return;
        }

        if (AlreadyRun)
        {
            WriteLog($"[CraftItemRedraw]: AlreadyRun Returning: {__instance.craft_definition.id}");
            return;
        }

        if (__instance.craft_definition.id.Contains("fire") || __instance.craft_definition.id.Contains("fuel"))
        {
            WriteLog($"[CraftItemRedraw]: Unsafe Returning: {__instance.craft_definition.id}");
            __instance._amount = 1;
            return;
        }

        var multiInventory = !GlobalCraftControlGUI.is_global_control_active
            ? MainGame.me.player.GetMultiInventoryForInteraction()
            : GUIElements.me.craft.multi_inventory;

        var craftInfo = GetCraftInfo(__instance, multiInventory);

        if (AutoMaxMultiQualCrafts.Value && craftInfo.IsMultiQualCraft)
        {
            __instance._amount = craftInfo.Min;
        }

        if (AutoMaxNormalCrafts.Value && !craftInfo.IsMultiQualCraft && craftInfo.NotCraftable.Count == 0)
        {
            __instance._amount = craftInfo.Min;
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

                        int maxCrafts;
                        string qualitySuffix;

                        if (AutoSelectHighestQualRecipe.Value)
                        {
                            if (gMaxCrafts > 0) { maxCrafts = gMaxCrafts; qualitySuffix = ":3"; }
                            else if (sMaxCrafts > 0) { maxCrafts = sMaxCrafts; qualitySuffix = ":2"; }
                            else { maxCrafts = bMaxCrafts; qualitySuffix = ":1"; }
                        }
                        else
                        {
                            maxCrafts = bMaxCrafts; qualitySuffix = ":1";
                            if (sMaxCrafts > maxCrafts) { maxCrafts = sMaxCrafts; qualitySuffix = ":2"; }
                            if (gMaxCrafts > maxCrafts) { maxCrafts = gMaxCrafts; qualitySuffix = ":3"; }
                        }

                        if (maxCrafts > 0)
                            craftable.Add(maxCrafts);
                        else
                            notCraftable.Add(0);

                        for (var j = 0; j < __instance._multiquality_ids.Count; j++)
                        {
                            if (!string.IsNullOrWhiteSpace(__instance._multiquality_ids[j]))
                                __instance._multiquality_ids[j] = itemID + qualitySuffix;
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
                        if (AutoSelectHighestQualRecipe.Value)
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

        return new CraftInfo {Min = min, IsMultiQualCraft = isMultiQualCraft, NotCraftable = notCraftable};
    }

    private class CraftInfo
    {
        public int Min;
        public bool IsMultiQualCraft;
        public List<int> NotCraftable;
    }


    [HarmonyPatch(typeof(CraftComponent))]
    public static class CraftComponentPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(CraftComponent.FillCraftsList))]
        public static void FillCraftsList_Postfix()
        {
            if (CcAlreadyRun) return;
            CcAlreadyRun = true;

            foreach (var craft in GameBalance.me.craft_data)
            {
                AdjustFireRequirements(craft);
                ApplyForcedMultiCraft(craft);
                MakeCraftAuto(craft);
            }
        }

        private static void AdjustFireRequirements(CraftDefinition craft)
        {
            if (!HalfFireRequirements.Value) return;

            foreach (var item in craft.needs_from_wgo.Where(item => item.id == "fire"))
            {
                item.value = Mathf.CeilToInt(item.value / 2f);
            }
        }

        private static void ApplyForcedMultiCraft(CraftDefinition craft)
        {
            if (!ForceMultiCraft.Value || craft.is_auto || IsUnsafeDefinition(craft)) return;

            craft.force_multi_craft = true;
            craft.disable_multi_craft = false;
        }

        private static void MakeCraftAuto(CraftDefinition craft)
        {
            if (craft.is_auto || !MakeEverythingAuto.Value || IsUnsafeDefinition(craft) || ShouldExcludeCraftFromAuto(craft)) return;

            var craftEnergyTime = craft.energy.EvaluateFloat(MainGame.me.player);
            craftEnergyTime *= 1.50f;
            craftEnergyTime = Mathf.CeilToInt(craftEnergyTime);

            craft.craft_time = SmartExpression.ParseExpression(craftEnergyTime.ToString(CultureInfo.InvariantCulture));
            craft.energy = SmartExpression.ParseExpression("0");
            craft.is_auto = true;
            craft.enqueue_type = CraftDefinition.EnqueueType.CanEnqueue;

            AdjustCraftOutput(craft);
        }

        private static bool ShouldExcludeCraftFromAuto(CraftDefinition craft)
        {
            if (!MakeHandTasksAuto.Value)
            {
                return craft.craft_in.Any(craftIn =>
                    craftIn.Contains("grave") ||
                    craftIn.Contains("mf_preparation") ||
                    craftIn.Contains("cooking_table") && !craft.craft_in.Any(craftIn => craftIn.Contains("refugee")));
            }

            return false;
        }

        private static void AdjustCraftOutput(CraftDefinition craft)
        {
            craft.output.ForEach(output =>
            {
                if (output.id is not ("r" or "g" or "b")) return;
                output.value /= 2;
                output.value = output.value < 1 ? 1 : Mathf.CeilToInt(output.value);
            });
        }

        [HarmonyPrefix, HarmonyPatch(nameof(CraftComponent.CraftReally))]
        public static void CraftReally_Prefix()
        {
            if (!MainGame.game_started || !MakeEverythingAuto.Value) return;

            foreach (var wgo in CurrentlyCrafting.Where(wgo => wgo != null && wgo.components.craft.is_crafting && !wgo.has_linked_worker))
            {
                wgo.OnWorkAction();
            }
        }
    }


    [HarmonyPatch(typeof(CraftDefinition))]
    public class CraftDefinitionPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(CraftDefinition.CanCraftMultiple))]
        public static void CraftDefinition_CanCraftMultiple(CraftDefinition __instance, ref bool __result)
        {
            if (!ForceMultiCraft.Value || IsUnsafeDefinition(__instance))
            {
                WriteLog($"[Unsafe]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
                return;
            }

            WriteLog($"[Safe?]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
            __result = true;
        }

        [HarmonyPostfix, HarmonyPatch(nameof(CraftDefinition.GetSpendTxt))]
        public static void CraftDefinition_GetSpendTxt(CraftDefinition __instance, WorldGameObject wgo, ref string __result,
            int multiplier = 1)
        {
            var text = "";
            int num;
            if (GlobalCraftControlGUI.is_global_control_active)
                num = __instance.gratitude_points_craft_cost is not {has_expression: true}
                    ? 0
                    : Mathf.CeilToInt(__instance.gratitude_points_craft_cost.EvaluateFloat(wgo));
            else
                num = __instance.energy is not {has_expression: true}
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
                        if (equippedTool?.definition?.tool_energy_k is {has_expression: true})
                        {
                            var k = equippedTool.definition.tool_energy_k.EvaluateFloat(wgo, MainGame.me.player);
                            if (k < toolK) toolK = k;
                        }
                    }
                }

                if (!toolK.EqualsTo(1f, 0.01f)) num = Mathf.CeilToInt(num * toolK);

                if (GlobalCraftControlGUI.is_global_control_active)
                {
                    var cost = ExhaustlessEnabled ? Mathf.CeilToInt(num * multiplier / 2f) : num * multiplier;
                    var hasEnough = MainGame.me.player.gratitude_points >= (__instance.gratitude_points_craft_cost?.EvaluateFloat(MainGame.me.player) ?? 0f);
                    text += hasEnough
                        ? $"[c](gratitude_points)[/c]{cost}"
                        : $"(gratitude_points)[c][ff1111]{cost}[/c]";
                }
                else
                {
                    var cost = ExhaustlessEnabled ? Mathf.CeilToInt(num / 2f) * multiplier : Mathf.CeilToInt(num * multiplier);
                    text += $"[c](en)[/c]{cost}";
                }
            }

            if (__instance.is_auto)
            {
                var craftTime = __instance.craft_time.EvaluateFloat(wgo);
                WriteLog($"[CraftText]: Craft: {__instance.id}, BaseTime: {craftTime}");

                if (craftTime != 0)
                {
                    if (FasterCraftEnabled)
                        craftTime = TimeAdjustment < 0 ? craftTime * TimeAdjustment : craftTime / TimeAdjustment;

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
            if (IsUnsafeDefinition(craft_item_gui.craft_definition)) return;
            if (AutoSelectCraftButtonWithController.Value && LazyInput.gamepad_active)
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
            AlreadyRun = true;
            var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
            WriteLog($"Keeper interacted with: {crafteryWgo.obj_id}.");
        }

        [HarmonyPrefix, HarmonyPatch(nameof(CraftGUI.Open))]
        public static void Open_Prefix()
        {
            AlreadyRun = false;
        }

        [HarmonyPostfix, HarmonyPatch(nameof(CraftGUI.SwitchTab))]
        public static void SwitchTab_Postfix()
        {
            AlreadyRun = true;
        }

        [HarmonyPrefix, HarmonyPatch(nameof(CraftGUI.SwitchTab))]
        public static void SwitchTab_Prefix()
        {
            AlreadyRun = false;
        }
    }

    [HarmonyPatch(typeof(CraftItemGUI))]
    public static class CraftItemGUIPatches
    {
        [HarmonyPostfix, HarmonyPatch(nameof(CraftItemGUI.OnCraftPressed))]
        public static void OnCraftPressed_Postfix(WorldGameObject __state)
        {
            if (!MakeEverythingAuto.Value || __state == null || __state.linked_worker != null || __state.has_linked_worker) return;

            CurrentlyCrafting.Add(__state);
            __state.OnWorkAction();
        }

        [HarmonyPrefix, HarmonyPatch(nameof(CraftItemGUI.OnCraftPressed))]
        public static void OnCraftPressed_Prefix(CraftItemGUI __instance, ref WorldGameObject __state)
        {
            WriteLog($"Craft: {__instance.craft_definition.id}, One time: {__instance.craft_definition.one_time_craft}");
            if (IsUnsafeDefinition(__instance.craft_definition)) return;

            var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
            __state = crafteryWgo;

            var time = __instance.craft_definition.craft_time.EvaluateFloat(crafteryWgo);
            ApplyFasterCraft(ref time);

            // time = Mathf.CeilToInt(time);
            // time *= __instance._amount;
            //
            // if (time >= 300 && !_disableComeBackLaterThoughts.Value)
            // {
            //     ShowComeBackLaterMessage(time);
            // }
        }

        private static void ApplyFasterCraft(ref float time)
        {
            if (!FasterCraftEnabled) return;

            if (TimeAdjustment < 0)
                time /= TimeAdjustment;
            else
                time *= TimeAdjustment;
        }

        // private static void ShowComeBackLaterMessage(float time)
        // {
        //     var endTime = time / 60;
        //     var message = endTime % 1 == 0
        //         ? $"Hmmm guess I'll come back in {time / 60:0} minutes..."
        //         : $"Hmmm guess I'll come back in roughly {time / 60:0} minutes...";
        // }
    }

    // private static void OnReturnToMenu()
    // {
    //     WriteLog($"Resetting variables.");
    //     _alreadyRun = false;
    //     _ccAlreadyRun = false;
    // }
}