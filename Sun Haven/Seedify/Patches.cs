namespace Seedify;

[Harmony]
public static class Patches
{
    private static readonly List<Season> AllSeasons = [Season.Fall, Season.Winter, Season.Spring, Season.Summer];

    private static readonly MethodInfo CropDieMethod = AccessTools.Method(typeof(DecorationUpdater), nameof(DecorationUpdater.CropDie));
    private static readonly FieldInfo CropInfosField = AccessTools.Field(typeof(ItemInfoDatabase), nameof(ItemInfoDatabase.cropInfos));
    private static readonly MethodInfo ScarecrowGetter = AccessTools.Property(typeof(CropSaveData), nameof(CropSaveData.scareCrowEffects)).GetGetMethod();

    private static readonly FieldInfo ModifiedCropInfosField = AccessTools.Field(typeof(Patches), nameof(ModifiedCropInfos));
    private static readonly MethodInfo ReplaceCropDieMethodScarecrow = AccessTools.Method(typeof(Patches), nameof(CropDieReplacerScarecrow));

    internal static Dictionary<int, CropInfo> ModifiedCropInfos = new();
    private static Coroutine UpdateAllSeedsRoutine { get; set; }
    private static Coroutine UpdateAllCropsRoutine { get; set; }
    private static Coroutine UpdateModifiedCropInfosRoutine { get; set; }


    private enum LogType
    {
        Info,
        Warning,
        Error
    }

