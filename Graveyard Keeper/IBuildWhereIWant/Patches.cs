namespace IBuildWhereIWant;

[Harmony]
public partial class Plugin
{
    private const string RefugeeZoneId = "refugee";

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ShowBuildGrid))]
    public static void BuildGrid_ShowBuildGrid(ref bool show)
    {
        if(Grid.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        show = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ClearPreviousTotemRadius))]
    public static void BuildGrid_ClearPreviousTotemRadius(ref bool apply_colors)
    {
        if(Grid.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        apply_colors = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.EnterRemoveMode))]
    public static void BuildModeLogics_EnterRemoveMode(ref BuildModeLogics __instance)
    {
        if (GreyOverlay.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        __instance._remove_grey_spr.SetActive(false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CancelCurrentMode))]
    public static void BuildModeLogics_CancelCurrentMode()
    {
        if (!CrossModFields.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        OpenCraftAnywhere();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CanBuild))]
    private static void BuildModeLogics_CanBuild(ref BuildModeLogics __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    private static void BuildModeLogics_DoPlace(ref BuildModeLogics __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
        if (CrossModFields.CraftAnywhere && MainGame.me.player.cur_zone.Length <= 0)
        {
            if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
            BuildGrid.ShowBuildGrid(false);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.FocusCameraOnBuildZone))]
    private static void BuildModeLogics_FocusCameraOnBuildZone(ref string zone_id)
    {
        if (!CrossModFields.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        zone_id = string.Empty;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.GetObjectRemoveCraftDefinition))]
    private static void BuildModeLogics_GetObjectRemoveCraftDefinition(string obj_id, ref ObjectCraftDefinition __result)
    {
        if (!CrossModFields.CraftAnywhere || MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;

        WriteLog($"[Remove]{obj_id}", true);

        __result = GameBalance.me.craft_obj_data
            .FirstOrDefault(objectCraftDefinition =>
                objectCraftDefinition.out_obj == obj_id &&
                objectCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Remove);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.OnBuildCraftSelected))]
    private static void BuildModeLogics_OnBuildCraftSelected(ref BuildModeLogics __instance)
    {
        if (!CrossModFields.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        BuildModeLogics.last_build_desk = BuildDeskClone;
        __instance._cur_build_zone_id = Zone;
        __instance._cur_build_zone = WorldZone.GetZoneByID(Zone);
        __instance._cur_build_zone_bounds = __instance._cur_build_zone.GetBounds();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FloatingWorldGameObject), nameof(FloatingWorldGameObject.RecalculateAvailability))]
    public static void FloatingWorldGameObject_RecalculateAvailability()
    {
        if (BuildingCollision.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
    
        FloatingWorldGameObject.can_be_built = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FlowGridCell), nameof(FlowGridCell.IsInsideWorldZone))]
    public static void FlowGridCell_IsInsideWorldZone(ref bool __result)
    {
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        __result = true;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FlowGridCell), nameof(FlowGridCell.IsPlaceAvailable))]
    public static void FlowGridCell_IsPlaceAvailable(ref bool __result)
    {
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        __result = true;
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetUniversalObjectInfo))]
    public static void WorldGameObject_GetUniversalObjectInfo(ref WorldGameObject __instance, ref UniversalObjectInfo __result)
    {
        if (BuildDeskClone == null) return;
        if (__instance != BuildDeskClone) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        __result.header = GetLocalizedString(strings.Header);
        __result.descr = GetLocalizedString(strings.Description);
    }
}