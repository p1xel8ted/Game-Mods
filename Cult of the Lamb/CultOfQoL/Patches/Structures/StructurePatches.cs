using CultOfQoL.Core;

namespace CultOfQoL.Patches.Structures;

[Harmony]
internal static class StructurePatches
{
    // Performance optimization: Cache Unity component lookups
    private static PropagandaSpeaker[] _cachedPropagandaSpeakers;
    private static float _lastSpeakerCacheTime;
    private const float SpeakerCacheValidityDuration = 5f; // Cache speakers for 5 seconds

    // Spider web production tracking
    private static readonly Dictionary<LumberjackStation, int> LOGCollectionCounts = new();

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.LifeSpawn), MethodType.Getter)]
    public static void Structures_LumberjackStation_LifeSpawn(Structures_LumberjackStation __instance, ref int __result)
    {
        if (Plugin.LumberAndMiningStationsDontAge.Value)
        {
            return;
        }

        if (!Mathf.Approximately(Plugin.LumberAndMiningStationsAgeMultiplier.Value, 1.0f))
        {
            __result = Mathf.CeilToInt(__result * Mathf.Abs(Plugin.LumberAndMiningStationsAgeMultiplier.Value));
            Plugin.WriteLog($"[Structures_LumberjackStation_LifeSpawn] Adjusted life spawn to: {__result}");
        }
    }


    [HarmonyPatch(typeof(PropagandaSpeaker), nameof(PropagandaSpeaker.Update))]
    public static class PropagandaSpeakerPatches
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if (!ConfigCache.GetCachedValue(ConfigCache.Keys.TurnOffSpeakersAtNight, () => Plugin.TurnOffSpeakersAtNight.Value)) return true;
            return !TimeManager.IsNight;
        }


        [HarmonyPostfix]
        public static void Postfix(ref PropagandaSpeaker __instance)
        {
            if (!ConfigCache.GetCachedValue(ConfigCache.Keys.DisablePropagandaSpeakerAudio, () => Plugin.DisablePropagandaSpeakerAudio.Value)) return;
            if (!__instance.VOPlaying) return;
            AudioManager.Instance.StopLoop(__instance.loopedInstance);
            __instance.VOPlaying = false;
        }
    }

    // LumberjackStation spider web production
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.GiveItem))]
    public static void LumberjackStation_GiveItem_Postfix(LumberjackStation __instance, InventoryItem.ITEM_TYPE type)
    {
        // Only process valid resource types (logs and stone)
        if (!IsWoodOrLogType(type)) return;

        // Check separate config values for each feature
        var produceSpiderWebs = ConfigCache.GetCachedValue(ConfigCache.Keys.ProduceSpiderWebsFromLumber, () => Plugin.ProduceSpiderWebsFromLumber.Value);
        var produceCrystalShards = ConfigCache.GetCachedValue(ConfigCache.Keys.ProduceCrystalShardsFromStone, () => Plugin.ProduceCrystalShardsFromStone.Value);

        // Exit early if neither feature is enabled
        if (!produceSpiderWebs && !produceCrystalShards) return;

        switch (type)
        {
            // Skip processing if this specific type isn't enabled
            case InventoryItem.ITEM_TYPE.LOG when !produceSpiderWebs:
            case InventoryItem.ITEM_TYPE.STONE when !produceCrystalShards:
                return;
        }

        // Initialize or get current count for this station
        if (!LOGCollectionCounts.TryGetValue(__instance, out var currentCount))
        {
            currentCount = 0;
        }

        currentCount++;
        LOGCollectionCounts[__instance] = currentCount;

        var spawnPosition = __instance.transform.position + Vector3.up * 0.5f;

        if (type == InventoryItem.ITEM_TYPE.LOG)
        {
            // Check spider web threshold for logs
            var logsPerSpiderWeb = ConfigCache.GetCachedValue(ConfigCache.Keys.SpiderWebsPerLogs, () => Plugin.SpiderWebsPerLogs.Value);
            if (currentCount >= logsPerSpiderWeb)
            {
                LOGCollectionCounts[__instance] = 0;
                InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SPIDER_WEB, 1, spawnPosition, 0f);
                Plugin.WriteLog($"Spawned spider web at lumber station after {logsPerSpiderWeb} logs collected");
            }
        }
        else if (type == InventoryItem.ITEM_TYPE.STONE)
        {
            // Check crystal shard threshold for stone
            var stonePerCrystal = ConfigCache.GetCachedValue(ConfigCache.Keys.CrystalShardsPerStone, () => Plugin.CrystalShardsPerStone.Value);
            if (currentCount >= stonePerCrystal)
            {
                LOGCollectionCounts[__instance] = 0;
                InventoryItem.Spawn(InventoryItem.ITEM_TYPE.CRYSTAL, 1, spawnPosition, 0f);
                Plugin.WriteLog($"Spawned crystal shard at mining station after {stonePerCrystal} stone collected");
            }
        }
    }

    // Cleanup when stations are disabled/removed
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LumberjackStation), nameof(LumberjackStation.OnDisableInteraction))]
    public static void LumberjackStation_OnDisableInteraction_Postfix(LumberjackStation __instance)
    {
        // Clean up tracking dictionary to prevent memory leaks
        LOGCollectionCounts.Remove(__instance);
        Plugin.WriteLog("Removed lumber station from spider web tracking dictionary on disable");
    }

    private static bool IsWoodOrLogType(InventoryItem.ITEM_TYPE type)
    {
        return type switch
        {
            InventoryItem.ITEM_TYPE.LOG => true,
            InventoryItem.ITEM_TYPE.STONE => true, // Include stone for mining stations
            _ => false
        };
    }


    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_FollowerKitchen), nameof(Interaction_FollowerKitchen.MealFinishedCooking))]
    [HarmonyPatch(typeof(Interaction_Kitchen), nameof(Interaction_Kitchen.MealFinishedCooking))]
    [HarmonyPatch(typeof(FollowerTask_Cook), nameof(FollowerTask_Cook.MealFinishedCooking))]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var codeInstructions = instructions.ToList();
        try
        {
            var matcher = new CodeMatcher(codeInstructions);

            matcher.MatchForward(false,
                new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(CookingData), nameof(CookingData.CookedMeal))
                ));

            if (matcher.IsInvalid)
            {
                Plugin.Log.LogWarning($"[Transpiler] {original.DeclaringType?.Name}.{original.Name}: Failed to find CookingData.CookedMeal call.");
                return codeInstructions;
            }

            matcher.Insert(
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StructurePatches), nameof(GetUniversalSpawnPosition),
                    [typeof(object)])),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(StructurePatches), nameof(CheckForBones),
                    [typeof(InventoryItem.ITEM_TYPE), typeof(Vector3)]))
            );

            Plugin.Log.LogInfo($"[Transpiler] {original.DeclaringType?.Name}.{original.Name}: Inserted CheckForBones call before CookedMeal.");
            return matcher.InstructionEnumeration();
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[Transpiler] {original.DeclaringType?.Name}.{original.Name}: {ex.Message}");
            return codeInstructions;
        }
    }


    // Helper method to get spawn position from any of the three types
    // Uses object parameter since FollowerTask_Cook doesn't inherit from Interaction_Kitchen
    public static Vector3 GetUniversalSpawnPosition(object instance)
    {
        if (instance is Interaction_Kitchen kitchen)
        {
            if (kitchen.SpawnMealPosition)
            {
                return kitchen.SpawnMealPosition.position;
            }
            return kitchen.transform.position;
        }

        if (instance is FollowerTask_Cook cookTask)
        {
            var kitchenInteraction = cookTask.FindKitchen();
            if (kitchenInteraction && kitchenInteraction.SpawnMealPosition)
            {
                return kitchenInteraction.SpawnMealPosition.position;
            }

            if (cookTask.kitchenStructure is { Data: not null })
            {
                return cookTask.kitchenStructure.Data.Position;
            }
        }

        Plugin.Log.LogWarning($"Could not determine spawn position for {instance?.GetType().Name ?? "null"}");
        return Vector3.positiveInfinity;
    }

    public static void CheckForBones(InventoryItem.ITEM_TYPE mealType, Vector3 position)
    {
        if (!Plugin.CookedMeatMealsContainBone.Value) return;
        if (float.IsPositiveInfinity(position.x)) return;

        var mixedMeal = mealType.IsMixedMeal();
        var meatMeal = mealType.IsMealWithMeat();
    
        var spawnAmount = Random.Range(1, 4);
        if (mixedMeal)
        {
            spawnAmount = Mathf.Max(1, Mathf.RoundToInt((float)spawnAmount / 2));
            var offsetPosition = position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, spawnAmount, offsetPosition, Random.Range(9, 11));
        }
        else if (meatMeal)
        {
            var offsetPosition = position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, spawnAmount, offsetPosition, Random.Range(9, 11));
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(Structures_OfferingShrine), nameof(Structures_OfferingShrine.Complete))]
    public static void Structures_OfferingShrine_Complete(Structures_OfferingShrine __instance)
    {
        // Performance optimization: Use cached config values
        if (ConfigCache.GetCachedValue(ConfigCache.Keys.AddSpiderWebsToOfferings, () => Plugin.AddSpiderWebsToOfferings.Value))
        {
            if (!__instance.Offerings.Contains(InventoryItem.ITEM_TYPE.SPIDER_WEB))
            {
                __instance.Offerings.Add(InventoryItem.ITEM_TYPE.SPIDER_WEB);
            }
        }
        else
        {
            if (__instance.Offerings.Contains(InventoryItem.ITEM_TYPE.SPIDER_WEB))
            {
                __instance.Offerings.Remove(InventoryItem.ITEM_TYPE.SPIDER_WEB);
            }
        }

        if (ConfigCache.GetCachedValue(ConfigCache.Keys.AddCrystalShardsToOfferings, () => Plugin.AddCrystalShardsToOfferings.Value))
        {
            if (!__instance.Offerings.Contains(InventoryItem.ITEM_TYPE.CRYSTAL))
            {
                __instance.Offerings.Add(InventoryItem.ITEM_TYPE.CRYSTAL);
            }
        }
        else
        {
            if (__instance.Offerings.Contains(InventoryItem.ITEM_TYPE.CRYSTAL))
            {
                __instance.Offerings.Remove(InventoryItem.ITEM_TYPE.CRYSTAL);
            }
        }
    }

