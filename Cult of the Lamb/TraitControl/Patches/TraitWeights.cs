using System.Reflection.Emit;

namespace TraitControl.Patches;

/// <summary>
/// Applies weighted random selection to starting trait generation.
/// Allows users to configure how often each trait appears, including disabling traits entirely.
/// </summary>
[Harmony]
public static class TraitWeights
{
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
    /// </summary>
    private static FollowerTrait.TraitType SelectTrait(IEnumerable<FollowerTrait.TraitType> sourceTraits)
    {
        if (Plugin.EnableTraitWeights.Value)
        {
            return GetWeightedTrait(sourceTraits);
        }

        return GetRandomTrait(sourceTraits);
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

            // Check game restrictions (same as vanilla):
            // - IsTraitUnavailable: checks DLC requirements, day requirements, etc.
            // - CultTraits: traits already applied cult-wide via doctrines
            if (!FollowerTrait.IsTraitUnavailable(trait) && !DataManager.Instance.CultTraits.Contains(trait))
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

            // Check game restrictions (same as vanilla):
            // - IsTraitUnavailable: checks DLC requirements, day requirements, etc.
            // - CultTraits: traits already applied cult-wide via doctrines
            if (!FollowerTrait.IsTraitUnavailable(trait) && !DataManager.Instance.CultTraits.Contains(trait))
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

        // Always filter out BishopOfCult - it's a special story trait
        availableTraits.Remove(FollowerTrait.TraitType.BishopOfCult);

        // Remove single-use traits that are already in use
        for (var i = availableTraits.Count - 1; i >= 0; i--)
        {
            var trait = availableTraits[i];

            // Check if any existing follower has this single-use trait
            foreach (var brain in FollowerBrain.AllBrains)
            {
                if (brain.HasTrait(trait) && FollowerTrait.SingleTraits.Contains(trait))
                {
                    availableTraits.RemoveAt(i);
                    break;
                }
            }
        }

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
}