    private static void WriteLog(string message, LogType type)
    {
        if (!Plugin.DebugLogging.Value) return;

        switch (type)
        {
            case LogType.Info:
                Plugin.Log.LogInfo(message);
                break;
            case LogType.Warning:
                Plugin.Log.LogWarning(message);
                break;
            case LogType.Error:
                Plugin.Log.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private static void CropDieReplacerScarecrow()
    {
        if (!Plugin.PlantSeedsInAnyFarmType.Value)
        {
            DecorationUpdater.CropDie();
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(DecorationUpdater), nameof(DecorationUpdater.Crop_UpdateMetaOvernight))]
    public static IEnumerable<CodeInstruction> DecorationUpdater_Crop_UpdateMetaOvernight_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var originalCode = instructions.Select(ci => new CodeInstruction(ci)).ToList();
        var codes = originalCode.Select(ci => new CodeInstruction(ci)).ToList();

        try
        {
            for (var index = 0; index < codes.Count; index++)
            {
                var code = codes[index];

                if (code.LoadsField(CropInfosField))
                {
                    code.operand = ModifiedCropInfosField;
                    WriteLog($"Replaced {nameof(CropInfosField)} with {nameof(ModifiedCropInfos)}", LogType.Info);
                }

                if (code.Calls(CropDieMethod) && codes[index - 4].Calls(ScarecrowGetter))
                {
                    code.operand = ReplaceCropDieMethodScarecrow;
                    WriteLog($"Replaced Scarecrow CropDie method with {nameof(ReplaceCropDieMethodScarecrow)}", LogType.Info);
                }
            }

            return codes.AsEnumerable();
        }
        catch (Exception e)
        {
            Plugin.Log.LogError($"Error in {nameof(DecorationUpdater_Crop_UpdateMetaOvernight_Transpiler)}: {e}");
            return originalCode.AsEnumerable();
        }
    }

    // [HarmonyPostfix]
    // [HarmonyPatch(typeof(LocalizeText), nameof(LocalizeText.TranslateText))]
    // public static void LocalizeText_TranslateText(string key, string __result)
    // {
    //     Plugin.Log.LogWarning($"LocalizeText.TranslateText: {key} - {__result}");
    //     // if (name.Contains("Seed", StringComparison.OrdinalIgnoreCase) || name.Contains("Seeds", StringComparison.OrdinalIgnoreCase))
    //     // {
    //     //     description = "THIS IS A TEST!";
    //     //     Plugin.Log.LogWarning($"Tooltip_SetText: {name} - {description}");
    //     // }
    // }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Crop), nameof(Crop.RegrowIfInCorrectSeason))]
    public static void Crop_RegrowIfInCorrectSeason(Crop __instance, ref bool __result)
    {
        ProcessCrop(__instance);
        if (Plugin.PlantSeedsInAnySeason.Value)
        {
            __result = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Seeds), nameof(Seeds.Awake))]
    public static void Seeds_Awake(Seeds __instance)
    {
        ProcessSeed(__instance._seedItem);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIHandler), nameof(UIHandler.ResetEvents))]
    public static void UIHandler_ResetEvents(UIHandler __instance)
    {
        ScenePortalManager.onLoadedWorld += RunUpdaters;
        ScenePortalManager.onFinishLoadingDecorations += RunUpdaters;
    }

    internal static void SetupModifiedData(Scene arg0, LoadSceneMode loadSceneMode)
    {
        if (ModifiedCropInfos.Count > 0)
        {
            WriteLog($"SetupModifiedData: {ModifiedCropInfos.Count} modified crop infos already", LogType.Info);
            return;
        }

        var og = SingletonBehaviour<ItemInfoDatabase>._instance.cropInfos;
        ModifiedCropInfos = og.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Clone()
        );

        WriteLog($"SetupModifiedData: {ModifiedCropInfos.Count} modified crop infos", LogType.Info);

        if (UpdateModifiedCropInfosRoutine != null)
        {
            Plugin.PluginInstance.StopCoroutine(UpdateModifiedCropInfosRoutine);
            UpdateModifiedCropInfosRoutine = null;
        }

        UpdateModifiedCropInfosRoutine = Plugin.PluginInstance.StartCoroutine(UpdateModifiedCropInfos());
    }

    private static IEnumerator UpdateModifiedCropInfos()
    {
        foreach (var cropInfo in ModifiedCropInfos)
        {
            var ogData = GetOriginalSeedData(cropInfo.Key);
            var crop = cropInfo.Value;
            crop.farmType = Plugin.PlantSeedsInAnyFarmType.Value ? FarmType.Any : ogData.farmType;
            crop.seasons = Plugin.PlantSeedsInAnySeason.Value ? AllSeasons : ogData.seasons;
            crop.regrowable = Plugin.EverythingIsRegrowable.Value || ogData.regrowable;

            WriteLog($"UpdateModifiedCropInfos: {cropInfo.Key} with farmType={crop.farmType} ({ogData.farmType}), seasons={string.Join(", ", crop.seasons)} ({string.Join(", ", ogData.seasons)}), regrowable={crop.regrowable} ({ogData.regrowable}))", LogType.Info);
        }

        yield break;
    }

    internal static void RunUpdaters()
    {
        if (UpdateAllSeedsRoutine != null)
        {
            Plugin.PluginInstance.StopCoroutine(UpdateAllSeedsRoutine);
            UpdateAllSeedsRoutine = null;
        }

        if (UpdateAllCropsRoutine != null)
        {
            Plugin.PluginInstance.StopCoroutine(UpdateAllCropsRoutine);
            UpdateAllCropsRoutine = null;
        }

        if (UpdateModifiedCropInfosRoutine != null)
        {
            Plugin.PluginInstance.StopCoroutine(UpdateModifiedCropInfosRoutine);
            UpdateModifiedCropInfosRoutine = null;
        }

        UpdateModifiedCropInfosRoutine = Plugin.PluginInstance.StartCoroutine(UpdateModifiedCropInfos());
        UpdateAllSeedsRoutine = Plugin.PluginInstance.StartCoroutine(UpdateAllSeeds());
        UpdateAllCropsRoutine = Plugin.PluginInstance.StartCoroutine(UpdateAllCrops());
    }


    private static CropInfo GetOriginalSeedData(int id)
    {
        return SingletonBehaviour<ItemInfoDatabase>.Instance.cropInfos.FirstOrDefault(a => a.Key == id).Value;
    }

    private static IEnumerator UpdateAllSeeds()
    {
        var resources = Resources.FindObjectsOfTypeAll<SeedData>().ToList();
        foreach (var seed in resources)
        {
            ProcessSeed(seed);
        }

        yield break;
    }

    private static IEnumerator UpdateAllCrops()
    {
        var crops = Resources.FindObjectsOfTypeAll<Crop>().ToList();
        foreach (var crop in crops)
        {
            ProcessCrop(crop);
        }

        yield break;
    }

    private static void ProcessCrop(Crop crop)
    {
        ProcessSeed(crop._seedItem);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Crop), nameof(Crop.ReceiveDamage))]
    public static void Crop_ReceiveDamage(Crop __instance)
    {
        ProcessSeed(__instance._seedItem);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Crop), nameof(Crop.CanBePlacedBecauseScarecrowNearby))]
    public static void Crop_CanBePlacedBecauseScarecrowNearby(Crop __instance)
    {
        ProcessSeed(__instance._seedItem);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Shop), nameof(Shop.SetStartingItems))]
    public static void Shop_SetStartingItems(Shop __instance, ref MerchantTable table)
    {
        if (!Plugin.AddOutOfSeasonSeedsToFarmingShop.Value || __instance.seasonalMerchantTables == null) return;

        var season = SingletonBehaviour<DayCycle>.Instance.Season;

        foreach (var t in __instance.seasonalMerchantTables.Where(t => t.Key != season))
        {
            table.randomShopItems.AddRange(t.Value.randomShopItems);
            table.randomShopItems = table.randomShopItems.Distinct().ToList();

            table.randomShopItems2.AddRange(t.Value.randomShopItems2);
            table.randomShopItems2 = table.randomShopItems2.Distinct().ToList();

            table.startingItems.AddRange(t.Value.startingItems);
            table.startingItems = table.startingItems.Distinct().ToList();

            table.startingItems2.AddRange(t.Value.startingItems2);
            table.startingItems2 = table.startingItems2.Distinct().ToList();
        }

        //fix the viewport height to fit all items
        var newCount = table.randomShopItems.Count + table.randomShopItems2.Count + table.startingItems.Count + table.startingItems2.Count + SingletonBehaviour<SaleManager>.Instance.merchantItems[__instance.merchantTable].Where(list => list is { Count: > 0 }).Sum(list => list.Count);

        var component = __instance._shopPanel.GetComponent<RectTransform>();
        if (__instance.bernardShop)
        {
            newCount += 21;
        }

        var newSizeY = (newCount + 1f) * (62 + 8);
        component.sizeDelta = new Vector2(component.sizeDelta.x, newSizeY);
    }

    private static void ProcessSeed(SeedData data)
    {
        var ogData = GetOriginalSeedData(data.id);

        data.farmType = Plugin.PlantSeedsInAnyFarmType.Value ? FarmType.Any : ogData.farmType;
        data.seasons = Plugin.PlantSeedsInAnySeason.Value ? AllSeasons : ogData.seasons;
        data.regrowable = Plugin.EverythingIsRegrowable.Value || ogData.regrowable;

        WriteLog($"ProcessSeed (Seed): {data.name} with farmType={data.farmType} ({ogData.farmType}), seasons={string.Join(", ", data.seasons)} ({string.Join(", ", ogData.seasons)}), regrowable={data.regrowable} ({ogData.regrowable}))", LogType.Info);
    }
}