using System.Diagnostics.CodeAnalysis;

namespace CultOfQoL.Patches;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    private static GameManager GI => GameManager.GetInstance();


    
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.OnInteract), typeof(StateMachine))]
    public static bool Interaction_Follower_OnInteract(ref interaction_FollowerInteraction __instance)
    {
        if (UIMenuBase.ActiveMenus.Count > 0 || GameManager.InMenu)
        {
            Plugin.L("Not interacting with follower because a menu//conversation is open.");
            foreach (var menu in UIMenuBase.ActiveMenus)
            {
                Plugin.L($"Menu: {menu}");
            }
            return false;
        }

        if (__instance.follower.Brain.CanLevelUp() && Plugin.MassLevelUp.Value)
        {
            GI.StartCoroutine(LevelUpAllFollowers());
        }
        return true;
    }

    private static IEnumerator LevelUpAllFollowers()
    {
        yield return new WaitForEndOfFrame();
        foreach (var follower in Follower.Followers.Where(follower => follower != null && follower.Brain != null && follower.Brain.CanLevelUp()))
        {
            if (!FollowerPatches.IsFollowerAvailable(follower.Brain) || FollowerPatches.IsFollowerImprisoned(follower.Brain)) continue;
            yield return new WaitForSeconds(0.15f);
            var interaction = follower.Interaction_FollowerInteraction;
            GI.StartCoroutine(interaction.LevelUpRoutine(follower.Brain.CurrentTaskType, null, false));
        }
    }

    private static IEnumerator WaterAllPlants()
    {
        yield return new WaitForEndOfFrame();
        foreach (var plot in FarmPlot.FarmPlots.Where(p => p.StructureBrain != null && p.StructureBrain.CanWater()))
        {
            yield return new WaitForSeconds(0.10f);
            plot.StructureInfo.Watered = true;
            plot.StructureInfo.WateredCount = 0;
            plot.WateringTime = 0.95f;
            plot.UpdateWatered();
            plot.UpdateCropImage();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void FarmPlot_OnInteract(ref FarmPlot __instance)
    {
        if (!Plugin.MassWater.Value) return;
        if (__instance.StructureBrain.CanWater())
        {
            GI.StartCoroutine(WaterAllPlants());
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.AddFertiliser), typeof(InventoryItem.ITEM_TYPE))]
    public static void FarmPlot_AddFertiliser(ref FarmPlot __instance, InventoryItem.ITEM_TYPE chosenItem)
    {
        if (!Plugin.MassFertilize.Value) return;
        GI.StartCoroutine(AddFertiliser(chosenItem));
    }

    private static IEnumerator AddFertiliser(InventoryItem.ITEM_TYPE chosenItem)
    {
        yield return new WaitForEndOfFrame();
        foreach (var plot in FarmPlot.FarmPlots.Where(p => p.StructureBrain != null && p.StructureBrain.CanFertilize()))
        {
            yield return new WaitForSeconds(0.10f);
            plot.StructureBrain.AddFertilizer(chosenItem);
            ResourceCustomTarget.Create(plot.gameObject, PlayerFarming.Instance.transform.position, chosenItem, plot.AddFertilizer);
            Inventory.ChangeItemQuantity((int) chosenItem, -1);
        }
    }


}