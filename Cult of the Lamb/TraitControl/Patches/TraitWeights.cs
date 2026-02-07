namespace TraitControl.Patches;

/// <summary>
/// Applies weighted random selection to starting trait generation.
/// Allows users to configure how often each trait appears, including disabling traits entirely.
/// </summary>
[Harmony]
public static class TraitWeights
{
    /// <summary>
    /// Tracks which traits have been assigned during the current follower's trait generation session.
    /// Prevents infinite loops when the weighted trait pool is restricted to a small number of traits.
    /// Reset by FollowerBrain constructor prefix and before re-indoctrinate/re-educate trait generation.
    /// </summary>
    private static readonly HashSet<FollowerTrait.TraitType> TraitsAssignedThisSession = [];

    /// <summary>
    /// Tracks whether the current reeducation is a TraitControl trait reroll (not a vanilla dissenter re-education).
    /// Used by the transpiler to conditionally skip AddPleasure - we don't want sin accumulation from trait rerolls.
    /// </summary>
    private static bool _isTraitRerollReeducation;

    /// <summary>
    /// Helper method for transpiler - returns true if the current reeducation is a trait reroll.
    /// When true, the AddPleasure call should be skipped to prevent sin accumulation.
    /// </summary>
    public static bool ShouldSkipReeducationPleasure() => _isTraitRerollReeducation;

    /// <summary>
    /// Conditional wrapper for AddPleasure used by the transpiler.
    /// Skips the call when this is a trait reroll (prevents sin accumulation and animation).
    /// Allows the call through for normal dissenter reeducation.
    /// </summary>
    public static void ConditionalAddPleasure(FollowerBrain brain, FollowerBrain.PleasureActions action, float multiplier)
    {
        if (ShouldSkipReeducationPleasure())
        {
            Plugin.Log.LogInfo("[Reeducate] Skipping AddPleasure - this is a trait reroll, not dissenter conversion");
            return;
        }

        brain.AddPleasure(action, multiplier);
    }

    /// <summary>
    /// Resets the guarantee flag before trait assignment begins for a new follower.
    /// This ensures the guaranteed trait is only given once per follower, not on every trait roll.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerBrain), MethodType.Constructor, typeof(FollowerInfo))]
    private static void FollowerBrain_Constructor_Prefix()
    {
        ResetSessionTracking();
    }

    /// <summary>
    /// Resets session tracking for trait assignment.
    /// Called before new follower creation and before re-indoctrinate/re-educate trait generation.
    /// </summary>
    internal static void ResetSessionTracking()
    {
        TraitsAssignedThisSession.Clear();
        PopulateGuaranteedTraits();
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
    /// Guaranteed traits take priority over all other selection methods.
    /// </summary>
    private static FollowerTrait.TraitType SelectTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        // Check for guaranteed traits first (bypasses all other selection)
        // Each call returns the next pending guaranteed trait until all are given
        var guaranteed = GetGuaranteedTrait();
        if (guaranteed != FollowerTrait.TraitType.None)
        {
            TraitsAssignedThisSession.Add(guaranteed);
            return guaranteed;
        }

        // Debug: Log source traits count
        var sourceList = sourceTraits.ToList();
        Plugin.Log.LogInfo($"[SelectTrait] Source traits count: {sourceList.Count}, UseAllTraits: {Plugin.UseAllTraits.Value}");

        FollowerTrait.TraitType result;
        if (Plugin.EnableTraitWeights.Value)
        {
            result = GetWeightedTrait(sourceList);
        }
        else
        {
            result = GetRandomTrait(sourceList);
        }

        // Log warning when no more traits available (user will get fewer traits than configured)
        if (result == FollowerTrait.TraitType.None && TraitsAssignedThisSession.Count > 0)
        {
            Plugin.Log.LogWarning($"[SelectTrait] Trait pool exhausted after {TraitsAssignedThisSession.Count} traits. " +
                                  $"Follower will receive fewer traits than the configured minimum ({Plugin.MinimumTraits.Value}). " +
                                  "Consider enabling more traits with non-zero weights.");
        }

        return result;
    }

