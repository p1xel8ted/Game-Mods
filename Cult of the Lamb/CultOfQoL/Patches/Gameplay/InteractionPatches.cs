using CultOfQoL.Core;

namespace CultOfQoL.Patches.Gameplay;

[Harmony]
[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public static class InteractionPatches
{
    private static GameManager GI => GameManager.GetInstance();
    private static int CachedWaterableFarmPlots { get; set; }
    private static int CachedFertilizableFarmPlots { get; set; }
    private static int CachedPlantableFarmPlots { get; set; }
    private static float NextFarmCountRefreshAt { get; set; }

    private static int CachedCleanablePoop { get; set; }
    private static int CachedCleanableVomit { get; set; }
    private static int CachedScarecrowsWithBirds { get; set; }
    private static int CachedFillableTroughs { get; set; }
    private static int CachedFillableToolsheds { get; set; }
    private static int CachedFillableMedics { get; set; }
    private static int CachedWolfTrapsEmpty { get; set; }
    private static int CachedWolfTrapsWithWolves { get; set; }
    private static int CachedFillableSeedSilos { get; set; }
    private static int CachedFillableFertilizerSilos { get; set; }
    private static int CachedLevelableFollowers { get; set; }
    private static int CachedSinExtractableFollowers { get; set; }
    private static float NextNonFarmRefreshAt { get; set; }

    private static float NextLabelDebugAt { get; set; }

    /// <summary>
    /// Interactions that game code triggers programmatically during menus/rewards.
    /// These call OnInteract directly while menus are open (e.g., job rewards, rituals, doctrines).
    /// </summary>
    private static readonly HashSet<Type> ProgrammaticInteractionTypes =
    [
        typeof(LoreStone),
        typeof(Interaction_TempleAltar),
        typeof(Interaction_DoctrineStone),
        typeof(Interaction_SimpleConversation)
    ];

    /// <summary>
    /// Prevents ALL interactions when menus are open.
    /// Fixes bug where pressing A on a menu also triggers world interactions with nearby objects.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.OnInteract), typeof(StateMachine))]
    public static bool Interaction_OnInteract(ref Interaction __instance)
    {
        if (ProgrammaticInteractionTypes.Contains(__instance.GetType()))
        {
            return true;
        }

        if (UIMenuBase.ActiveMenus.Count == 0 && !GameManager.InMenu)
        {
            return true;
        }

        Plugin.WriteLog($"[Interaction] Blocking interaction with {__instance.GetType().Name} - {UIMenuBase.ActiveMenus.Count} menu(s) open or InMenu={GameManager.InMenu}");
        return false;
    }

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
        GI.StartCoroutine(FertilizeAllPlots(chosenItem));
    }

    private static IEnumerator WaterAllPlants()
    {
        yield return new WaitForEndOfFrame();
        var waterablePlots = FarmPlot.FarmPlots.Where(p => p != null && p.Structure != null && p.StructureBrain != null && p.StructureBrain.CanWater()).ToList();
        if (waterablePlots.Count == 0 || !MassActionCosts.TryDeductCosts(waterablePlots.Count)) yield break;

        Plugin.WriteLog($"[MassWater] Watering {waterablePlots.Count} plots");
        foreach (var plot in waterablePlots)
        {
            plot.StructureInfo.Watered = true;
            plot.StructureInfo.WateredCount = 0;
            plot.WateringTime = 0.95f;
            plot.UpdateWatered();
            if (MonoSingleton<UIManager>.Instance)
            {
                plot.UpdateCropImage();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(FarmPlot), nameof(FarmPlot.Update))]
    public static void FarmPlot_Update()
    {
        RefreshFarmPlotCounts();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction), nameof(Interaction.Label), MethodType.Getter)]
    public static void Interaction_Label_Getter(Interaction __instance, ref string __result)
    {
        if (!Plugin.ShowMassActionCostPreview.Value) return;
        if (string.IsNullOrWhiteSpace(__result)) return;

        var count = __instance switch
        {
            FarmPlot plot => GetFarmPlotPreviewCount(plot, __result),

            Interaction_Poop when Plugin.MassCleanPoop.Value => GetNonFarmCount(() => CachedCleanablePoop),
            Vomit when Plugin.MassCleanVomit.Value => GetNonFarmCount(() => CachedCleanableVomit),
            Scarecrow when Plugin.MassOpenScarecrows.Value => GetNonFarmCount(() => CachedScarecrowsWithBirds),

            Interaction_RanchTrough when Plugin.MassFillTroughs.Value => GetNonFarmCount(() => CachedFillableTroughs),
            Interaction_Toolshed when Plugin.MassFillToolsheds.Value => GetNonFarmCount(() => CachedFillableToolsheds),
            Interaction_Medic when Plugin.MassFillMedicStations.Value => GetNonFarmCount(() => CachedFillableMedics),

            Interaction_WolfTrap trap when Plugin.MassWolfTraps.Value != MassWolfTrapMode.Disabled =>
                GetNonFarmCount(() => trap.structure.Brain.Data.HasBird ? CachedWolfTrapsWithWolves : CachedWolfTrapsEmpty),

            Interaction_SiloSeeder when Plugin.MassFillSeedSilos.Value => GetNonFarmCount(() => CachedFillableSeedSilos),
            Interaction_SiloFertilizer when Plugin.MassFillFertilizerSilos.Value => GetNonFarmCount(() => CachedFillableFertilizerSilos),

            interaction_FollowerInteraction fi when Plugin.MassLevelUp.Value && fi.follower?.Brain?.CanLevelUp() == true =>
                GetNonFarmCount(() => CachedLevelableFollowers - 1),

            interaction_FollowerInteraction fi when Plugin.MassSinExtract.Value &&
                (fi.follower?.Brain?.CurrentTaskType == FollowerTaskType.Floating || fi.follower?.Brain?.CanGiveSin() == true) =>
                GetNonFarmCount(() => CachedSinExtractableFollowers - 1),

            _ => 0
        };

        if (count <= 0) return;

        var preview = MassActionCosts.GetCostPreviewTextForCount(count);
        if (string.IsNullOrWhiteSpace(preview)) return;

        __result = $"{__result}\n{preview}";

        if (Time.unscaledTime < NextLabelDebugAt) return;
        NextLabelDebugAt = Time.unscaledTime + 1.5f;
        Plugin.WriteLog($"[CostPreview] {__instance.GetType().Name}: count={count}, label='{__result.Replace("\n", " | ")}'");
    }

    private static int GetFarmPlotPreviewCount(FarmPlot plot, string label)
    {
        if (Plugin.MassWater.Value && !string.IsNullOrEmpty(plot.sWater) &&
            label.StartsWith(plot.sWater, StringComparison.Ordinal))
        {
            return CachedWaterableFarmPlots;
        }

        if (Plugin.MassFertilize.Value && !string.IsNullOrEmpty(plot.sFertilize) &&
            label.StartsWith(plot.sFertilize, StringComparison.Ordinal))
        {
            return CachedFertilizableFarmPlots;
        }

        if (Plugin.MassPlantSeeds.Value && !string.IsNullOrEmpty(plot.sPlant) &&
            label.StartsWith(plot.sPlant, StringComparison.Ordinal))
        {
            return CachedPlantableFarmPlots;
        }

        return 0;
    }

    private static int GetNonFarmCount(Func<int> countSelector)
    {
        RefreshNonFarmCounts();
        return countSelector();
    }
    

    private static IEnumerator FertilizeAllPlots(InventoryItem.ITEM_TYPE chosenItem)
    {
        yield return new WaitForEndOfFrame();
        var fertilizablePlots = FarmPlot.FarmPlots.Where(p => p != null && p.Structure != null && p.StructureBrain != null && p.StructureBrain.CanFertilize()).ToList();
        if (fertilizablePlots.Count == 0 || !MassActionCosts.TryDeductCosts(fertilizablePlots.Count)) yield break;
        var itemsNeeded = fertilizablePlots.Count;
        var itemsAvailable = Inventory.GetItemQuantity((int)chosenItem);
        
        if (itemsNeeded > itemsAvailable)
        {
            Plugin.WriteLog($"[MassFertilize] Need {itemsNeeded} fertilizer but only have {itemsAvailable}");
            fertilizablePlots = fertilizablePlots.Take(itemsAvailable).ToList();
        }
        
        Plugin.WriteLog($"[MassFertilize] Fertilizing {fertilizablePlots.Count} plots with {chosenItem}");
        foreach (var plot in fertilizablePlots)
        {
            plot.StructureBrain.AddFertilizer(chosenItem);
            ResourceCustomTarget.Create(plot.gameObject, PlayerFarming.Instance.transform.position, chosenItem, plot.AddFertilizer);
            Inventory.ChangeItemQuantity((int)chosenItem, -1);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private static void RefreshFarmPlotCounts()
    {
        if (!Plugin.ShowMassActionCostPreview.Value) return;
        if (Time.unscaledTime < NextFarmCountRefreshAt) return;
        NextFarmCountRefreshAt = Time.unscaledTime + 0.25f;

        var waterable = 0;
        var fertilizable = 0;
        var plantable = 0;

        foreach (var plot in StructureManager.GetAllStructuresOfType<Structures_FarmerPlot>())
        {
            if (plot?.Data == null) continue;
            if (Plugin.MassWater.Value && plot.CanWater()) waterable++;
            if (Plugin.MassFertilize.Value && plot.CanFertilize()) fertilizable++;
            if (Plugin.MassPlantSeeds.Value && plot.CanPlantSeed() && plot.Data.Inventory.Count == 0) plantable++;
        }

        CachedWaterableFarmPlots = waterable;
        CachedFertilizableFarmPlots = fertilizable;
        CachedPlantableFarmPlots = plantable;
    }

    private static void RefreshNonFarmCounts()
    {
        if (Time.unscaledTime < NextNonFarmRefreshAt) return;
        NextNonFarmRefreshAt = Time.unscaledTime + 0.25f;

        CachedCleanablePoop = Interaction_Poop.Poops.Count(p => p && !p.Activating);
        CachedCleanableVomit = Vomit.Vomits.Count(v => v && !v.Activating);
        CachedScarecrowsWithBirds = Scarecrow.Scarecrows.Count(s => s != null && s.Brain.HasBird);

        CachedFillableTroughs = Interaction_RanchTrough.Troughs
            .Count(t => t != null && !t.StructureBrain.ReservedByPlayer &&
                        t.GetCompostCount() < t.StructureBrain.Capacity);

        CachedFillableToolsheds = Interaction_Toolshed.Toolsheds
            .Count(t => t != null && !t.StructureBrain.ReservedByPlayer &&
                        t.GetCompostCount() < t.StructureBrain.Capacity);

        CachedFillableMedics = Interaction_Medic.Medics
            .Count(m => m != null && !m.StructureBrain.ReservedByPlayer &&
                        m.GetCompostCount() < m.StructureBrain.Capacity);

        CachedWolfTrapsEmpty = Interaction_WolfTrap.Traps
            .Count(t => t != null && !t.structure.Brain.Data.HasBird &&
                        t.structure.Brain.Data.Inventory.Count == 0);

        CachedWolfTrapsWithWolves = Interaction_WolfTrap.Traps
            .Count(t => t != null && t.structure.Brain.Data.HasBird);

        CachedFillableSeedSilos = Interaction_SiloSeeder.SiloSeeders
            .Count(s => s != null && !s.StructureBrain.ReservedByPlayer &&
                        s.GetCompostCount() < s.StructureBrain.Capacity);

        CachedFillableFertilizerSilos = Interaction_SiloFertilizer.SiloFertilizers
            .Count(s => s != null && !s.StructureBrain.ReservedByPlayer &&
                        s.GetCompostCount() < s.StructureBrain.Capacity);

        CachedLevelableFollowers = Helpers.AllFollowers.Count(f => f.Brain != null && f.Brain.CanLevelUp());

        CachedSinExtractableFollowers = Follower.Followers.Count(f =>
            f && f.Brain != null && f.Brain.CanGiveSin() && !f.InGiveSinSequence);
    }

}
