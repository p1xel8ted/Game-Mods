using MassOfTheLamb.Core;

namespace MassOfTheLamb.Patches;

[Harmony]
public static class MassCostPreviewPatches
{
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