    /// <summary>
    /// List of guaranteed traits that still need to be given during the current follower's creation.
    /// Populated at the start of trait generation and consumed one at a time.
    /// </summary>
    private static readonly List<FollowerTrait.TraitType> _pendingGuaranteedTraits = [];

    /// <summary>
    /// Populates the pending guaranteed traits list with all enabled and available guaranteed traits.
    /// Called at the start of trait generation for a new follower.
    /// </summary>
    private static void PopulateGuaranteedTraits()
    {
        _pendingGuaranteedTraits.Clear();

        // Add all enabled guaranteed traits that are available
        if (Plugin.GuaranteeImmortal.Value && Plugin.IncludeImmortal.Value && IsTraitAvailable(FollowerTrait.TraitType.Immortal))
        {
            _pendingGuaranteedTraits.Add(FollowerTrait.TraitType.Immortal);
        }

        if (Plugin.GuaranteeDisciple.Value && Plugin.IncludeDisciple.Value && IsTraitAvailable(FollowerTrait.TraitType.Disciple))
        {
            _pendingGuaranteedTraits.Add(FollowerTrait.TraitType.Disciple);
        }

        if (Plugin.GuaranteeDontStarve.Value && Plugin.IncludeDontStarve.Value && IsTraitAvailable(FollowerTrait.TraitType.DontStarve))
        {
            _pendingGuaranteedTraits.Add(FollowerTrait.TraitType.DontStarve);
        }

        if (Plugin.GuaranteeBlind.Value && Plugin.IncludeBlind.Value && IsTraitAvailable(FollowerTrait.TraitType.Blind))
        {
            _pendingGuaranteedTraits.Add(FollowerTrait.TraitType.Blind);
        }

        if (Plugin.GuaranteeBornToTheRot.Value && Plugin.IncludeBornToTheRot.Value && IsTraitAvailable(FollowerTrait.TraitType.BornToTheRot))
        {
            _pendingGuaranteedTraits.Add(FollowerTrait.TraitType.BornToTheRot);
        }

        if (_pendingGuaranteedTraits.Count > 0)
        {
            Plugin.Log.LogInfo($"[GuaranteedTraits] Queued {_pendingGuaranteedTraits.Count} guaranteed traits: {string.Join(", ", _pendingGuaranteedTraits)}");
        }
    }

