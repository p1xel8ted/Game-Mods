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
            GI.StartCoroutine(LevelUpAllFollowers(__instance));
        }
        return true;
    }

    private static IEnumerator LevelUpAllFollowers(interaction_FollowerInteraction interactionFollowerInteraction)
    {
        yield return new WaitForEndOfFrame();
        foreach (var follower in Follower.Followers.Where(follower => follower && follower.Brain != null && follower.Brain.CanLevelUp()))
        {
            if (!FollowerPatches.IsFollowerAvailable(follower.Brain) || FollowerPatches.IsFollowerImprisoned(follower.Brain)) continue;
            yield return new WaitForSeconds(0.15f);
            try
            {
                Plugin.L($"Attempting to level up follower {follower.name}");
                GI.StartCoroutine(interactionFollowerInteraction.LevelUpRoutine(follower.Brain.CurrentTaskType, null, false, true, false));
            }
            catch (Exception e)
            {
                Plugin.Log.LogError($"Error leveling up follower: {e.Message}");
            }
        }
    }

    // private static IEnumerator PickAllPlants()
    // {
    //     yield return new WaitForEndOfFrame();
    //     foreach (var plot in FarmPlot.FarmPlots.Where(p => p.StructureBrain is {IsFullyGrown: true}))
    //     {
    //         yield return new WaitForSeconds(0.10f);
    //         Plugin.Log.LogWarning($"Picking crop from {plot.name}");
    //         plot.Harvested();
    //     }
    // }

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
    
    // [HarmonyReversePatch]
    // [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract), typeof(StateMachine))]
    // [MethodImpl(MethodImplOptions.NoInlining)]
    // public static void BaseOnInteract(Interaction_Berries instance, StateMachine state)
    // {
    //     // This will be replaced with the original method's body at runtime
    // }
    //
    // //Interaction_Berries : Interaction
    // [HarmonyPrefix]
    // [HarmonyPatch(typeof(Interaction_Berries), nameof(Interaction_Berries.OnInteract), typeof(StateMachine))]
    // public static void Interaction_Berries_OnInteract(ref Interaction_Berries __instance, StateMachine state)
    // {
    //     foreach (var berry in Interaction_Berries.Berries.Where(berry => berry.StructureBrain.IsCrop))
    //     {
    //         Plugin.Log.LogWarning($"Picking berries from {berry.name}");
    //         BaseOnInteract(__instance, state);
    //       
    //         berry.activatingPlayers.Add(berry._playerFarming);
    //         GI.StartCoroutine(berry.PickBerries(berry._playerFarming));
    //     }
    // }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.OnInteract), typeof(StateMachine))]
    public static void FarmPlot_OnInteract(ref FarmPlot __instance)
    {
        if (Plugin.MassWater.Value && __instance.StructureBrain.CanWater())
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