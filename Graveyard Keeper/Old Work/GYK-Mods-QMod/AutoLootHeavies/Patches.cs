using System.Collections.Generic;
using AutoLootHeavies.lang;
using HarmonyLib;
using Helper;
using UnityEngine;

namespace AutoLootHeavies;

[HarmonyPatch]
[HarmonyPriority(1)]
public static partial class MainPatcher
{
    private static bool _initialFullUpdate;

    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (MainGame.game_started) return;
        if (!_initialFullUpdate)
        {
            _initialFullUpdate = true;
            MainGame.me.StartCoroutine(RunFullUpdate());
        }
    }

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.reloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }

        if (!MainGame.game_started) return;

        if (Input.GetKeyUp(_cfg.toggleTeleportToDumpSiteKeybind))
        {
            if (!_cfg.teleportToDumpSiteWhenAllStockPilesFull)
            {
                _cfg.teleportToDumpSiteWhenAllStockPilesFull = true;
                Tools.ShowMessage(strings.TeleOn, Vector3.zero);
            }
            else
            {
                _cfg.teleportToDumpSiteWhenAllStockPilesFull = false;
                Tools.ShowMessage(strings.TeleOff, Vector3.zero);
            }

            UpdateConfig();
        }

        if (Input.GetKeyUp(_cfg.setTimberLocationKeybind))
        {
            _cfg.designatedTimberLocation = MainGame.me.player_pos;
            Tools.ShowMessage(strings.DumpTimber, _cfg.designatedTimberLocation);
            UpdateConfig();
        }

        if (Input.GetKeyUp(_cfg.setOreLocationKeybind))
        {
            _cfg.designatedOreLocation = MainGame.me.player_pos;
            Tools.ShowMessage(strings.DumpOre, _cfg.designatedOreLocation);
            UpdateConfig();
        }

        if (Input.GetKeyUp(_cfg.setStoneLocationKeybind))
        {
            _cfg.designatedStoneLocation = MainGame.me.player_pos;
            Tools.ShowMessage(strings.DumpStone, _cfg.designatedStoneLocation);
            UpdateConfig();
        }
    }


    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.DropOverheadItem))]
    public static bool BaseCharacterComponent_DropOverheadItem_Postfix(ref BaseCharacterComponent __instance)
      //  ref bool __state)
    {
        if (!__instance.wgo.is_player) return true;
        if (!OverheadItemIsHeavy(__instance.overhead_item)) return true;
        //if (!__state) return;

        List<Item> insert = new();
        var item = __instance.overhead_item;
        var itemId = __instance.overhead_item.id;
        insert.Clear();

        insert.Add(__instance.overhead_item);

        Log(
            $"Refreshing and re-sorting stockpile distances.");
        foreach (var pile in SortedStockpiles)
        {
            var distance = Vector3.Distance(MainGame.me.player_pos, pile.GetStockpileObject().pos3);
            pile.SetDistanceFromPlayer(distance);
        }

        SortedStockpiles.Sort((x, y) => x.GetDistanceFromPlayer().CompareTo(y.GetDistanceFromPlayer()));


        foreach (var stockpile in SortedStockpiles)
        {
            Log(
                $"Trying to insert {itemId} into {stockpile.GetStockpileObject()}, {stockpile.GetDistanceFromPlayer()} units away.");
            var success = TryPutToInventoryAndNull(__instance, stockpile.GetStockpileObject(), insert);
            if (success)
            {
                Log(
                    $"Successfully inserted {itemId} into {stockpile.GetStockpileObject()}, {stockpile.GetDistanceFromPlayer()} units away.");
                ShowLootAddedIcon(item);
                break;
            }

            Log(
                $"Failed to insert {itemId} into {stockpile.GetStockpileObject()}, {stockpile.GetDistanceFromPlayer()} units away.");
        }

        if (__instance.overhead_item != null)
        {
            if (_cfg.teleportToDumpSiteWhenAllStockPilesFull)
            {
                TeleportItem(__instance, __instance.overhead_item);
                //Log($"Teleporting {itemId} to dump site.");
            }
            else
            {
                DropObjectAndNull(__instance, __instance.overhead_item);
                Log($"Dropping object due to teleportation being disabled.");
            }
        }

        return false;
    }


    // [HarmonyPostfix]
    [HarmonyPrefix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(BaseCharacterComponent), nameof(BaseCharacterComponent.SetOverheadItem))]
    public static void BaseCharacterComponent_SetOverheadItem(ref BaseCharacterComponent __instance, ref Item item)
    {
        if (__instance.wgo.is_player && item != null && OverheadItemIsHeavy(item))
        {
            MainGame.me.StartCoroutine(RunFullUpdate());
            // UpdateStockpiles();
        }
    }

    private static bool StockpileIsValid(WorldGameObject wgo)
    {
        return wgo.obj_id.Contains(Constants.ItemObjectId.Timber) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Ore) ||
               wgo.obj_id.Contains(Constants.ItemObjectId.Stone);
    }


    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(ref WorldGameObject __instance)
    {
        if (StockpileIsValid(__instance))
        {
            AddStockpile(__instance);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnAddNewWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnAddNewWGOt(ref WorldGameObject wgo)
    {
        if (StockpileIsValid(wgo))
        {
            AddStockpile(wgo);
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(WorldMap), nameof(WorldMap.OnDestroyWGO), typeof(WorldGameObject))]
    public static void WorldGameObject_OnDestroyWGO(ref WorldGameObject wgo)
    {
        if (StockpileIsValid(wgo))
        {
            RemoveStockpile(wgo);
        }
    }
}