    /// <summary>
    /// Returns the next pending guaranteed trait, or None if all have been given.
    /// Removes the trait from the pending list so it won't be given again.
    /// </summary>
    private static FollowerTrait.TraitType GetGuaranteedTrait()
    {
        if (_pendingGuaranteedTraits.Count == 0)
        {
            return FollowerTrait.TraitType.None;
        }

        var trait = _pendingGuaranteedTraits[0];
        _pendingGuaranteedTraits.RemoveAt(0);
        Plugin.Log.LogInfo($"[GuaranteedTraits] Giving guaranteed trait: {trait} ({_pendingGuaranteedTraits.Count} remaining)");
        return trait;
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
    /// Excludes traits already assigned in the current session to prevent infinite loops.
    /// </summary>
    private static FollowerTrait.TraitType GetRandomTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        var availableTraits = GetFilteredTraits(sourceTraits);

        // Remove traits already assigned this session to prevent infinite loops
        availableTraits.RemoveAll(t => TraitsAssignedThisSession.Contains(t));

        // Try to find a valid trait with random selection
        var attempts = 0;
        while (++attempts < 100 && availableTraits.Count > 0)
        {
            var trait = availableTraits[Random.Range(0, availableTraits.Count)];

            // Check game restrictions (DLC requirements, day requirements, etc.)
            if (!FollowerTrait.IsTraitUnavailable(trait))
            {
                TraitsAssignedThisSession.Add(trait);
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

        // Filter out ALL negative traits when trait replacement is enabled
        // This prevents negative traits from being selected, avoiding the need for post-hoc replacement
        if (Plugin.NoNegativeTraits.Value)
        {
            availableTraits.RemoveAll(t => !FollowerTrait.IsPositiveTrait(t));
            Plugin.Log.LogInfo($"[GetFilteredTraits] Removed negative traits (NoNegativeTraits enabled): {initialCount} -> {availableTraits.Count}");
        }

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

        // Filter out cult traits (doctrine-granted traits that apply cult-wide)
        // These shouldn't be rolled as individual traits - they'd waste a slot
        // Traits like Fertility, Allegiance, Cannibal, etc. from doctrines
        if (DataManager.Instance?.CultTraits != null)
        {
            foreach (var cultTrait in DataManager.Instance.CultTraits)
            {
                availableTraits.Remove((FollowerTrait.TraitType)cultTrait);
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
    /// Excludes traits already assigned in the current session to prevent infinite loops.
    /// </summary>
    private static FollowerTrait.TraitType SelectWeightedTrait(List<FollowerTrait.TraitType> traits)
    {
        if (traits.Count == 0)
        {
            return FollowerTrait.TraitType.None;
        }

        // Build weight list, excluding traits already assigned this session
        var weights = new List<float>();
        var validTraits = new List<FollowerTrait.TraitType>();
        var totalWeight = 0f;

        foreach (var trait in traits)
        {
            // Skip traits already assigned in this session to prevent infinite loops
            if (TraitsAssignedThisSession.Contains(trait))
            {
                continue;
            }

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
                var selected = validTraits[i];
                TraitsAssignedThisSession.Add(selected);
                return selected;
            }
        }

        // Fallback (shouldn't reach here)
        var fallback = validTraits[validTraits.Count - 1];
        TraitsAssignedThisSession.Add(fallback);
        return fallback;
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

        // Reset session tracking before generating new traits to prevent infinite loops
        ResetSessionTracking();

        var followerInfo = __instance.sacrificeFollower.Brain._directInfoAccess;
        var oldTraitCount = followerInfo.Traits.Count;
        Plugin.Log.LogInfo($"[Reindoctrinate] Starting re-indoctrination for {followerInfo.Name}");
        Plugin.Log.LogInfo($"[Reindoctrinate] Current traits ({oldTraitCount}): {string.Join(", ", followerInfo.Traits)}");

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

        // Protect trait count if enabled - add more traits if we ended up with fewer
        if (Plugin.ProtectTraitCountOnReroll.Value && followerInfo.Traits.Count < oldTraitCount)
        {
            var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.StartingTraits;
            var attempts = 0;
            while (followerInfo.Traits.Count < oldTraitCount && attempts < 100)
            {
                attempts++;
                var trait = SelectTrait(sourceTraits);
                if (trait == FollowerTrait.TraitType.None)
                {
                    break;
                }

                if (!followerInfo.Traits.Contains(trait))
                {
                    followerInfo.Traits.Add(trait);
                    Plugin.Log.LogInfo($"[Reindoctrinate] Protected trait count - added {trait}");
                }
            }
            Plugin.Log.LogInfo($"[Reindoctrinate] After protection ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

            // Re-apply trait replacement to handle any negative traits added by protection
            if (Plugin.NoNegativeTraits.Value)
            {
                NoNegativeTraits.ProcessTraitReplacement(__instance.sacrificeFollower.Brain);
                Plugin.Log.LogInfo($"[Reindoctrinate] After post-protection replacement ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");
            }
        }

        // Show notification if enabled
        if (Plugin.ShowNotificationOnTraitReroll.Value)
        {
            var newTraitCount = followerInfo.Traits.Count;
            NotificationCentre.Instance?.PlayGenericNotification($"<color=#FFD201>{followerInfo.Name}</color>'s traits rerolled! ({oldTraitCount} → {newTraitCount})");
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

        // Defensive null checks to prevent crashes
        if (__instance?.follower?.Brain?.Info == null) return;

        if (__instance.follower.Brain.Info.CursedState == Thought.Dissenter) return;

        // Set flag to indicate this is a trait reroll, not a vanilla dissenter re-education
        // This tells the transpiler to skip AddPleasure to prevent sin accumulation
        _isTraitRerollReeducation = true;

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

            // Reset session tracking before generating new traits to prevent infinite loops
            ResetSessionTracking();

            var followerInfo = brain._directInfoAccess;
            var oldTraitCount = followerInfo.Traits.Count;
            Plugin.Log.LogInfo($"[Reeducate] Starting trait reroll for {followerInfo.Name}");
            Plugin.Log.LogInfo($"[Reeducate] Current traits ({oldTraitCount}): {string.Join(", ", followerInfo.Traits)}");

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

            // Protect trait count if enabled - add more traits if we ended up with fewer
            if (Plugin.ProtectTraitCountOnReroll.Value && followerInfo.Traits.Count < oldTraitCount)
            {
                var sourceTraits = Plugin.UseAllTraits.Value ? Plugin.AllTraitsList : FollowerTrait.StartingTraits;
                var attempts = 0;
                while (followerInfo.Traits.Count < oldTraitCount && attempts < 100)
                {
                    attempts++;
                    var trait = SelectTrait(sourceTraits);
                    if (trait == FollowerTrait.TraitType.None)
                    {
                        break;
                    }

                    if (!followerInfo.Traits.Contains(trait))
                    {
                        followerInfo.Traits.Add(trait);
                        Plugin.Log.LogInfo($"[Reeducate] Protected trait count - added {trait}");
                    }
                }
                Plugin.Log.LogInfo($"[Reeducate] After protection ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");

                // Re-apply trait replacement to handle any negative traits added by protection
                if (Plugin.NoNegativeTraits.Value)
                {
                    NoNegativeTraits.ProcessTraitReplacement(brain);
                    Plugin.Log.LogInfo($"[Reeducate] After post-protection replacement ({followerInfo.Traits.Count}): {string.Join(", ", followerInfo.Traits)}");
                }
            }

            // Show notification if enabled
            if (Plugin.ShowNotificationOnTraitReroll.Value)
            {
                var newTraitCount = followerInfo.Traits.Count;
                NotificationCentre.Instance?.PlayGenericNotification($"<color=#FFD201>{followerInfo.Name}</color>'s traits rerolled! ({oldTraitCount} > {newTraitCount})");
            }
        }

        // Clear the flag now that reeducation is complete
        _isTraitRerollReeducation = false;
    }

    // ══════════════════════════════════════════════════════════════════════
    // Reeducation Pleasure Skip Transpiler
    // ══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Transpiler for ReeducateRoutine - conditionally skips AddPleasure for trait rerolls.
    /// Vanilla code adds 50 pleasure points when CursedState == None, which causes sin accumulation.
    /// For trait rerolls (normal followers), we skip this to prevent unintended sin buildup.
    /// For vanilla dissenter re-education, AddPleasure runs normally.
    /// </summary>
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(interaction_FollowerInteraction), nameof(interaction_FollowerInteraction.ReeducateRoutine), MethodType.Enumerator)]
    private static IEnumerable<CodeInstruction> ReeducateRoutine_Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        var pleasureMethod = AccessTools.Method(typeof(FollowerBrain), nameof(FollowerBrain.AddPleasure), [typeof(FollowerBrain.PleasureActions), typeof(float)]);
        var conditionalMethod = AccessTools.Method(typeof(TraitWeights), nameof(ConditionalAddPleasure));
        var patched = false;

        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].Calls(pleasureMethod))
            {
                // Replace callvirt AddPleasure with call ConditionalAddPleasure
                // Stack is already: [brain] [PleasureActions] [float] - same params, just static instead of instance
                codes[i].opcode = OpCodes.Call;
                codes[i].operand = conditionalMethod;
                patched = true;
                Plugin.Log.LogInfo("[Transpiler] ReeducateRoutine: Replaced AddPleasure with ConditionalAddPleasure");
                break;
            }
        }

        if (!patched)
        {
            Plugin.Log.LogWarning("[Transpiler] ReeducateRoutine: Could not find AddPleasure call to patch");
        }

        return codes;
    }
}