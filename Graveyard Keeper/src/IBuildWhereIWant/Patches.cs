namespace IBuildWhereIWant;

[Harmony]
public static class Patches
{
    private const string RefugeeZoneId = "refugee";

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MainGame), nameof(MainGame.Update))]
    public static void MainGame_Update()
    {
        if (!Plugin.CanOpenCraftAnywhere()) return;

        if (LazyInput.gamepad_active && ReInput.players.GetPlayer(0).GetButtonDown(Plugin.MenuControllerButton.Value) ||
            Plugin.MenuKeyBind.Value.IsUp())
        {
            Plugin.OpenCraftAnywhere();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ShowBuildGrid))]
    public static void BuildGrid_ShowBuildGrid(ref bool show)
    {
        if (Plugin.Grid.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        show = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildGrid), nameof(BuildGrid.ClearPreviousTotemRadius))]
    public static void BuildGrid_ClearPreviousTotemRadius(ref bool apply_colors)
    {
        if (Plugin.Grid.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        apply_colors = false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.EnterRemoveMode))]
    public static void BuildModeLogics_EnterRemoveMode(BuildModeLogics __instance)
    {
        if (Plugin.GreyOverlay.Value) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        __instance._remove_grey_spr.SetActive(false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CancelCurrentMode))]
    public static void BuildModeLogics_CancelCurrentMode()
    {
        if (!Plugin.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        Plugin.OpenCraftAnywhere();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.CanBuild))]
    private static void BuildModeLogics_CanBuild(BuildModeLogics __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.DoPlace))]
    private static void BuildModeLogics_DoPlace(BuildModeLogics __instance)
    {
        __instance._multi_inventory = MainGame.me.player.GetMultiInventoryForInteraction();
        if (Plugin.CraftAnywhere && MainGame.me.player.cur_zone.Length <= 0)
        {
            if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
            BuildGrid.ShowBuildGrid(false);
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.FocusCameraOnBuildZone))]
    private static void BuildModeLogics_FocusCameraOnBuildZone(ref string zone_id)
    {
        if (!Plugin.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        zone_id = string.Empty;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.GetObjectRemoveCraftDefinition))]
    private static void BuildModeLogics_GetObjectRemoveCraftDefinition(string obj_id, ref ObjectCraftDefinition __result)
    {
        if (!Plugin.CraftAnywhere || MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;

        Plugin.WriteLog($"[Remove]{obj_id}", true);

        __result = GameBalance.me.craft_obj_data
            .FirstOrDefault(objectCraftDefinition =>
                objectCraftDefinition.out_obj == obj_id &&
                objectCraftDefinition.build_type == ObjectCraftDefinition.BuildType.Remove);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.OnBuildCraftSelected))]
    private static void BuildModeLogics_OnBuildCraftSelected(BuildModeLogics __instance)
    {
        if (!Plugin.CraftAnywhere) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        BuildModeLogics.last_build_desk = Plugin.BuildDeskClone;
        __instance._cur_build_zone_id = Plugin.ZoneId;
        __instance._cur_build_zone = WorldZone.GetZoneByID(Plugin.ZoneId);
        __instance._cur_build_zone_bounds = __instance._cur_build_zone.GetBounds();
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FloatingWorldGameObject), nameof(FloatingWorldGameObject.RecalculateAvailability))]
    public static void FloatingWorldGameObject_RecalculateAvailability()
    {
        if (Plugin.BuildingCollision.Value) return;
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

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.Interact))]
    public static void WorldGameObject_Interact(WorldGameObject __instance)
    {
        if (__instance.obj_def.interaction_type is not ObjectDefinition.InteractionType.None
            and not ObjectDefinition.InteractionType.Craft
            and not ObjectDefinition.InteractionType.Builder)
        {
            Plugin.CraftAnywhere = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.GetUniversalObjectInfo))]
    public static void WorldGameObject_GetUniversalObjectInfo(WorldGameObject __instance, UniversalObjectInfo __result)
    {
        if (Plugin.BuildDeskClone == null) return;
        if (__instance != Plugin.BuildDeskClone) return;
        if (MainGame.me.player.GetMyWorldZoneId().Contains(RefugeeZoneId)) return;
        Lang.Reload();
        __result.header = Lang.Get("Header");
        __result.descr = Lang.Get("Description");
    }
}
