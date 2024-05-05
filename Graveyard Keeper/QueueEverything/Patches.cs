using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GYKHelper;
using HarmonyLib;
using UnityEngine;

namespace QueueEverything;

[HarmonyPatch]
public partial class Plugin
{
    [HarmonyAfter("p1xel8ted.gyk.gykhelper", "p1xel8ted.gyk.fastercraftreloaded")]
    [HarmonyPostfix, HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open()
    {
        FasterCraftEnabled = false;
        FasterCraftReloaded = false;
        //  _originalFasterCraft = false;
        ExhaustlessEnabled = false;

        FasterCraftReloaded = Harmony.HasAnyPatches("p1xel8ted.gyk.fastercraftreloaded");

        ExhaustlessEnabled = Tools.ModLoaded("Exhaustless", "Exhaust-less.dll", "Exhaust-less") || Harmony.HasAnyPatches("p1xel8ted.gyk.exhaust-less");
        
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
    public static void CraftItemGUI_Redraw(ref CraftItemGUI __instance)
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
        List<int> craftable = new();
        List<int> notCraftable = new();
        var isMultiQualCraft = false;
        var bCraftable = 0;
        var sCraftable = 0;
        var gCraftable = 0;

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
                var itemValueNeeded = __instance.current_craft.needs[i].value;
                var itemID = __instance.current_craft.needs[i].id;
                var bStarItem = multiInventory.GetTotalCount(itemID + ":1");
                var sStarItem = multiInventory.GetTotalCount(itemID + ":2");
                var gStarItem = multiInventory.GetTotalCount(itemID + ":3");
                bCraftable = bStarItem / itemValueNeeded;
                sCraftable = sStarItem / itemValueNeeded;
                gCraftable = gStarItem / itemValueNeeded;

                if (__instance.current_craft.needs.Count > 1 && __instance.current_craft.needs.All(need => need.id == itemID))
                {
                    var totalItemCraftable = (bStarItem + sStarItem + gStarItem) / itemValueNeeded;
                    craftable.Add(totalItemCraftable);
                }
                else
                {
                    if (bCraftable != 0) craftable.Add(bCraftable);
                    if (sCraftable != 0) craftable.Add(sCraftable);
                    if (gCraftable != 0) craftable.Add(gCraftable);
                }

                if (bCraftable + sCraftable + gCraftable > 0)
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


        var m1 = craftable.Count > 0 ? craftable.Min() : 0;
        var multiMin = Mathf.Max(bCraftable, sCraftable, gCraftable);
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
                    (craftIn.Contains("cooking_table") && !craft.craft_in.Any(craftIn => craftIn.Contains("refugee"))));
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
        public static void CraftDefinition_CanCraftMultiple(ref CraftDefinition __instance, ref bool __result)
        {
            if (!ForceMultiCraft.Value || IsUnsafeDefinition(__instance))
            {
                WriteLog($"[Unsafe]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
                __result = false;
            }
            else
            {
                WriteLog($"[Safe?]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
                __result = true;
            }
        }

        [HarmonyPostfix, HarmonyPatch(nameof(CraftDefinition.GetSpendTxt))]
        public static void CraftDefinition_GetSpendTxt(ref CraftDefinition __instance, ref WorldGameObject wgo, ref string __result,
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
                var num2 = 1f;
                bool flag;
                if (wgo == null)
                {
                    flag = false;
                }
                else
                {
                    var objDef = wgo.obj_def;
                    flag = objDef?.tool_actions != null;
                }

                if (flag)
                    foreach (var equippedTool in from itemType in wgo.obj_def.tool_actions.action_tools
                             where itemType != ItemDefinition.ItemType.Hand
                             select MainGame.me.player.GetEquippedTool(itemType))
                    {
                        bool flag2;
                        if (equippedTool == null)
                        {
                            flag2 = false;
                        }
                        else
                        {
                            var definition = equippedTool.definition;
                            flag2 = definition?.tool_energy_k != null;
                        }

                        if (!flag2 || !equippedTool.definition.tool_energy_k.has_expression) continue;
                        var num3 = equippedTool.definition.tool_energy_k.EvaluateFloat(wgo, MainGame.me.player);
                        if (num3 < num2) num2 = num3;
                    }

                if (!num2.EqualsTo(1f, 0.01f)) num = Mathf.CeilToInt(num * num2);

                if (GlobalCraftControlGUI.is_global_control_active)
                {
                    var gratitudePoints = MainGame.me.player.gratitude_points;
                    var smartExpression = __instance.gratitude_points_craft_cost;
                    if (num > 0) num = Mathf.CeilToInt(num) * multiplier;

                    if (ExhaustlessEnabled)
                    {
                        var adjustedNum = Mathf.CeilToInt(num / 2f);
                        if (gratitudePoints < (smartExpression?.EvaluateFloat(MainGame.me.player) ?? 0f))
                        {
                            if (adjustedNum % 1 == 0)
                                text = text + "(gratitude_points)[c][ff1111]" + adjustedNum.ToString("0") + "[/c]";
                            else
                                text = text + "(gratitude_points)[c][ff1111]" + adjustedNum.ToString("0.0") +
                                       "[/c]";
                        }
                        else
                        {
                            if (adjustedNum % 1 == 0)
                                text = text + "[c](gratitude_points)[/c]" + adjustedNum.ToString("0");
                            else
                                text = text + "[c](gratitude_points)[/c]" + adjustedNum.ToString("0.0");
                        }
                    }
                    else
                    {
                        if (gratitudePoints < (smartExpression?.EvaluateFloat(MainGame.me.player) ?? 0f))
                            text = text + "(gratitude_points)[c][ff1111]" + num + "[/c]";
                        else
                            text = text + "[c](gratitude_points)[/c]" + num;
                    }
                }
                else
                {
                    // if (num > 0) num = Mathf.CeilToInt(num * multiplier);

                    if (ExhaustlessEnabled)
                    {
                        // var adjustedNum = (float)Math.Round(num / 2f, 2);
                        var adjustedNum = Mathf.CeilToInt(num / 2f) * multiplier;
                        if (adjustedNum % 1 == 0)
                            text = text + "[c](en)[/c]" + adjustedNum.ToString("0");
                        else
                            text = text + "[c](en)[/c]" + adjustedNum.ToString("0.0");
                    }
                    else
                    {
                        text = text + "[c](en)[/c]" + Mathf.CeilToInt(num * multiplier);
                    }
                }
            }

            if (__instance.is_auto)
            {
                float num4 = 0;

                num4 = __instance.craft_time.EvaluateFloat(wgo);
                WriteLog($"[CraftText]: Craft: {__instance.id}, BaseTime: {num4}");

                if (num4 != 0)
                {
                    if (FasterCraftEnabled)
                    {
                        if (TimeAdjustment < 0)
                        {
                            num4 *= TimeAdjustment;
                        }
                        else
                        {
                            num4 /= TimeAdjustment;
                        }
                    }

                    num4 = Mathf.CeilToInt(num4);
                    if (num4 > 0) num4 *= multiplier;

                    var timeSpan = TimeSpan.FromSeconds(num4);
                    text = text.ConcatWithSeparator(timeSpan.Hours >= 1
                        ? $"[c](time)[/c]{timeSpan.Hours:0}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}"
                        : $"[c](time)[/c]{timeSpan.Minutes:0}:{timeSpan.Seconds:00}");
                }
            }

            foreach (var item in __instance.needs_from_wgo.Where(item => item.id == "fire"))
            {
                var amount = Mathf.CeilToInt(item.value * multiplier);
                var text2 = amount % 1 == 0
                    ? $"[c](fire2)[/c]{item.value * multiplier:0}"
                    : $"[c](fire2)[/c]{item.value * multiplier:0.0}";
                if (!wgo!.data.IsEnoughItems(item, "", 0, multiplier)) text2 = "[ff1111]" + text2 + "[/c]";

                text = text.ConcatWithSeparator(text2);
            }

            bool flag3;
            if (wgo == null)
            {
                flag3 = false;
            }
            else
            {
                var objDef2 = wgo.obj_def;
                flag3 = objDef2?.tool_actions != null;
            }

            if (flag3 && !__instance.is_auto)
                for (var j = 0; j < wgo.obj_def.tool_actions.action_tools.Count; j++)
                {
                    var itemType2 = wgo.obj_def.tool_actions.action_tools[j];
                    if (itemType2 == ItemDefinition.ItemType.Hand) continue;
                    var text3 = itemType2.ToString().ToLower();
                    var num5 = wgo.obj_def.tool_actions.action_k[j];
                    var num6 = Mathf.FloorToInt(100f * num5);
                    var equippedTool2 = MainGame.me.player.GetEquippedTool(itemType2);
                    if (equippedTool2 == null)
                    {
                        text = text + "\n[c][ff1111](" + text3 + "_s)[-][/c]";
                    }
                    else
                    {
                        num6 = Mathf.FloorToInt(num6 * equippedTool2.definition.efficiency);
                        text += $"\n[c]({text3}_s)[/c]\n{num6}%";
                    }
                }

            __result = text;
        }
    }


    [HarmonyPatch(typeof(CraftGUI))]
    public static class CraftGUIPatches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.ExpandItem))]
        public static void ExpandItem_Postfix(ref CraftGUI __instance, ref CraftItemGUI craft_item_gui)
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
        public static void OnCraftPressed_Prefix(ref CraftItemGUI __instance, ref WorldGameObject __state)
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
        //
        //     Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
        //     Tools.ShowMessage(CrossModFields.Lang.StartsWith("en") ? message : strings.Message, Vector3.zero, sayAsPlayer: true);
        // }
    }

    // private static void OnReturnToMenu()
    // {
    //     WriteLog($"Resetting variables.");
    //     _alreadyRun = false;
    //     _ccAlreadyRun = false;
    // }
}