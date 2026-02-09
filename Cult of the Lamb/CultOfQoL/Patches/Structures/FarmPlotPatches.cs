namespace CultOfQoL.Patches.Structures;

[Harmony]
public static class FarmPlotPatches
{
    private static Dictionary<int, int> _defrostDays = new();

    private static string GetDataFilePath(int slot)
    {
        var dir = Path.Combine(BepInEx.Paths.ConfigPath, "CultOfQoL");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"defrost_{slot}.json");
    }

    internal static void SaveDefrostData()
    {
        try
        {
            var path = GetDataFilePath(SaveAndLoad.SAVE_SLOT);
            if (_defrostDays.Count == 0)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                return;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(_defrostDays));
        }
        catch (Exception ex)
        {
            Plugin.WriteLog($"[RotFertilizer] Failed to save defrost data: {ex.Message}", Plugin.LogType.Error);
        }
    }

    internal static void LoadDefrostData()
    {
        try
        {
            var path = GetDataFilePath(SaveAndLoad.SAVE_SLOT);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _defrostDays = JsonConvert.DeserializeObject<Dictionary<int, int>>(json) ?? new Dictionary<int, int>();
                Plugin.WriteLog($"[RotFertilizer] Loaded defrost data for slot {SaveAndLoad.SAVE_SLOT}: {_defrostDays.Count} entries");
            }
            else
            {
                _defrostDays.Clear();
            }
        }
        catch (Exception ex)
        {
            Plugin.WriteLog($"[RotFertilizer] Failed to load defrost data: {ex.Message}", Plugin.LogType.Error);
            _defrostDays.Clear();
        }
    }

    static FarmPlotPatches()
    {
        SaveAndLoad.OnLoadComplete += LoadDefrostData;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_FarmerPlot), nameof(Structures_FarmerPlot.AddFertilizer))]
    public static void AddFertilizer_Postfix(Structures_FarmerPlot __instance, InventoryItem.ITEM_TYPE type)
    {
        if (!Plugin.RotFertilizerDecay.Value) return;
        if (type != InventoryItem.ITEM_TYPE.POOP_ROTSTONE) return;

        _defrostDays[__instance.Data.ID] = TimeManager.CurrentDay;
        Plugin.WriteLog($"[RotFertilizer] Tracked defrost for plot {__instance.Data.ID} on day {TimeManager.CurrentDay}");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_FarmerPlot), nameof(Structures_FarmerPlot.OnNewPhaseStarted))]
    public static void OnNewPhaseStarted_Postfix(Structures_FarmerPlot __instance)
    {
        if (!Plugin.RotFertilizerDecay.Value) return;
        if (!__instance.Data.DefrostedCrop) return;

        var id = __instance.Data.ID;
        var duration = Plugin.RotFertilizerDuration.Value;

        // Pre-existing defrost (from before feature enabled or from a save without tracking data)
        if (!_defrostDays.ContainsKey(id))
        {
            _defrostDays[id] = TimeManager.CurrentDay;
            return;
        }

        if (TimeManager.CurrentDay - _defrostDays[id] < duration) return;

        // Warming has expired
        __instance.Data.DefrostedCrop = false;
        _defrostDays.Remove(id);

        Plugin.WriteLog($"[RotFertilizer] Warming expired on plot {id} after {duration} days");

        var farmPlot = FarmPlot.GetFarmPlot(id);
        if (farmPlot != null)
        {
            farmPlot.UpdateWatered();
        }
    }

    /// <summary>
    /// When decay is enabled and warming has expired (DefrostedCrop = false), block growth
    /// even though POOP_ROTSTONE is still on the plot. The fertilizer stays so that
    /// SetWithered()'s guard prevents crop death — crops freeze but don't wither.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_FarmerPlot), nameof(Structures_FarmerPlot.CanGrow))]
    public static void CanGrow_Postfix(Structures_FarmerPlot __instance, SeasonsManager.Season season, ref bool __result)
    {
        if (!__result) return;
        if (!Plugin.RotFertilizerDecay.Value) return;

        // Only intervene when warming has expired but rot fertilizer is still present
        if (__instance.Data.DefrostedCrop) return;
        if (!__instance.HasFertilized() || __instance.GetFertilizer().type != 187) return;

        // Don't block if other growth enablers are active
        if (__instance.NearbyFarmCropGrower()) return;
        if (season == SeasonsManager.Season.Winter && __instance.GrowsDuringWinter) return;

        __result = false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save), [])]
    public static void SaveAndLoad_Save_Prefix()
    {
        SaveDefrostData();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.DeleteSaveSlot))]
    public static void DeleteSaveSlot_Postfix(int saveSlot)
    {
        var path = GetDataFilePath(saveSlot);
        if (File.Exists(path))
        {
            File.Delete(path);
            Plugin.WriteLog($"[RotFertilizer] Deleted defrost data for slot {saveSlot}");
        }
    }
}
