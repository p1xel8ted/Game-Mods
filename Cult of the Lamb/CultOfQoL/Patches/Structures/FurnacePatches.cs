namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class FurnacePatches
{
    static FurnacePatches()
    {
        TimeManager.OnNewPhaseStarted += OnNewPhaseStarted;
    }

    /// <summary>
    /// Each game phase during winter, drains furnace fuel based on the number of proximity heaters.
    /// More heaters = faster fuel consumption, adding challenge to winter management.
    /// </summary>
    private static void OnNewPhaseStarted()
    {
        if (!Plugin.FurnaceHeaterScaling.Value) return;
        if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter) return;

        // Get heater count first — GetAllStructuresOfType uses a shared cache
        var heaterCount = StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.PROXIMITY_FURNACE).Count;
        if (heaterCount <= 0) return;

        var costPerHeater = Plugin.FurnaceHeaterFuelCost.Value;
        var totalDrain = heaterCount * costPerHeater;

        foreach (var furnace in StructureManager.GetAllStructuresOfType<Structures_Furnace>())
        {
            if (furnace.Data.Fuel <= 0) continue;

            var fuelBefore = furnace.Data.Fuel;
            furnace.UpdateFuel(totalDrain);
            Plugin.WriteLog($"[FurnaceHeater] Drained {totalDrain} fuel ({heaterCount} heaters x {costPerHeater}) " +
                            $"from furnace {furnace.Data.ID} (was {fuelBefore}, now {furnace.Data.Fuel})");
        }
    }
}
