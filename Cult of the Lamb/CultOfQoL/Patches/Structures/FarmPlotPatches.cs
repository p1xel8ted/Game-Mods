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
            Plugin.WriteLog($"Failed to save defrost data: {ex.Message}", Plugin.LogType.Error);
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
                Plugin.WriteLog($"Loaded defrost data for slot {SaveAndLoad.SAVE_SLOT}: {_defrostDays.Count} entries");
            }
            else
            {
                _defrostDays.Clear();
            }
        }
        catch (Exception ex)
        {
            Plugin.WriteLog($"Failed to load defrost data: {ex.Message}", Plugin.LogType.Error);
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
        Plugin.WriteLog($"Tracked defrost for plot {__instance.Data.ID} on day {TimeManager.CurrentDay}");
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

        Plugin.WriteLog($"Rot fertilizer expired on plot {id} after {duration} days");

        var farmPlot = FarmPlot.GetFarmPlot(id);
        if (farmPlot != null)
        {
            farmPlot.UpdateWatered();
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save), new Type[0])]
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
            Plugin.WriteLog($"Deleted defrost data for slot {saveSlot}");
        }
    }
}
