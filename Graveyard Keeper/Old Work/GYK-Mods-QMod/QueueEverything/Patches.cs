using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using HarmonyLib;
using Helper;
using QueueEverything.lang;
using UnityEngine;
using System.IO;

namespace QueueEverything;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.CraftReally))]
    public static void CraftComponent_CraftReally()
    {
        if (!MainGame.game_started) return;
        if (!_cfg.makeEverythingAuto) return;

        foreach (var wgo in currentlyCrafting.Where(wgo => wgo != null && wgo.components.craft.is_crafting && !wgo.has_linked_worker))
        {
            wgo.OnWorkAction();
        }
    }


    [HarmonyPostfix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.QModHelper")]
    [HarmonyPatch(typeof(MainMenuGUI), nameof(MainMenuGUI.Open))]
    public static void MainMenuGUI_Open()
    {
        _fasterCraftEnabled = false;
        _fasterCraftReloaded = false;
        _originalFasterCraft = false;
        _exhaustlessEnabled = false;

        _fasterCraftReloaded = Tools.ModLoaded("FasterCraftReloaded", "FasterCraftReloaded.dll", "FasterCraft Reloaded!") || Harmony.HasAnyPatches("p1xel8ted.GraveyardKeeper.FasterCraftReloaded");
        _originalFasterCraft = Harmony.HasAnyPatches("com.glibfire.graveyardkeeper.fastercraft.mod");
        _exhaustlessEnabled = Tools.ModLoaded("Exhaustless", "Exhaust-less.dll", "Exhaust-less") || Harmony.HasAnyPatches("p1xel8ted.GraveyardKeeper.exhaust-less");

        if (_fasterCraftReloaded)
        {
            _fasterCfg = FcConfig.GetOptions();
            _timeAdjustment = _fasterCfg.craftSpeedMultiplier;
            _fasterCraftEnabled = true;
            Log($"FasterCraft Reloaded! detected, using its config.");
        }
        else if (_originalFasterCraft)
        {
            _fasterCraftEnabled = true;
            Log($"OG FasterCraft detected, using its config.");
            LoadFasterCraftConfig();
        }

        if (_exhaustlessEnabled)
        {
            Log($"Exhaust-less! detected, using its config.");
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.FillCraftsList))]
    public static void CraftComponent_FillCraftsList()
    {
        if (_ccAlreadyRun) return;
        _ccAlreadyRun = true;

        try
        {
            foreach (var craft in GameBalance.me.craft_data)
            {
                if (_cfg.halfFireRequirements)
                {
                    foreach (var item in craft.needs_from_wgo.Where(item => item.id == "fire"))
                    {
                        item.value = Mathf.CeilToInt(item.value / 2f); //
                    }
                }

                if (craft.is_auto || IsUnsafeDefinition(craft)) continue;

                if (_cfg.forceMultiCraft)
                {
                    craft.force_multi_craft = true;
                    craft.disable_multi_craft = false;
                }

                if (_cfg.makeEverythingAuto)
                {
                    var refugeeCraft = craft.craft_in.Any(craftIn => craftIn.Contains("refugee"));
                    var graveCraft = false;
                    var bodyCraft = false;
                    var cookingTableCraft = false;

                    if (!_cfg.makeHandTasksAuto)
                    {
                        graveCraft = craft.craft_in.Any(craftIn => craftIn.Contains("grave"));
                        bodyCraft = craft.craft_in.Any(craftIn => craftIn.Contains("mf_preparation"));
                        cookingTableCraft = craft.craft_in.Any(craftIn => craftIn.Contains("cooking_table") && !refugeeCraft);
                    }

                    if (cookingTableCraft || graveCraft || bodyCraft) continue;

                    var craftEnergyTime = craft.energy.EvaluateFloat(MainGame.me.player);

                    //add a penalty to the time for making it auto.
                    craftEnergyTime *= 1.50f;

                    if (craftEnergyTime % 1 != 0)
                    {
                        craftEnergyTime = Mathf.CeilToInt(craftEnergyTime);
                    }

                    craft.craft_time =
                        SmartExpression.ParseExpression(craftEnergyTime.ToString(CultureInfo.InvariantCulture));
                    craft.energy = SmartExpression.ParseExpression("0");
                    craft.is_auto = true;
                    craft.enqueue_type = CraftDefinition.EnqueueType.CanEnqueue;

                    craft.output.ForEach(output =>
                    {
                        if (output.id is not ("r" or "g" or "b")) return;
                        output.value /= 2;
                        output.value = output.value < 1 ? 1 : Mathf.CeilToInt(output.value);
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Log($"{ex.Message}, {ex.Source}, {ex.StackTrace}", true);
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftDefinition), nameof(CraftDefinition.CanCraftMultiple))]
    public static void CraftDefinition_CanCraftMultiple(ref CraftDefinition __instance, ref bool __result)
    {
        // if (__result) return;
        if (IsUnsafeDefinition(__instance) || !_cfg.forceMultiCraft)
        {
            Log($"[Unsafe]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
            __result = false;
            return;
        }

        Log($"[Safe?]: {__instance.id}, CraftTimeZero: {__instance.craft_time_is_zero}");
        __result = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftDefinition), nameof(CraftDefinition.GetSpendTxt))]
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

                if (_exhaustlessEnabled)
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

                if (_exhaustlessEnabled)
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
            Log($"[CraftText]: Craft: {__instance.id}, BaseTime: {num4}");

            if (num4 != 0)
            {
                if (_fasterCraftEnabled)
                {
                    if (_timeAdjustment < 0)
                    {
                        num4 *= _timeAdjustment;
                    }
                    else
                    {
                        num4 /= _timeAdjustment;
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


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.ExpandItem))]
    public static void CraftGUI_ExpandItem(ref CraftGUI __instance, ref CraftItemGUI craft_item_gui)
    {
        if (IsUnsafeDefinition(craft_item_gui.craft_definition)) return;
        if (_cfg.autoSelectCraftButtonWithController && LazyInput.gamepad_active)
            foreach (var uiButton in craft_item_gui.GetComponentsInChildren<UIButton>())
            {
                if (!uiButton.name.Contains("craft")) continue;
                __instance.gamepad_controller.SetFocusedItem(uiButton.GetComponent<GamepadNavigationItem>());
                uiButton.gameObject.SetActive(true);
            }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open))]
    public static void CraftGUI_Open_Postfix()
    {
        _alreadyRun = true;
        var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
        File.AppendAllText("./QMods/QueueEverything/interacted-objects.txt", crafteryWgo.obj_id + "\n");
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.Open))]
    public static void CraftGUI_Open_Prefix()
    {
        _alreadyRun = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.SwitchTab))]
    public static void CraftGUI_CraftGUI_Postfix()
    {
        // if (_unsafeInteraction) return;
        _alreadyRun = true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftGUI), nameof(CraftGUI.SwitchTab))]
    public static void CraftGUI_CraftGUI_Prefix()
    {
        // if (_unsafeInteraction) return;
        //_craftAmount = 1;
        _alreadyRun = false;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.OnCraftPressed))]
    public static void CraftItemGUI_OnCraftPressed_Postfix(WorldGameObject __state)
    {
        if (!_cfg.makeEverythingAuto) return;
        if (__state == null) return;
        if (__state.linked_worker != null) return;
        if (__state.has_linked_worker) return;
        currentlyCrafting.Add(__state);
        __state.OnWorkAction();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.OnCraftPressed))]
    public static void CraftItemGUI_OnCraftPressed_Prefix(ref CraftItemGUI __instance, ref WorldGameObject __state)
    {
        Log($"Craft: {__instance.craft_definition.id}, One time: {__instance.craft_definition.one_time_craft}");
        if (IsUnsafeDefinition(__instance.craft_definition)) return;

        var crafteryWgo = GUIElements.me.craft.GetCrafteryWGO();
        __state = crafteryWgo;

        var time = __instance.craft_definition.craft_time.EvaluateFloat(crafteryWgo);

        if (_fasterCraftEnabled)
        {
            if (_timeAdjustment < 0)
                time /= _timeAdjustment;
            else
                time *= _timeAdjustment;
        }

        time = Mathf.CeilToInt(time);
        time *= __instance._amount;

        if (time >= 300 && !_cfg.disableComeBackLaterThoughts)
        {
            var endTime = time / 60;
            var message = endTime % 1 == 0
                ? $"Hmmm guess I'll come back in {time / 60:0} minutes..."
                : $"Hmmm guess I'll come back in roughly {time / 60:0} minutes...";

            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            Tools.ShowMessage(CrossModFields.Lang.StartsWith("en") ? message : strings.Message, Vector3.zero, sayAsPlayer: true);
        }
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftItemGUI), nameof(CraftItemGUI.Redraw))]
    public static void CraftItemGUI_Redraw(ref CraftItemGUI __instance)
    {
        if (!__instance.craft_definition.CanCraftMultiple())
        {
            Log($"[CraftItemRedraw]: Unsafe Returning: {__instance.craft_definition.id}");
            __instance._amount = 1;
            return;
        }
        
        if (_alreadyRun)
        {
            Log($"[CraftItemRedraw]: AlreadyRun Returning: {__instance.craft_definition.id}");
            return;
        }
        
        if (__instance.craft_definition.id.Contains("fire") || __instance.craft_definition.id.Contains("fuel"))
        {
            Log($"[CraftItemRedraw]: Unsafe Returning: {__instance.craft_definition.id}");
            __instance._amount = 1;
            return;
        }

        List<int> craftable = new();
        List<int> notCraftable = new();
        craftable.Clear();
        notCraftable.Clear();
        var isMultiQualCraft = false;
        var bCraftable = 0;
        var sCraftable = 0;
        var gCraftable = 0;
        var multiInventory = !GlobalCraftControlGUI.is_global_control_active
            ? MainGame.me.player.GetMultiInventoryForInteraction()
            : GUIElements.me.craft.multi_inventory;
        //const string path = "./qmods/multis.txt";

        //var message = GUIElements.me.craft.GetCrafteryWGO().obj_id + "\n---------------------\n" + "Item: " + __instance.current_craft.id + ", Craft Def: " + __instance.craft_definition.id + "\n";

        for (var i = 0; i < __instance._multiquality_ids.Count; i++)
            if (string.IsNullOrWhiteSpace(__instance._multiquality_ids[i]))
            {
                var itemCount = multiInventory.GetTotalCount(__instance.current_craft.needs[i].id);
                var itemNeed = __instance.current_craft.needs[i].value;
                var itemCraftable = itemCount / itemNeed;
                if (itemCraftable != 0)
                    craftable.Add(itemCraftable);
                else
                    notCraftable.Add(itemCraftable);
                // message += "Item: " + __instance.current_craft.needs[i].id + ", Icon: " + __instance.current_craft.needs[i].GetIcon() + ", Stock: " + itemCount +
                //", Craftable: " + itemCraftable + "\n";
                // message += " - Required for craft: " + itemNeed + "\n";
            }
            else
            {
                isMultiQualCraft = true;
                var itemValueNeeded = __instance.current_craft.needs[i].value;
                var bStarItem = multiInventory.GetTotalCount(__instance.current_craft.needs[i].id + ":1");
                var sStarItem = multiInventory.GetTotalCount(__instance.current_craft.needs[i].id + ":2");
                var gStarItem = multiInventory.GetTotalCount(__instance.current_craft.needs[i].id + ":3");
                bCraftable = bStarItem / itemValueNeeded;
                sCraftable = sStarItem / itemValueNeeded;
                gCraftable = gStarItem / itemValueNeeded;
                if (bCraftable != 0) craftable.Add(bCraftable);
                if (sCraftable != 0) craftable.Add(sCraftable);
                if (gCraftable != 0) craftable.Add(gCraftable);

                // message += "MQ Item " + i + ": " + ____multiquality_ids[i] + "\n - Gold Stock: " + gStarItem +
                //            ", Craftable: " + gStarItem / itemValueNeeded + "\n";
                // message += " - Silver Stock: " + sStarItem + ", Craftable: " + sStarItem / itemValueNeeded +
                //            "\n";
                // message += " - Bronze Stock: " + bStarItem + ", Craftable: " + bCraftable + "\n";
                // message += " - Required for craft: " + __instance.current_craft.needs[i].value + "\n";
                if (bCraftable + sCraftable + gCraftable > 0)
                    if (_cfg.autoSelectHighestQualRecipe)
                    {
                        if (gCraftable > 0)
                            __instance._multiquality_ids[i] = __instance.current_craft.needs[i].id + ":3";
                        else if (sCraftable > 0)
                            __instance._multiquality_ids[i] = __instance.current_craft.needs[i].id + ":2";
                        else
                            __instance._multiquality_ids[i] = __instance.current_craft.needs[i].id + ":1";
                    }
            }

        var m1 = 0;
        if (craftable.Count > 0) m1 = craftable.Min();

        var multiMin = Mathf.Max(bCraftable, sCraftable, gCraftable);
        var min = multiMin <= 0 ? m1 : Math.Min(m1, multiMin);

        if (_cfg.autoMaxMultiQualCrafts)
        {
            if (isMultiQualCraft && multiMin != 0)
            {
                __instance._amount = min;
            }
        }

        // message += "Max Craftable: " + min + "\n";
        // message += "Ingredient list count: " + craftable.Count + "\n";
        // message += "---------------------\n";
        // File.AppendAllText(path, message);

        if (_cfg.autoMaxNormalCrafts)
        {
            if (isMultiQualCraft) return;
            if (notCraftable.Count > 0) return;
            __instance._amount = min;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started) return;
        if (!_cfg.makeEverythingAuto) return;
        if (_craftsStarted) return;

        foreach (var wgo in MainGame.me.world.GetComponentsInChildren<WorldGameObject>(true))
        {
            if (wgo != null && wgo.components.craft.is_crafting && !wgo.has_linked_worker)
            {
                currentlyCrafting.Add(wgo);
            }
        }

        _craftsStarted = true;
    }


    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            ReloadConfig();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }
}