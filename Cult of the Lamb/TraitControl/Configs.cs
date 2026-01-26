namespace TraitControl;

public partial class Plugin
{
    // Trait Replacement settings
    internal static ConfigEntry<bool> NoNegativeTraits { get; private set; }
    internal static ConfigEntry<bool> UseUnlockedTraitsOnly { get; private set; }
    internal static ConfigEntry<bool> IncludeImmortal { get; private set; }
    internal static ConfigEntry<bool> IncludeDisciple { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenRemovingTraits { get; private set; }
    internal static ConfigEntry<bool> ShowNotificationsWhenAddingTraits { get; private set; }

    // Trait Weights settings
    internal static ConfigEntry<bool> EnableTraitWeights { get; private set; }
    internal static Dictionary<FollowerTrait.TraitType, ConfigEntry<float>> TraitWeights { get; } = new();
}
