using System.Linq;
using FasterCraftReloaded.lang;
using HarmonyLib;
using Helper;
using UnityEngine;
using Tools = Helper.Tools;

namespace FasterCraftReloaded;

[HarmonyPatch]
public static partial class MainPatcher
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.ProcessRemovingCraft))]
    public static void BuildModeLogics_ProcessRemovingCraft(ref WorldGameObject wgo, ref float delta_time)
    {
        if (!_cfg.IncreaseBuildAndDestroySpeed) return;
        Log($"[BuildModeLogics.ProcessRemovingCraft]: WGO: {wgo.obj_id}");

        delta_time *= 4f;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DoAction))]
    public static void WorldGameObject_DoAction(WorldGameObject other_obj, ref float delta_time)
    {
        if (!_cfg.IncreaseBuildAndDestroySpeed) return;

        Log($"[WorldGameObject.DoAction]: WGO: {other_obj.obj_id}");
        delta_time *= 4;
    }


    [HarmonyPrefix]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.DoAction))]
    public static void CraftComponent_DoAction(CraftComponent __instance, ref float delta_time)
    {
        // if (__instance?.current_craft == null) return;
        if (__instance.other_obj == null) return;
        if (!__instance.other_obj.is_player) return;

        if (Exclude.Any(__instance.wgo.obj_id.ToLowerInvariant().Contains))
        {
            Log($"[ModifyCraftSpeed - REJECTED]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        Log(
            $"[CC.DoAction]: WGO: {__instance.wgo.obj_id}, WgoIsPlayer: {__instance.wgo.is_player}, Craft: {__instance.current_craft.id}, OtherObj: {__instance.other_obj.obj_id}, OtherWgoIsPlayer: {__instance.other_obj.is_player}");

        delta_time *= _cfg.CraftSpeedMultiplier;
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.TheSeedEqualizer", "p1xel8ted.GraveyardKeeper.AppleTreesEnhanced")]
    [HarmonyPriority(3)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;

        Log($"[CraftComponent.ReallyUpdateComponent]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");

        if (_cfg.ModifyCompostSpeed && Tools.CompostCraft(__instance.wgo))
        {
            delta_time *= _cfg.CompostSpeedMultiplier;
            Log($"[ModifyCompostSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyZombieMinesSpeed && Tools.ZombieMineCraft(__instance.wgo))
        {
            delta_time *= _cfg.ZombieMinesSpeedMultiplier;
            Log($"[ModifyZombieMinesSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyZombieSawmillSpeed && Tools.ZombieSawmillCraft(__instance.wgo))
        {
            delta_time *= _cfg.ZombieSawmillSpeedMultiplier;
            Log($"[ZombieSawmillSpeedMultiplier]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyPlayerGardenSpeed && Tools.PlayerGardenCraft(__instance.wgo))
        {
            delta_time *= _cfg.PlayerGardenSpeedMultiplier;
            Log($"[ModifyPlayerGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyRefugeeGardenSpeed && Tools.RefugeeGardenCraft(__instance.wgo))
        {
            delta_time *= _cfg.RefugeeGardenSpeedMultiplier;
            Log($"[ModifyRefugeeGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyZombieGardenSpeed && Tools.ZombieGardenCraft(__instance.wgo))
        {
            delta_time *= _cfg.ZombieGardenSpeedMultiplier;
            Log($"[ModifyZombieGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (_cfg.ModifyZombieVineyardSpeed && Tools.ZombieVineyardCraft(__instance.wgo))
        {
            delta_time *= _cfg.ZombieVineyardSpeedMultiplier;
            Log($"[ModifyZombieVineyardSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Exclude.Any(__instance.wgo.obj_id.ToLowerInvariant().Contains))
        {
            Log($"[ModifyCraftSpeed - REJECTED]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        Log(
            $"[CC.ReallyUpdateComponent]: WGO: {__instance.wgo.obj_id}, WgoIsPlayer: {__instance.wgo.is_player}, Craft: {__instance.current_craft.id}");
        delta_time *= _cfg.CraftSpeedMultiplier;
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