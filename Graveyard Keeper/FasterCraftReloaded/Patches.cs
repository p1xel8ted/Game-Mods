using System.Linq;
using GYKHelper;
using HarmonyLib;

namespace FasterCraftReloaded;

[HarmonyBefore("p1xel8ted.gyk.queueeverything")]
[HarmonyPatch]
public static class Patches
{
    private static readonly string[] Exclude =
    {
        "zombie", "refugee", "bee", "tree", "berry", "bush", "pump", "compost", "peat", "slime", "candelabrum", "incense", "garden", "planting"
    };

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BuildModeLogics), nameof(BuildModeLogics.ProcessRemovingCraft))]
    public static void BuildModeLogics_ProcessRemovingCraft(ref WorldGameObject wgo, ref float delta_time)
    {
        if (!Plugin.IncreaseBuildAndDestroySpeed.Value) return;
        Helpers.Log($"[BuildModeHelpers.Logics.ProcessRemovingCraft]: WGO: {wgo.obj_id}");

        delta_time *= Plugin.BuildAndDestroySpeed.Value;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(WorldGameObject), nameof(WorldGameObject.DoAction))]
    public static void WorldGameObject_DoAction(WorldGameObject other_obj, ref float delta_time)
    {
        if (!Plugin.IncreaseBuildAndDestroySpeed.Value) return;

        Helpers.Log($"[WorldGameObject.DoAction]: WGO: {other_obj.obj_id}");
        delta_time *= Plugin.BuildAndDestroySpeed.Value;
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
            Helpers.Log($"[ModifyCraftSpeed - REJECTED]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        Helpers.Log(
            $"[CC.DoAction]: WGO: {__instance.wgo.obj_id}, WgoIsPlayer: {__instance.wgo.is_player}, Craft: {__instance.current_craft.id}, OtherObj: {__instance.other_obj.obj_id}, OtherWgoIsPlayer: {__instance.other_obj.is_player}");

        delta_time *= Plugin.CraftSpeedMultiplier.Value;
    }


    [HarmonyPrefix]
    [HarmonyAfter("p1xel8ted.GraveyardKeeper.TheSeedEqualizer", "p1xel8ted.GraveyardKeeper.AppleTreesEnhanced")]
    [HarmonyPriority(3)]
    [HarmonyPatch(typeof(CraftComponent), nameof(CraftComponent.ReallyUpdateComponent))]
    public static void CraftComponent_ReallyUpdateComponent(CraftComponent __instance, ref float delta_time)
    {
        if (__instance?.current_craft == null) return;

        Helpers.Log($"[CraftComponent.ReallyUpdateComponent]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");

        if (Plugin.ModifyCompostSpeed.Value && Tools.CompostCraft(__instance.wgo))
        {
            delta_time *= Plugin.CompostSpeedMultiplier.Value;
            Helpers.Log($"[ModifyCompostSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyZombieMinesSpeed.Value && Tools.ZombieMineCraft(__instance.wgo))
        {
            delta_time *= Plugin.ZombieMinesSpeedMultiplier.Value;
            Helpers.Log($"[ModifyZombieMinesSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyZombieSawmillSpeed.Value && Tools.ZombieSawmillCraft(__instance.wgo))
        {
            delta_time *= Plugin.ZombieSawmillSpeedMultiplier.Value;
            Helpers.Log($"[ZombieSawmillSpeedMultiplier]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyPlayerGardenSpeed.Value && Tools.PlayerGardenCraft(__instance.wgo))
        {
            delta_time *= Plugin.PlayerGardenSpeedMultiplier.Value;
            Helpers.Log($"[ModifyPlayerGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyRefugeeGardenSpeed.Value && Tools.RefugeeGardenCraft(__instance.wgo))
        {
            delta_time *= Plugin.RefugeeGardenSpeedMultiplier.Value;
            Helpers.Log($"[ModifyRefugeeGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyZombieGardenSpeed.Value && Tools.ZombieGardenCraft(__instance.wgo))
        {
            delta_time *= Plugin.ZombieGardenSpeedMultiplier.Value;
            Helpers.Log($"[ModifyZombieGardenSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Plugin.ModifyZombieVineyardSpeed.Value && Tools.ZombieVineyardCraft(__instance.wgo))
        {
            delta_time *= Plugin.ZombieVineyardSpeedMultiplier.Value;
            Helpers.Log($"[ModifyZombieVineyardSpeed]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        if (Exclude.Any(__instance.wgo.obj_id.ToLowerInvariant().Contains))
        {
            Helpers.Log($"[ModifyCraftSpeed - REJECTED]: WGO: {__instance.wgo.obj_id}, Craft: {__instance.current_craft.id}");
            return;
        }

        Helpers.Log(
            $"[CC.ReallyUpdateComponent]: WGO: {__instance.wgo.obj_id}, WgoIsPlayer: {__instance.wgo.is_player}, Craft: {__instance.current_craft.id}");
        delta_time *= Plugin.CraftSpeedMultiplier.Value;
    }
}