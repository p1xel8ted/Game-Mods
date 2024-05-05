using System.Linq;
using System.Threading;
using HarmonyLib;
using Helper;
using IBuildWhereIWant.lang;
using Rewired;
using UnityEngine;
using Tools = Helper.Tools;

namespace IBuildWhereIWant;

[HarmonyPatch]
public static partial class MainPatcher
{
      [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ShowBuildGrid))]
    public static void BuildGrid_ShowBuildGrid(ref bool show)
    {
        if (!_cfg.disableGrid) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
        show = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ClearPreviousTotemRadius))]
    public static void BuildGrid_ClearPreviousTotemRadius(ref bool apply_colors)
    {
        if (!_cfg.disableGrid) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
        apply_colors = false;
    }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.EnterRemoveMode))]
        public static void BuildModeLogics_EnterRemoveMode(ref BuildModeLogics __instance)
        {
            if (!_cfg.disableGreyRemoveOverlay) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            __instance._remove_grey_spr.SetActive(false);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.CancelCurrentMode))]
        public static void BuildModeLogics_CancelCurrentMode()
        {
            if (!CrossModFields.CraftAnywhere) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            OpenCraftAnywhere();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.CanBuild))]
        private static void BuildModeLogics_CanBuild(ref BuildModeLogics __instance)
        {
            __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.DoPlace))]
        private static void BuildModeLogics_DoPlace(ref BuildModeLogics __instance)
        {
            __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
            if (CrossModFields.CraftAnywhere && MainGame.me.player.cur_zone.Length <= 0)
            {
                if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
                BuildGrid.ShowBuildGrid(false);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.FocusCameraOnBuildZone))]
        private static void BuildModeLogics_FocusCameraOnBuildZone(ref string zone_id)
        {
            if (!CrossModFields.CraftAnywhere) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            zone_id = string.Empty;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.GetObjectRemoveCraftDefinition))]
        private static void BuildModeLogics_GetObjectRemoveCraftDefinition(string obj_id, ref ObjectCraftDefinition __result)
        {
            if (!CrossModFields.CraftAnywhere) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            Debug.LogError($"[Remove]{obj_id}");
            foreach (var objectCraftDefinition in GameBalance.me.craft_obj_data.Where(objectCraftDefinition =>
                         objectCraftDefinition.out_obj == obj_id && objectCraftDefinition.build_type ==
                         ObjectCraftDefinition.BuildType.Remove))
            {
                __result = objectCraftDefinition;
                return;
            }

            __result = null;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildModeLogics),nameof(BuildModeLogics.OnBuildCraftSelected))]
        private static void BuildModeLogics_OnBuildCraftSelected(ref BuildModeLogics __instance)
        {
            if (!CrossModFields.CraftAnywhere) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            BuildModeLogics.last_build_desk = _buildDeskClone;
            __instance._cur_build_zone_id = Zone;
            __instance._cur_build_zone = WorldZone.GetZoneByID(Zone);
            __instance._cur_build_zone_bounds = __instance._cur_build_zone.GetBounds();
        }
    


        [HarmonyPostfix]
        [HarmonyPatch(typeof(FloatingWorldGameObject), nameof(FloatingWorldGameObject.RecalculateAvailability))]
        public static void FloatingWorldGameObject_RecalculateAvailability()
        {
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            if (_cfg.disableBuildingCollision)
            {
                FloatingWorldGameObject.can_be_built = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FlowGridCell),nameof(FlowGridCell.IsInsideWorldZone))]
        public static void FlowGridCell_IsInsideWorldZone(ref bool __result)
        {
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            __result = true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FlowGridCell),nameof(FlowGridCell.IsPlaceAvailable))]
        public static void FlowGridCell_IsPlaceAvailable(ref bool __result)
        {
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            __result = true;
        }
    


        [HarmonyPrefix]
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

            if (!MainGame.game_started || MainGame.me.player.is_dead || MainGame.me.player.IsDisabled() ||
                MainGame.paused || !BaseGUI.all_guis_closed) return;

            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;

            if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(_cfg.menuControllerButton))
            {
                OpenCraftAnywhere();
            }

            if (Input.GetKeyUp(_cfg.menuKeyBind))
            {
                OpenCraftAnywhere();
            }
        }
    

 
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetUniversalObjectInfo))]
        public static void WorldGameObject_GetUniversalObjectInfo(ref WorldGameObject __instance, ref UniversalObjectInfo __result)
        {

            if (_buildDeskClone == null) return;
            if (__instance != _buildDeskClone) return;
            if (MainGame.me.player.GetMyWorldZoneId().Contains("refugee")) return;
            Thread.CurrentThread.CurrentUICulture = CrossModFields.Culture;
            __result.header = strings.Header;
            __result.descr = strings.Description;
        }  
}