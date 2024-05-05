using System.Linq;
using AppleTreesEnhanced.lang;
using HarmonyLib;
using Helper;
using UnityEngine;

namespace AppleTreesEnhanced;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyBefore("p1xel8ted.GraveyardKeeper.FasterCraftReloaded")]
    [HarmonyPriority(2)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (!_cfg.BoostGrowSpeedWhenRaining) return;
        if (__instance?.current_craft == null) return;
        if (!EnvironmentEngine.me.is_rainy) return;
        if (!HarvestSpawners.Any(__instance.current_craft.id.Contains)) return;
        
        Log($"It's raining! Boosting base grow speed of {__instance.current_craft.id} by 100%!");

        delta_time *= 2;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanBuyItem), typeof(ItemDefinition), typeof(bool))]
    public static void Vendor_CanBuyItem(ref Vendor __instance, ref ItemDefinition item_def, ref bool __result)
    {
        if (!_cfg.BeeKeeperBuyback) return;
        if (!__instance.id.Contains(Constants.OutputItems.Bee)) return;
        if (item_def == null) return;
        if (SellThesePlease.Any(item_def.id.Equals))
        {
            __result = true;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Vendor), nameof(Vendor.CanTradeItem), typeof(ItemDefinition))]
    public static void Vendor_CanTradeItem(ref Vendor __instance, ref ItemDefinition item_def, ref bool __result)
    {
        if (!_cfg.BeeKeeperBuyback) return;
        if (!__instance.id.Contains(Constants.OutputItems.Bee)) return;
        if (item_def == null) return;
        if (SellThesePlease.Any(item_def.id.Equals))
        {
            __result = true;
        }
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.ReplaceWithObject))]
    public static void WorldGameObject_ReplaceWithObject(ref WorldGameObject __instance, ref string new_obj_id)
    {
        if (string.Equals(new_obj_id, Constants.HarvestReady.BeeHouse) && IsPlayerBeeHive(__instance))
        {
            ProcessGardenBeeHive(__instance);
        }
        else if (string.Equals(new_obj_id, Constants.HarvestReady.GardenAppleTree))
        {
            ProcessGardenAppleTree(__instance);
        }
        else if (string.Equals(new_obj_id, Constants.HarvestReady.GardenBerryBush))
        {
            ProcessGardenBerryBush(__instance);
        }
        else if (string.Equals(new_obj_id, Constants.HarvestReady.WorldBerryBush1))
        {
            ProcessBerryBush1(__instance);
        }
        else if (string.Equals(new_obj_id, Constants.HarvestReady.WorldBerryBush2))
        {
            ProcessBerryBush2(__instance);
        }
        else if (string.Equals(new_obj_id, Constants.HarvestReady.WorldBerryBush3))
        {
            ProcessBerryBush3(__instance);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!MainGame.game_started || !MainGame.loaded_from_scene_main || _updateDone) return;

        var dudBees = Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Constants.HarvestGrowing.BeeHouse).Where(b => b.progress <= 0)
            .Where(IsPlayerBeeHive);
        var dudBeesCount = 0;
        foreach (var dudBee in dudBees)
        {
            dudBeesCount++;
            ProcessBeeRespawn(dudBee);
            Log($"Fixed DudBee {dudBeesCount}");
        }

        var dudTrees = Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Constants.HarvestGrowing.GardenAppleTree).Where(b => b.progress <= 0);
        var dudTreeCount = 0;
        foreach (var dudTree in dudTrees)
        {
            dudTreeCount++;
            ProcessRespawn(dudTree, Constants.HarvestGrowing.GardenAppleTree,
                Constants.HarvestSpawner.GardenAppleTree);
            Log($"Fixed DudGardenTree {dudTreeCount}");
        }

        var dudBushes = Object.FindObjectsOfType<WorldGameObject>(true)
            .Where(a => a.obj_id == Constants.HarvestGrowing.GardenBerryBush).Where(b => b.progress <= 0);
        var dudBushCount = 0;
        foreach (var dudBush in dudBushes)
        {
            dudBushCount++;
            ProcessRespawn(dudBush, Constants.HarvestGrowing.GardenBerryBush,
                Constants.HarvestSpawner.GardenBerryBush);
            Log($"Fixed DudGardenBush {dudBushCount}");
        }

        var readyBees = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Constants.HarvestReady.BeeHouse)
            .Where(IsPlayerBeeHive);
        var readyGardenTrees = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Constants.HarvestReady.GardenAppleTree);
        var readyGardenBushes = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => a.obj_id == Constants.HarvestReady.GardenBerryBush);
        var readyWorldBushes = Object.FindObjectsOfType<WorldGameObject>(true).Where(a => WorldReadyHarvests.Contains(a.obj_id));

        foreach (var item in readyBees)
        {
            ProcessGardenBeeHive(item);
        }

        foreach (var item in readyGardenTrees)
        {
            ProcessGardenAppleTree(item);
        }

        foreach (var item in readyGardenBushes)
        {
            ProcessGardenBerryBush(item);
        }

        foreach (var item in readyWorldBushes)
        {
            switch (item.obj_id)
            {
                case Constants.HarvestReady.WorldBerryBush1:
                    ProcessBerryBush1(item);
                    break;

                case Constants.HarvestReady.WorldBerryBush2:
                    ProcessBerryBush2(item);
                    break;

                case Constants.HarvestReady.WorldBerryBush3:
                    ProcessBerryBush3(item);
                    break;
            }
        }

        _updateDone = true;
    }

    //hooks into the time of day update and saves if the K key was pressed
    [HarmonyPrefix]
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.Update))]
    public static void TimeOfDay_Update()
    {
        if (Input.GetKeyUp(_cfg.ReloadConfigKeyBind))
        {
            _cfg = Config.GetOptions();

            if (Time.time - CrossModFields.ConfigReloadTime > CrossModFields.ConfigReloadTimeLength)
            {
                Tools.ShowMessage(GetLocalizedString(strings.ConfigMessage), Vector3.zero);
                CrossModFields.ConfigReloadTime = Time.time;
            }
        }
    }
}