//stop fuel being taken when speakers are off
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Structures_PropagandaSpeaker), nameof(Structures_PropagandaSpeaker.OnNewPhaseStarted))]
    public static bool Structures_PropagandaSpeaker_OnNewPhaseStarted()
    {
        if (!Plugin.TurnOffSpeakersAtNight.Value) return true;
        return !TimeManager.IsNight;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TimeManager), nameof(TimeManager.StartNewPhase))]
    public static void TimeManager_StartNewPhase(DayPhase phase)
    {
        // Performance optimization: Use cached config value
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.TurnOffSpeakersAtNight, () => Plugin.TurnOffSpeakersAtNight.Value)) return;

        if (phase != DayPhase.Night) return;

        // Performance optimization: Cache expensive Unity component lookup
        var currentTime = Time.time;
        if (_cachedPropagandaSpeakers == null || currentTime - _lastSpeakerCacheTime > SpeakerCacheValidityDuration)
        {
            _cachedPropagandaSpeakers = Object.FindObjectsOfType<PropagandaSpeaker>();
            _lastSpeakerCacheTime = currentTime;
        }

        foreach (var structure in _cachedPropagandaSpeakers)
        {
            if (!structure) continue; // Handle destroyed objects
            structure.StopAllCoroutines();
            structure.Spine.AnimationState.SetAnimation(0, "off", true);
            var fireOff = structure.onFireOff;
            fireOff?.Invoke();
            AudioManager.Instance.StopLoop(structure.loopedInstance);
            structure.VOPlaying = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_LumberjackStation), nameof(Structures_LumberjackStation.IncreaseAge))]
    public static void Structures_LumberjackStation_IncreaseAge(ref Structures_LumberjackStation __instance)
    {
        // Performance optimization: Use cached config value
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.LumberAndMiningStationsDontAge, () => Plugin.LumberAndMiningStationsDontAge.Value)) return;

        __instance.Data.Age = 0;
        Plugin.WriteLog("Resetting age of lumber/mining station to 0!");
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(StructureBrain), nameof(StructureBrain.SoulMax), MethodType.Getter)]
    public static void Structures_Bed_Constructor(StructureBrain __instance, ref int __result)
    {
        if (__instance is not Structures_Bed) return;

        __result = AdjustSoulMax(__result);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine), nameof(Structures_Shrine.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_SoulMax(ref int __result)
    {
        __result = AdjustSoulMax(__result);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Misfit), nameof(Structures_Shrine_Misfit.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Misfit_SoulMax(ref int __result)
    {
        __result = AdjustSoulMax(__result);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Ratau), nameof(Structures_Shrine_Ratau.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Ratau_SoulMax(ref int __result)
    {
        __result = AdjustSoulMax(__result);
    }

    private static int AdjustSoulMax(int original)
    {
        // Performance optimization: Use cached config value and avoid repeated calculations
        var multiplier = ConfigCache.GetCachedValue(ConfigCache.Keys.SoulCapacityMulti, () => Plugin.SoulCapacityMulti.Value);
        return !Mathf.Approximately(multiplier, 1.0f) ? Mathf.CeilToInt(original * Mathf.Abs(multiplier)) : original;
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Shrine_Passive), nameof(Structures_Shrine_Passive.SoulMax), MethodType.Getter)]
    public static void Structures_Shrine_Passive_SoulMax(ref int __result)
    {
        __result = AdjustSoulMax(__result);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_SiloSeed), nameof(Structures_SiloSeed.Capacity), MethodType.Getter)]
    [HarmonyPatch(typeof(Structures_SiloFertiliser), nameof(Structures_SiloFertiliser.Capacity), MethodType.Getter)]
    public static void Structures_Capacity(ref StructureBrain __instance, ref float __result)
    {
        // Performance optimization: Use cached config values
        __result = MakeMultipleOf32(__result);

        var siloMultiplier = ConfigCache.GetCachedValue(ConfigCache.Keys.SiloCapacityMulti, () => Plugin.SiloCapacityMulti.Value);
        if (!Mathf.Approximately(siloMultiplier, 1.0f))
        {
            __result = MakeMultipleOf32(Mathf.Ceil(__result * Mathf.Abs(siloMultiplier)));
        }
    }

    private static float MakeMultipleOf32(float value)
    {
        // Performance optimization: Use cached config value
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.UseMultiplesOf32, () => Plugin.UseMultiplesOf32.Value))
        {
            return value;
        }

        return Mathf.Ceil(value / 32f) * 32f;
    }

    /// <summary>
    /// Excludes grass from the "Deposit All Seeds" action on seed silos.
    /// Grass is included in InventoryItem.AllSeeds, but players often want to keep it
    /// separate since it's commonly obtained and can overwhelm crop seed storage.
    ///
    /// This transpiler replaces calls to InventoryItem.AllSeeds with our filtered version
    /// that optionally excludes grass based on the config setting.
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(Interaction_SiloSeeder), nameof(Interaction_SiloSeeder.OnSecondaryInteract))]
    public static IEnumerable<CodeInstruction> Interaction_SiloSeeder_OnSecondaryInteract_Transpiler(
        IEnumerable<CodeInstruction> instructions)
    {
        var allSeedsGetter = AccessTools.PropertyGetter(typeof(InventoryItem), nameof(InventoryItem.AllSeeds));
        var filteredSeedsMethod = AccessTools.Method(typeof(StructurePatches), nameof(GetFilteredSeeds));

        foreach (var instruction in instructions)
        {
            if (instruction.Calls(allSeedsGetter))
            {
                // Replace InventoryItem.AllSeeds with our filtered version
                yield return new CodeInstruction(OpCodes.Call, filteredSeedsMethod);
            }
            else
            {
                yield return instruction;
            }
        }
    }

    /// <summary>
    /// Returns AllSeeds list, optionally filtered to exclude grass.
    /// </summary>
    public static List<InventoryItem.ITEM_TYPE> GetFilteredSeeds()
    {
        var seeds = InventoryItem.AllSeeds;
        if (!Plugin.ExcludeGrassFromSeedDeposit.Value)
        {
            return seeds;
        }

        seeds.Remove(InventoryItem.ITEM_TYPE.GRASS);
        return seeds;
    }

    /// <summary>
    /// Defensive fix for vanilla bug: QueuedRefineryVariants can desync from QueuedResources,
    /// causing ArgumentOutOfRangeException in RefineryDeposit. This ensures they stay in sync.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.RefineryDeposit))]
    public static void Structures_Refinery_RefineryDeposit_Prefix(Structures_Refinery __instance)
    {
        // Ensure QueuedRefineryVariants has at least as many elements as QueuedResources
        while (__instance.Data.QueuedRefineryVariants.Count < __instance.Data.QueuedResources.Count)
        {
            __instance.Data.QueuedRefineryVariants.Add(0);
            Plugin.WriteLog("[Refinery] Added missing variant entry to prevent desync crash");
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Structures_Refinery), nameof(Structures_Refinery.GetCost), typeof(InventoryItem.ITEM_TYPE), typeof(int))]
    public static void Structures_Refinery_GetCost(ref List<StructuresData.ItemCost> __result)
    {
        // Performance optimization: Use cached config value
        if (!ConfigCache.GetCachedValue(ConfigCache.Keys.AdjustRefineryRequirements, () => Plugin.AdjustRefineryRequirements.Value)) return;
        foreach (var item in __result)
        {
            item.CostValue = Mathf.CeilToInt(item.CostValue / 2f);
        }
    }

    // Flag to prevent infinite recursion when mass filling refinery
    private static bool _refineryMassFillInProgress;

    // Flag to prevent infinite recursion when mass petting animals
    private static bool _massPetAnimalsInProgress;

    // Flags to prevent infinite recursion when mass filling cooking stations
    private static bool _cookingFireMassFillInProgress;
    private static bool _kitchenMassFillInProgress;
    private static bool _pubMassFillInProgress;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Interaction_Ranchable), nameof(Interaction_Ranchable.OnAnimalCommandFinalized))]
    public static void Interaction_Ranchable_OnAnimalCommandFinalized(
        ref Interaction_Ranchable __instance,
        params FollowerCommands[] followerCommands)
    {
        if (!Plugin.MassPetAnimals.Value) return;
        if (_massPetAnimalsInProgress) return;
        if (followerCommands.Length == 0 || followerCommands[0] != FollowerCommands.PetAnimal) return;

        GameManager.GetInstance().StartCoroutine(PetAllAnimals(__instance));
    }

    private static IEnumerator PetAllAnimals(Interaction_Ranchable pettedAnimal)
    {
        _massPetAnimalsInProgress = true;
        yield return new WaitForEndOfFrame();

        try
        {
            // Get all ranchable animals that haven't been petted today
            var animals = Interaction.interactions
                .OfType<Interaction_Ranchable>()
                .Where(r => r && r != pettedAnimal && !r.animal.PetToday)
                .ToList();

            Plugin.WriteLog($"[MassPetAnimals] Petting {animals.Count} additional animals");

            foreach (var animal in animals)
            {
                if (!animal || animal.animal.PetToday) continue;

                // Apply pet effects directly instead of calling PetIE
                // This avoids the coroutine's player-locking animation and long delays
                animal.AddAdoration(50f);
                animal.animal.PetToday = true;

                // Visual and audio feedback
                BiomeConstants.Instance.EmitHeartPickUpVFX(animal.transform.position, 0f, "red", "burst_small");
                AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", animal.transform.position);
            }
        }
        finally
        {
            _massPetAnimalsInProgress = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIRefineryMenuController), nameof(UIRefineryMenuController.AddToQueue))]
    public static void UIRefineryMenuController_AddToQueue(UIRefineryMenuController __instance, RefineryItem item)
    {
        if (!Plugin.RefineryMassFill.Value) return;
        if (_refineryMassFillInProgress) return;

        _refineryMassFillInProgress = true;
        try
        {
            var maxItems = __instance.kMaxItems;
            var currentCount = __instance._structureInfo.QueuedResources.Count;

            // Keep adding same item until queue is full or can't afford
            while (currentCount < maxItems && item.CanAfford)
            {
                __instance.AddToQueue(item);
                currentCount = __instance._structureInfo.QueuedResources.Count;
            }

            Plugin.WriteLog($"[RefineryMassFill] Filled queue with {item.Type} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _refineryMassFillInProgress = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIFollowerKitchenMenuController), nameof(UIFollowerKitchenMenuController.AddToQueue))]
    public static void UIFollowerKitchenMenuController_AddToQueue(UIFollowerKitchenMenuController __instance, InventoryItem.ITEM_TYPE meal)
    {
        if (!Plugin.KitchenMassFill.Value) return;
        if (_kitchenMassFillInProgress) return;

        _kitchenMassFillInProgress = true;
        try
        {
            var maxItems = UIFollowerKitchenMenuController.kMaxItems;
            var currentCount = __instance._structureInfo.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(meal))
            {
                __instance.AddToQueue(meal);
                currentCount = __instance._structureInfo.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[KitchenMassFill] Filled queue with {meal} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _kitchenMassFillInProgress = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIPubMenuController), nameof(UIPubMenuController.AddToQueue))]
    public static void UIPubMenuController_AddToQueue(UIPubMenuController __instance, InventoryItem.ITEM_TYPE meal)
    {
        if (!Plugin.PubMassFill.Value) return;
        if (_pubMassFillInProgress) return;

        _pubMassFillInProgress = true;
        try
        {
            var maxItems = __instance._pub.Brain.MaxQueue;
            var currentCount = __instance._structureInfo.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(meal))
            {
                __instance.AddToQueue(meal);
                currentCount = __instance._structureInfo.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[PubMassFill] Filled queue with {meal} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _pubMassFillInProgress = false;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UICookingFireMenuController), nameof(UICookingFireMenuController.OnRecipeChosen))]
    public static void UICookingFireMenuController_OnRecipeChosen(UICookingFireMenuController __instance, InventoryItem.ITEM_TYPE recipe)
    {
        if (!Plugin.CookingFireMassFill.Value) return;
        if (_cookingFireMassFillInProgress) return;

        _cookingFireMassFillInProgress = true;
        try
        {
            var maxItems = __instance.RecipeLimit();
            var currentCount = __instance._kitchenData.QueuedMeals.Count;

            while (currentCount < maxItems && CookingData.CanMakeMeal(recipe))
            {
                __instance.OnRecipeChosen(recipe);
                currentCount = __instance._kitchenData.QueuedMeals.Count;
            }

            Plugin.WriteLog($"[CookingFireMassFill] Filled queue with {recipe} - {currentCount}/{maxItems} slots used");
        }
        finally
        {
            _cookingFireMassFillInProgress = false;
        }
    }

    // TODO: Re-enable after testing
    /*
    #region Mass Nurture

    [HarmonyPostfix]
    [HarmonyPatch(typeof(UIDaycareMenu), nameof(UIDaycareMenu.OnNurturePressed))]
    public static void UIDaycareMenu_OnNurturePressed_Postfix(UIDaycareMenu __instance)
    {
        if (!Plugin.MassNurture.Value) return;



        var sourceDaycare = __instance._structuresBrain;

        foreach (var daycare in StructureManager.GetAllStructuresOfType<Structures_Daycare>())
        {
            if (daycare == sourceDaycare) continue;

            foreach (var childId in daycare.Data.MultipleFollowerIDs.ToList())
            {
                var follower = FollowerManager.FindFollowerByID(childId);
                if (follower == null) continue;
                if (follower.Brain.Stats.Cuddled) continue;
                if (!FollowerManager.IsChild(childId)) continue;

                // Apply nurture effects (same as NurtureIE lines 183-190)
                CultFaithManager.AddThought(Thought.ChildCuddle_0, follower.Brain.Info.ID);
                follower.Brain.AddThought((Thought)Random.Range(393, 397));
                AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", follower.transform.position);
                BiomeConstants.Instance.EmitHeartPickUpVFX(follower.transform.position, 0f, "red", "burst_big");
                follower.Brain.Stats.Cuddled = true;
                follower.Brain._directInfoAccess.CuddledAmount++;
                follower.Brain.AddAdoration(FollowerBrain.AdorationActions.CuddleBaby, null);

                Plugin.WriteLog($"[MassNurture] Nurtured {follower.Brain.Info.Name} in another daycare!");
            }
        }
    }

    #endregion
    */
}