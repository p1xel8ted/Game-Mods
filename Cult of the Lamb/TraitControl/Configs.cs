namespace TraitControl;

public partial class Plugin
{
    // Trait Replacement settings
    internal static ConfigEntry<bool> NoNegativeTraits { get; private set; }
    internal static ConfigEntry<bool> ApplyToExistingFollowers { get; private set; }
    internal static ConfigEntry<bool> UseUnlockedTraitsOnly { get; private set; }
    internal static ConfigEntry<bool> PreferExclusiveCounterparts { get; private set; }
    internal static ConfigEntry<bool> PreserveMutatedTrait { get; private set; }
    internal static ConfigEntry<bool> AllowMultipleUniqueTraits { get; private set; }
    internal static ConfigEntry<int> MinimumTraits { get; private set; }
    internal static ConfigEntry<int> MaximumTraits { get; private set; }
    internal static ConfigEntry<bool> RandomizeTraitsOnReindoctrination { get; private set; }
    internal static ConfigEntry<bool> TraitRerollOnReeducation { get; private set; }
    internal static ConfigEntry<bool> ProtectTraitCountOnReroll { get; private set; }
    internal static ConfigEntry<bool> RerollableAltarTraits { get; private set; }
    internal static ConfigEntry<bool> IncludeImmortal { get; private set; }
    internal static ConfigEntry<bool> GuaranteeImmortal { get; private set; }
    internal static ConfigEntry<bool> IncludeDisciple { get; private set; }
    internal static ConfigEntry<bool> GuaranteeDisciple { get; private set; }
    internal static ConfigEntry<bool> IncludeDontStarve { get; private set; }
    internal static ConfigEntry<bool> GuaranteeDontStarve { get; private set; }
    internal static ConfigEntry<bool> IncludeBlind { get; private set; }
    internal static ConfigEntry<bool> GuaranteeBlind { get; private set; }
    internal static ConfigEntry<bool> IncludeBornToTheRot { get; private set; }
    internal static ConfigEntry<bool> GuaranteeBornToTheRot { get; private set; }
    internal static ConfigEntry<bool> IncludeBishopOfCult { get; private set; }
    internal static ConfigEntry<bool> GuaranteeBishopOfCult { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenRemovingTraits { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenAddingTraits { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationOnTraitReroll { get; private set; }

    // Trait Weights settings
    internal static ConfigEntry<bool> EnableTraitWeights { get; private set; }
    internal static ConfigEntry<bool> IncludeStoryEventTraits { get; private set; }
    internal static ConfigEntry<bool> UseAllTraits { get; private set; }
    internal static Dictionary<FollowerTrait.TraitType, ConfigEntry<float>> TraitWeights { get; } = new();
    internal static List<FollowerTrait.TraitType> AllTraitsList { get; } = [];
}
