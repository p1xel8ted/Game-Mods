namespace TraitControl.Patches;

/// <summary>
/// Applies weighted random selection to starting trait generation.
/// Allows users to configure how often each trait appears, including disabling traits entirely.
/// </summary>
[Harmony]
public static class TraitWeights
{
    /// <summary>
    /// Tracks whether the guaranteed trait has already been given during the current follower's creation.
    /// Reset by the FollowerBrain constructor prefix before trait assignment begins.
    /// </summary>
    private static bool _guaranteedTraitGivenThisSession;

    /// <summary>
    /// Resets the guarantee flag before trait assignment begins for a new follower.
    /// This ensures the guaranteed trait is only given once per follower, not on every trait roll.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerBrain), MethodType.Constructor, typeof(FollowerInfo))]
    private static void FollowerBrain_Constructor_Prefix()
    {
        _guaranteedTraitGivenThisSession = false;
    }

    /// <summary>
    /// Refreshes AllTraitsList when a save is loaded to pick up CultTraits changes.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BiomeBaseManager), nameof(BiomeBaseManager.Start))]
    private static void BiomeBaseManager_Start()
    {
        RefreshAllTraitsList();
        Plugin.RefreshTraitWeightVisibility();
    }

    /// <summary>
    /// Refreshes trait lists when a cult trait is added (via doctrines, trait manipulator, etc.).
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.AddCultTrait))]
    private static void AddCultTrait_Postfix()
    {
        RefreshAllTraitsList();
        NoNegativeTraits.GenerateAvailableTraits();
        Plugin.RefreshTraitWeightVisibility();
    }

    /// <summary>
    /// Refreshes the AllTraitsList used for trait selection.
    /// Called when save loads or relevant configs change.
    /// </summary>
    internal static void RefreshAllTraitsList()
    {
        Plugin.AllTraitsList.Clear();
        var allTraits = Enum.GetValues(typeof(FollowerTrait.TraitType))
            .Cast<FollowerTrait.TraitType>()
            .Where(t => t != FollowerTrait.TraitType.None &&
                        t != FollowerTrait.TraitType.Spy &&
                        t != FollowerTrait.TraitType.BishopOfCult)
            .ToList();
        IEnumerable<FollowerTrait.TraitType> traitsForPatches = allTraits;
        if (!Plugin.IncludeStoryEventTraits.Value)
        {
            traitsForPatches = traitsForPatches.Where(t => !Plugin.StoryEventTraits.Contains(t));
        }
        Plugin.AllTraitsList.AddRange(traitsForPatches);
        Plugin.Log.LogInfo($"[RefreshAllTraitsList] Refreshed AllTraitsList with {Plugin.AllTraitsList.Count} traits");
    }

    /// <summary>
    /// Adds extra traits after vanilla assignment if min/max trait count is configured higher.
    /// Vanilla assigns 2-3 traits; this postfix adds more to meet the configured minimum/maximum.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerBrain), MethodType.Constructor, typeof(FollowerInfo))]
    private static void FollowerBrain_Constructor_Postfix(FollowerBrain __instance)
    {
        var min = Plugin.MinimumTraits.Value;
        var max = Plugin.MaximumTraits.Value;

        Plugin.Log.LogInfo($"[FollowerBrain Constructor] Follower: {__instance.Info.Name}");
        Plugin.Log.LogInfo($"[FollowerBrain Constructor] Config: min={min}, max={max}");
        Plugin.Log.LogInfo($"[FollowerBrain Constructor] Initial traits ({__instance.Info.Traits.Count}): {string.Join(", ", __instance.Info.Traits)}");

        // Skip if vanilla behavior (2-3 traits)
        if (min <= 2 && max <= 3)
        {
            Plugin.Log.LogInfo("[FollowerBrain Constructor] Using vanilla behavior (min<=2 && max<=3), skipping extra traits");
            return;
        }

        var currentCount = __instance.Info.Traits.Count;
        var targetCount = Random.Range(min, max + 1);

        Plugin.Log.LogInfo($"[FollowerBrain Constructor] Target: {targetCount} traits (random between {min}-{max})");

        // Already have enough traits
        if (currentCount >= targetCount)
        {
            Plugin.Log.LogInfo($"[FollowerBrain Constructor] Already has {currentCount} traits >= target {targetCount}, skipping");
            return;
        }

        var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.StartingTraits;
        var attempts = 0;

        while (currentCount < targetCount && attempts < 100)
        {
            attempts++;
            var trait = SelectTrait(sourceTraits);

            if (trait == FollowerTrait.TraitType.None)
            {
                Plugin.Log.LogInfo($"[FollowerBrain Constructor] SelectTrait returned None, stopping");
                break; // No more valid traits available
            }

            if (!__instance.HasTrait(trait))
            {
                __instance.Info.Traits.Add(trait);
                currentCount++;
                Plugin.Log.LogInfo($"[FollowerBrain Constructor] Added trait {trait} (now {currentCount}/{targetCount})");
            }
        }

        if (currentCount < targetCount)
        {
            Plugin.Log.LogWarning($"[FollowerBrain Constructor] Could only assign {currentCount}/{targetCount} traits. Not enough unlocked/available traits to meet minimum.");
        }

        Plugin.Log.LogInfo($"[FollowerBrain Constructor] Final traits ({__instance.Info.Traits.Count}): {string.Join(", ", __instance.Info.Traits)}");
    }

    /// <summary>
    /// Prefix patch for GetStartingTrait. Always runs our selection logic to ensure
    /// unique trait toggles work regardless of whether weighting is enabled.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetStartingTrait))]
    public static bool GetStartingTrait_Prefix(ref FollowerTrait.TraitType __result)
    {
        var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.StartingTraits;
        __result = SelectTrait(sourceTraits);
        return false; // Always skip original
    }

    /// <summary>
    /// Prefix patch for GetRareTrait. Always runs our selection logic to ensure
    /// unique trait toggles work regardless of whether weighting is enabled.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetRareTrait))]
    public static bool GetRareTrait_Prefix(ref FollowerTrait.TraitType __result)
    {
        var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.RareStartingTraits;
        __result = SelectTrait(sourceTraits);
        return false; // Always skip original
    }

    /// <summary>
    /// Patch class for SetFaithful lambda - uses TargetMethod to find compiler-generated method.
    /// </summary>
    [HarmonyPatch]
    public static class SetFaithfulPatch
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var method = AccessTools.GetDeclaredMethods(typeof(Interaction_FollowerInSpiderWeb))
                .FirstOrDefault(m => m.Name.Contains("SetFaithful") && m.Name.Contains("b__"));

            if (method == null)
            {
                Plugin.Log.LogWarning("[Transpiler] Interaction_FollowerInSpiderWeb: Could not find SetFaithful lambda method.");
            }

            return method;
        }

        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        {
            return PatchFaithfulTraitsSelection(instructions, original?.Name ?? "unknown");
        }
    }

    /// <summary>
    /// Replaces FaithfulTraits[Random.Range(0, Count)] with GetWeightedFaithfulTrait().
    /// </summary>
    private static IEnumerable<CodeInstruction> PatchFaithfulTraitsSelection(IEnumerable<CodeInstruction> instructions, string methodName)
    {
        var codes = new List<CodeInstruction>(instructions);
        var faithfulTraitsField = AccessTools.Field(typeof(FollowerTrait), nameof(FollowerTrait.FaithfulTraits));
        var randomRangeMethod = AccessTools.Method(typeof(Random), nameof(Random.Range), [typeof(int), typeof(int)]);
        var getWeightedMethod = AccessTools.Method(typeof(TraitWeights), nameof(GetWeightedFaithfulTrait));
        var patched = false;

        for (var i = 0; i < codes.Count - 5; i++)
        {
            // Look for pattern: ldsfld FaithfulTraits, ldc.i4.0, ldsfld FaithfulTraits, callvirt get_Count, call Random.Range, callvirt get_Item
            if (codes[i].LoadsField(faithfulTraitsField) &&
                codes[i + 1].opcode == OpCodes.Ldc_I4_0 &&
                codes[i + 2].LoadsField(faithfulTraitsField) &&
                codes[i + 4].Calls(randomRangeMethod))
            {
                // Replace the entire sequence with a call to GetWeightedFaithfulTrait
                var labels = codes[i].labels;
                codes[i] = new CodeInstruction(OpCodes.Call, getWeightedMethod).WithLabels(labels);

                // NOP out the rest of the pattern (indices 1-5: ldc.i4.0, ldsfld, callvirt Count, call Range, callvirt Item)
                for (var j = 1; j <= 5; j++)
                {
                    codes[i + j].opcode = OpCodes.Nop;
                    codes[i + j].operand = null;
                }

                patched = true;
                break;
            }
        }

        if (!patched)
        {
            Plugin.Log.LogWarning($"[Transpiler] Interaction_FollowerInSpiderWeb.{methodName}: Failed to find FaithfulTraits selection pattern.");
        }
        else
        {
            Plugin.Log.LogInfo($"[Transpiler] Interaction_FollowerInSpiderWeb.{methodName}: Replaced FaithfulTraits selection with weighted selection.");
        }

        return codes;
    }

    /// <summary>
    /// Gets a trait from FaithfulTraits (or all traits if UseAllTraits is enabled).
    /// Uses SelectTrait to ensure unique trait toggles work regardless of weighting.
    /// </summary>
    public static FollowerTrait.TraitType GetWeightedFaithfulTrait()
    {
        var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.FaithfulTraits;
        return SelectTrait(sourceTraits);
    }

    /// <summary>
    /// Selects a trait using either weighted or random selection based on config.
    /// Filtering (unique toggles, single-use, game restrictions) always applies.
    /// Guaranteed traits take priority over all other selection methods, but only once per follower.
    /// </summary>
    private static FollowerTrait.TraitType SelectTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        // Check for guaranteed traits first (bypasses all other selection)
        // Only give the guaranteed trait once per follower - subsequent trait rolls use normal selection
        if (!_guaranteedTraitGivenThisSession)
        {
            var guaranteed = GetGuaranteedTrait();
            if (guaranteed != FollowerTrait.TraitType.None)
            {
                _guaranteedTraitGivenThisSession = true;
                return guaranteed;
            }
        }

        // Debug: Log source traits count
        var sourceList = sourceTraits.ToList();
        Plugin.Log.LogInfo($"[SelectTrait] Source traits count: {sourceList.Count}, UseAllTraits: {Plugin.UseAllTraits.Value}");

        if (Plugin.EnableTraitWeights.Value)
        {
            return GetWeightedTrait(sourceList);
        }

        return GetRandomTrait(sourceList);
    }

    /// <summary>
    /// Checks if any guaranteed trait is enabled and available.
    /// Returns the first available guaranteed trait, or None if none are available.
    /// </summary>
    private static FollowerTrait.TraitType GetGuaranteedTrait()
    {
        // Check each guaranteed trait in order
        if (Plugin.GuaranteeImmortal.Value && Plugin.IncludeImmortal.Value)
        {
            if (IsTraitAvailable(FollowerTrait.TraitType.Immortal))
            {
                return FollowerTrait.TraitType.Immortal;
            }
        }

        if (Plugin.GuaranteeDisciple.Value && Plugin.IncludeDisciple.Value)
        {
            if (IsTraitAvailable(FollowerTrait.TraitType.Disciple))
            {
                return FollowerTrait.TraitType.Disciple;
            }
        }

        if (Plugin.GuaranteeDontStarve.Value && Plugin.IncludeDontStarve.Value)
        {
            if (IsTraitAvailable(FollowerTrait.TraitType.DontStarve))
            {
                return FollowerTrait.TraitType.DontStarve;
            }
        }

        if (Plugin.GuaranteeBlind.Value && Plugin.IncludeBlind.Value)
        {
            if (IsTraitAvailable(FollowerTrait.TraitType.Blind))
            {
                return FollowerTrait.TraitType.Blind;
            }
        }

        if (Plugin.GuaranteeBornToTheRot.Value && Plugin.IncludeBornToTheRot.Value)
        {
            if (IsTraitAvailable(FollowerTrait.TraitType.BornToTheRot))
            {
                return FollowerTrait.TraitType.BornToTheRot;
            }
        }

        return FollowerTrait.TraitType.None;
    }

    /// <summary>
    /// Checks if a trait is available (not already in use if single-use, not unavailable by game rules).
    /// </summary>
    private static bool IsTraitAvailable(FollowerTrait.TraitType trait)
    {
        // Check single-use restrictions (SingleTraits and UniqueTraits)
        // Skip this check if AllowMultipleUniqueTraits is enabled
        if (!Plugin.AllowMultipleUniqueTraits.Value)
        {
            if (FollowerTrait.SingleTraits.Contains(trait) || FollowerTrait.UniqueTraits.Contains(trait))
            {
                if (FollowerBrain.AllBrains.Any(b => b.HasTrait(trait)))
                {
                    return false;
                }
            }
        }

        // Check game restrictions (DLC requirements, day requirements, etc.)
        if (FollowerTrait.IsTraitUnavailable(trait))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Selects a trait using random selection (no weighting).
    /// Still applies all filtering: unique toggles, single-use, game restrictions.
    /// </summary>
    private static FollowerTrait.TraitType GetRandomTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        var availableTraits = GetFilteredTraits(sourceTraits);

        // Try to find a valid trait with random selection
        var attempts = 0;
        while (++attempts < 100 && availableTraits.Count > 0)
        {
            var trait = availableTraits[Random.Range(0, availableTraits.Count)];

            // Check game restrictions (DLC requirements, day requirements, etc.)
            if (!FollowerTrait.IsTraitUnavailable(trait))
            {
                return trait;
            }
        }

        return FollowerTrait.TraitType.None;
    }

    /// <summary>
    /// Selects a trait from the given list using weighted random selection.
    /// Respects game rules for unavailable traits and single-use traits.
    /// </summary>
    private static FollowerTrait.TraitType GetWeightedTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        var availableTraits = GetFilteredTraits(sourceTraits);

        // Try to find a valid trait with weighted selection
        var attempts = 0;
        while (++attempts < 100)
        {
            var trait = SelectWeightedTrait(availableTraits);
            if (trait == FollowerTrait.TraitType.None)
            {
                break; // No valid traits available
            }

            // Check game restrictions (DLC requirements, day requirements, etc.)
            if (!FollowerTrait.IsTraitUnavailable(trait))
            {
                return trait;
            }
        }

        return FollowerTrait.TraitType.None;
    }

    /// <summary>
    /// Applies filtering to a trait list: unique traits and single-use trait checks.
    /// Uses section 01 configs (IncludeImmortal, IncludeDisciple) to control unique trait availability.
    /// </summary>
    private static List<FollowerTrait.TraitType> GetFilteredTraits(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        var availableTraits = new List<FollowerTrait.TraitType>(sourceTraits);
        var initialCount = availableTraits.Count;

        // Filter unique traits based on section 01 configs
        if (!Plugin.IncludeImmortal.Value)
        {
            availableTraits.Remove(FollowerTrait.TraitType.Immortal);
        }

        if (!Plugin.IncludeDisciple.Value)
        {
            availableTraits.Remove(FollowerTrait.TraitType.Disciple);
        }

        if (!Plugin.IncludeDontStarve.Value)
        {
            availableTraits.Remove(FollowerTrait.TraitType.DontStarve);
        }

        if (!Plugin.IncludeBlind.Value)
        {
            availableTraits.Remove(FollowerTrait.TraitType.Blind);
        }

        if (!Plugin.IncludeBornToTheRot.Value)
        {
            availableTraits.Remove(FollowerTrait.TraitType.BornToTheRot);
        }

        // Always filter out special traits that require game state setup
        availableTraits.Remove(FollowerTrait.TraitType.BishopOfCult); // Story-related, granted when converting a bishop
        availableTraits.Remove(FollowerTrait.TraitType.Spy); // Requires SpyJoinedDay to be set or spies leave immediately

        // Filter out traits without localization (not fully implemented in the game)
        for (var i = availableTraits.Count - 1; i >= 0; i--)
        {
            var trait = availableTraits[i];
            var localizedName = FollowerTrait.GetLocalizedTitle(trait);
            if (string.IsNullOrEmpty(localizedName) || localizedName.StartsWith("Traits/"))
            {
                availableTraits.RemoveAt(i);
            }
        }

        // Filter out story/event traits
        if (!Plugin.IncludeStoryEventTraits.Value)
        {
            // Setting disabled: remove all event traits
            foreach (var trait in Plugin.StoryEventTraits)
            {
                availableTraits.Remove(trait);
            }
        }
        else if (Plugin.NoNegativeTraits.Value)
        {
            // Event traits enabled but trait replacement also enabled:
            // Remove negative event traits (they'd just get replaced anyway)
            foreach (var trait in Plugin.StoryEventTraits)
            {
                if (!FollowerTrait.IsPositiveTrait(trait))
                {
                    availableTraits.Remove(trait);
                }
            }
        }

        // Filter by game unlock requirements if enabled
        if (Plugin.UseUnlockedTraitsOnly.Value)
        {
            availableTraits.RemoveAll(t => FollowerTrait.IsTraitUnavailable(t));
        }

        // Remove single-use traits that are already in use (unless AllowMultipleUniqueTraits is enabled)
        // This includes both SingleTraits (Lazy, Snorer, etc.) and UniqueTraits (Immortal, Disciple, etc.)
        if (!Plugin.AllowMultipleUniqueTraits.Value)
        {
            for (var i = availableTraits.Count - 1; i >= 0; i--)
            {
                var trait = availableTraits[i];

                foreach (var brain in FollowerBrain.AllBrains)
                {
                    if (brain.HasTrait(trait) &&
                        (FollowerTrait.SingleTraits.Contains(trait) || FollowerTrait.UniqueTraits.Contains(trait)))
                    {
                        availableTraits.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        Plugin.Log.LogInfo($"[GetFilteredTraits] {initialCount} -> {availableTraits.Count} traits after filtering");

        return availableTraits;
    }

    /// <summary>
    /// Performs weighted random selection from available traits.
    /// </summary>
    private static FollowerTrait.TraitType SelectWeightedTrait(List<FollowerTrait.TraitType> traits)
    {
        if (traits.Count == 0)
        {
            return FollowerTrait.TraitType.None;
        }

        // Build weight list
        var weights = new List<float>();
        var validTraits = new List<FollowerTrait.TraitType>();
        var totalWeight = 0f;

        foreach (var trait in traits)
        {
            var weight = GetTraitWeight(trait);
            if (weight > 0f)
            {
                weights.Add(weight);
                validTraits.Add(trait);
                totalWeight += weight;
            }
        }

        if (validTraits.Count == 0 || totalWeight <= 0f)
        {
            return FollowerTrait.TraitType.None;
        }

        // Weighted random selection
        var randomValue = Random.Range(0f, totalWeight);
        var cumulative = 0f;

        for (var i = 0; i < validTraits.Count; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
            {
                return validTraits[i];
            }
        }

        // Fallback (shouldn't reach here)
        return validTraits[validTraits.Count - 1];
    }

    /// <summary>
    /// Gets the configured weight for a trait, defaulting to 1.0 if not configured.
    /// </summary>
    private static float GetTraitWeight(FollowerTrait.TraitType trait)
    {
        if (Plugin.TraitWeights.TryGetValue(trait, out var entry))
        {
            return entry.Value;
        }

        // Default weight for traits not in the config (e.g., dynamically added traits)
        return 1.0f;
    }

    /// <summary>
    /// Transpiler for RandomisedTraits - patches the vanilla formula to use configurable min/max.
    /// Vanilla: max=6, range from (Traits.Count-1) to (Traits.Count+2), clamped to [1, max]
    /// Patched: max=config, range from configMin to configMax+1 (exclusive upper bound)
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.RandomisedTraits))]
    private static IEnumerable<CodeInstruction> RandomisedTraits_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var getMaxMethod = AccessTools.Method(typeof(TraitWeights), nameof(GetMaxTraitCount));
        var getMinMethod = AccessTools.Method(typeof(TraitWeights), nameof(GetMinTraitCount));
        var getMaxPlusOneMethod = AccessTools.Method(typeof(TraitWeights), nameof(GetMaxPlusOneTraitCount));

        var patchedMax = false;
        var patchedMin = false;
        var patchedMaxPlusOne = false;

        for (var i = 0; i < codes.Count; i++)
        {
            // Replace ldc.i4.6 (max = 6)
            if (!patchedMax && codes[i].opcode == OpCodes.Ldc_I4_6)
            {
                codes[i] = new CodeInstruction(OpCodes.Call, getMaxMethod);
                patchedMax = true;
                Plugin.Log.LogInfo("[Transpiler] RandomisedTraits: Replaced max=6 with GetMaxTraitCount()");
            }
            // Replace ldc.i4.1, sub pattern (Traits.Count - 1)
            else if (!patchedMin && codes[i].opcode == OpCodes.Ldc_I4_1 &&
                     i + 1 < codes.Count &&
                     codes[i + 1].opcode == OpCodes.Sub)
            {
                codes[i] = new CodeInstruction(OpCodes.Pop); // Remove Traits.Count from stack
                codes[i + 1] = new CodeInstruction(OpCodes.Call, getMinMethod); // Push our min
                patchedMin = true;
                Plugin.Log.LogInfo("[Transpiler] RandomisedTraits: Replaced Traits.Count-1 with GetMinTraitCount()");
            }
            // Replace ldc.i4.2, add pattern (Traits.Count + 2)
            else if (!patchedMaxPlusOne && codes[i].opcode == OpCodes.Ldc_I4_2 &&
                     i + 1 < codes.Count &&
                     codes[i + 1].opcode == OpCodes.Add)
            {
                codes[i] = new CodeInstruction(OpCodes.Pop); // Remove Traits.Count from stack
                codes[i + 1] = new CodeInstruction(OpCodes.Call, getMaxPlusOneMethod); // Push max+1
                patchedMaxPlusOne = true;
                Plugin.Log.LogInfo("[Transpiler] RandomisedTraits: Replaced Traits.Count+2 with GetMaxPlusOneTraitCount()");
            }
        }

        if (!patchedMax)
        {
            Plugin.Log.LogWarning("[Transpiler] RandomisedTraits: Could not find ldc.i4.6 to patch.");
        }

        if (!patchedMin)
        {
            Plugin.Log.LogWarning("[Transpiler] RandomisedTraits: Could not find ldc.i4.1/sub pattern to patch.");
        }

        if (!patchedMaxPlusOne)
        {
            Plugin.Log.LogWarning("[Transpiler] RandomisedTraits: Could not find ldc.i4.2/add pattern to patch.");
        }

        return codes;
    }

    /// <summary>
    /// Returns the configured minimum trait count for RandomisedTraits transpiler.
    /// </summary>
    public static int GetMinTraitCount() => Plugin.MinimumTraits.Value;

    /// <summary>
    /// Returns a high value (100) to effectively disable the Clamp in RandomisedTraits.
    /// The vanilla code uses Clamp(value, 1, max) which caps both lower and upper bounds.
    /// By setting max=100, we let our min/max+1 values control the range directly.
    /// </summary>
    public static int GetMaxTraitCount() => 100;

    /// <summary>
    /// Returns max+1 for exclusive upper bound in Random.Range for RandomisedTraits transpiler.
    /// </summary>
    public static int GetMaxPlusOneTraitCount() => Plugin.MaximumTraits.Value + 1;

    /// <summary>
    /// Debug postfix to log before/after traits during re-indoctrination.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerInfo), nameof(FollowerInfo.RandomisedTraits))]
    private static void RandomisedTraits_Postfix(FollowerInfo __instance, int seed, List<FollowerTrait.TraitType> __result)
    {
        Plugin.Log.LogInfo($"[RandomisedTraits] Follower: {__instance.Name}");
        Plugin.Log.LogInfo($"[RandomisedTraits] Before: {__instance.Traits.Count} traits - {string.Join(", ", __instance.Traits)}");
        Plugin.Log.LogInfo($"[RandomisedTraits] After: {__result.Count} traits - {string.Join(", ", __result)}");
        Plugin.Log.LogInfo($"[RandomisedTraits] Config: min={Plugin.MinimumTraits.Value}, max={Plugin.MaximumTraits.Value}");
    }

    /// <summary>
    /// Prefix patch for re-indoctrination to randomize traits before the menu is shown.
    /// Vanilla re-indoctrination only changes appearance/name - this adds trait randomization.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Interaction_Reindoctrinate), nameof(Interaction_Reindoctrinate.SimpleNewRecruitRoutine))]
    private static void Reindoctrinate_Prefix(Interaction_Reindoctrinate __instance, bool customise)
    {
        if (!Plugin.RandomizeTraitsOnReindoctrination.Value)
        {
            return;
        }

        if (!customise || __instance.sacrificeFollower == null)
        {
            return;
        }

        var followerInfo = __instance.sacrificeFollower.Brain._directInfoAccess;
        Plugin.Log.LogInfo($"[Reindoctrinate] Starting re-indoctrination for {followerInfo.Name}");
        Plugin.Log.LogInfo($"[Reindoctrinate] Current traits ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

        // Generate new randomized traits using the patched RandomisedTraits method
        var seed = followerInfo.ID + TimeManager.CurrentDay;
        var newTraits = followerInfo.RandomisedTraits(seed);

        // Replace the follower's traits with the new randomized ones
        followerInfo.Traits.Clear();
        followerInfo.Traits.AddRange(newTraits);

        Plugin.Log.LogInfo($"[Reindoctrinate] New traits ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

        // Apply trait replacement if enabled (replaces negative traits with positive ones)
        if (Plugin.NoNegativeTraits.Value)
        {
            NoNegativeTraits.ProcessTraitReplacement(__instance.sacrificeFollower.Brain);
            Plugin.Log.LogInfo($"[Reindoctrinate] After trait replacement ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    // Trait Reroll via Reeducation
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Adds the Reeducate command to normal followers when enabled.
    /// Dissenters already have this command in vanilla.
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(FollowerCommandGroups), nameof(FollowerCommandGroups.NormalCommands))]
    private static void FollowerCommandGroups_NormalCommands_AddReeducate(ref List<CommandItem> __result)
    {
        if (!Plugin.TraitRerollOnReeducation.Value) return;
        __result.Add(FollowerCommandItems.Reeducate());
    }

    /// <summary>
    /// Wraps the reeducation routine to re-roll traits after completion.
    /// Only applies to normal followers (not dissenters).
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReeducateRoutine))]
    private static void ReeducateRoutine_Postfix(ref IEnumerator __result, interaction_FollowerInteraction __instance)
    {
        if (!Plugin.TraitRerollOnReeducation.Value) return;
        if (__instance.follower.Brain.Info.CursedState == Thought.Dissenter) return;

        __result = WrapReeducateRoutine(__result, __instance.follower.Brain);
    }

    /// <summary>
    /// Wrapper that runs the original reeducation routine, then re-rolls the follower's traits.
    /// Uses the already-patched RandomisedTraits method to respect TraitControl settings.
    /// </summary>
    private static IEnumerator WrapReeducateRoutine(IEnumerator original, FollowerBrain brain)
    {
        while (original.MoveNext())
        {
            yield return original.Current;
        }

        // Re-roll traits for normal followers
        if (brain.Info.CursedState != Thought.Dissenter)
        {
            // Clamp Reeducation stat back to 0 to prevent negative values
            if (brain.Stats.Reeducation < 0f)
            {
                brain.Stats.Reeducation = 0f;
            }

            var followerInfo = brain._directInfoAccess;
            Plugin.Log.LogInfo($"[Reeducate] Starting trait reroll for {followerInfo.Name}");
            Plugin.Log.LogInfo($"[Reeducate] Current traits ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

            // Generate new randomized traits using the patched RandomisedTraits method
            var seed = followerInfo.ID + TimeManager.CurrentDay;
            var newTraits = followerInfo.RandomisedTraits(seed);

            // Replace the follower's traits with the new randomized ones
            followerInfo.Traits.Clear();
            followerInfo.Traits.AddRange(newTraits);

            Plugin.Log.LogInfo($"[Reeducate] New traits ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

            // Apply trait replacement if enabled (replaces negative traits with positive ones)
            if (Plugin.NoNegativeTraits.Value)
            {
                NoNegativeTraits.ProcessTraitReplacement(brain);
                Plugin.Log.LogInfo($"[Reeducate] After trait replacement ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");
            }
        }
    }
}
