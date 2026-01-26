namespace TraitControl.Patches;

/// <summary>
/// Applies weighted random selection to starting trait generation.
/// Allows users to configure how often each trait appears, including disabling traits entirely.
/// </summary>
[Harmony]
public static class TraitWeights
{
    /// <summary>
    /// Prefix patch for GetStartingTrait that applies weighted random selection.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetStartingTrait))]
    public static bool GetStartingTrait_Prefix(ref FollowerTrait.TraitType __result)
    {
        if (!Plugin.EnableTraitWeights.Value)
        {
            return true; // Run original method
        }

        __result = GetWeightedTrait(FollowerTrait.StartingTraits);
        return false; // Skip original method
    }

    /// <summary>
    /// Prefix patch for GetRareTrait that applies weighted random selection.
    /// </summary>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FollowerTrait), nameof(FollowerTrait.GetRareTrait))]
    public static bool GetRareTrait_Prefix(ref FollowerTrait.TraitType __result)
    {
        if (!Plugin.EnableTraitWeights.Value)
        {
            return true; // Run original method
        }

        __result = GetWeightedTrait(FollowerTrait.RareStartingTraits);
        return false; // Skip original method
    }

    /// <summary>
    /// Selects a trait from the given list using weighted random selection.
    /// Respects game rules for unavailable traits and single-use traits.
    /// </summary>
    private static FollowerTrait.TraitType GetWeightedTrait(List<FollowerTrait.TraitType> sourceTraits)
    {
        // Build filtered list (respecting single-trait rules like vanilla)
        var availableTraits = new List<FollowerTrait.TraitType>(sourceTraits);
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

        // Try to find a valid trait with weighted selection
        var attempts = 0;
        while (++attempts < 100)
        {
            var trait = SelectWeightedTrait(availableTraits);
            if (trait == FollowerTrait.TraitType.None)
            {
                break; // No valid traits available
            }

            // Check game restrictions (same as vanilla)
            if (!FollowerTrait.IsTraitUnavailable(trait) && !DataManager.Instance.CultTraits.Contains(trait))
            {
                return trait;
            }
        }

        return FollowerTrait.TraitType.None;
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
