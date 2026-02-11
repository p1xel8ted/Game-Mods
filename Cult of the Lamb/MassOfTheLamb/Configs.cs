namespace MassOfTheLamb;

public enum MassWolfTrapMode
{
    Disabled,
    FillOnly,
    CollectOnly,
    Both
}

/// <summary>
/// Controls how mass action costs are calculated.
/// </summary>
public enum MassActionCostMode
{
    /// <summary>Flat fee regardless of count.</summary>
    PerMassAction,

    /// <summary>Cost multiplied by number of objects affected.</summary>
    PerObject
}

public partial class Plugin
{
    // ── Mass Action Costs ──
    internal static ConfigEntry<MassActionCostMode> MassActionCostModeEntry { get; private set; }
    internal static ConfigEntry<bool> ShowMassActionCostPreview { get; private set; }
    internal static ConfigEntry<float> MassActionGoldCost { get; private set; }
    internal static ConfigEntry<float> MassActionTimeCost { get; private set; }
    internal static ConfigEntry<int> MassFaithReduction { get; private set; }

    // ── Mass Follower ──
    internal static ConfigEntry<int> MassNotificationThreshold { get; private set; }
    internal static ConfigEntry<bool> MassBribe { get; private set; }
    internal static ConfigEntry<bool> MassBless { get; private set; }
    internal static ConfigEntry<bool> MassExtort { get; private set; }
    internal static ConfigEntry<bool> MassIntimidate { get; private set; }
    internal static ConfigEntry<bool> MassIntimidateScareAll { get; private set; }
    internal static ConfigEntry<bool> MassInspire { get; private set; }
    internal static ConfigEntry<bool> MassRomance { get; private set; }
    internal static ConfigEntry<bool> MassBully { get; private set; }
    internal static ConfigEntry<bool> MassReassure { get; private set; }
    internal static ConfigEntry<bool> MassReeducate { get; private set; }
    internal static ConfigEntry<bool> MassLevelUp { get; private set; }
    internal static ConfigEntry<bool> MassLevelUpInstantSouls { get; private set; }
    internal static ConfigEntry<bool> MassPetFollower { get; private set; }
    internal static ConfigEntry<bool> MassPetAllFollowers { get; private set; }
    internal static ConfigEntry<bool> MassSinExtract { get; private set; }

    // ── Mass Animal ──
    internal static ConfigEntry<bool> MassPetAnimals { get; private set; }
    internal static ConfigEntry<bool> MassCleanAnimals { get; private set; }
    internal static ConfigEntry<bool> MassFeedAnimals { get; private set; }
    internal static ConfigEntry<bool> MassMilkAnimals { get; private set; }
    internal static ConfigEntry<bool> MassShearAnimals { get; private set; }
    internal static ConfigEntry<bool> FillTroughToCapacity { get; private set; }
    internal static ConfigEntry<bool> MassFillTroughs { get; private set; }

    // ── Mass Collect ──
    internal static ConfigEntry<bool> CollectAllGodTearsAtOnce { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromBeds { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromOuthouses { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromOfferingShrines { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromPassiveShrines { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromCompost { get; private set; }
    internal static ConfigEntry<bool> MassCollectFromHarvestTotems { get; private set; }
    internal static ConfigEntry<bool> MassCleanPoop { get; private set; }
    internal static ConfigEntry<bool> MassCleanVomit { get; private set; }

    // ── Mass Farm ──
    internal static ConfigEntry<bool> MassPlantSeeds { get; private set; }
    internal static ConfigEntry<bool> MassFertilize { get; private set; }
    internal static ConfigEntry<bool> MassWater { get; private set; }
    internal static ConfigEntry<bool> FillToolshedToCapacity { get; private set; }
    internal static ConfigEntry<bool> MassFillToolsheds { get; private set; }
    internal static ConfigEntry<bool> FillMedicToCapacity { get; private set; }
    internal static ConfigEntry<bool> MassFillMedicStations { get; private set; }
    internal static ConfigEntry<bool> FillSeedSiloToCapacity { get; private set; }
    internal static ConfigEntry<bool> MassFillSeedSilos { get; private set; }
    internal static ConfigEntry<bool> FillFertilizerSiloToCapacity { get; private set; }
    internal static ConfigEntry<bool> MassFillFertilizerSilos { get; private set; }
    internal static ConfigEntry<bool> MassOpenScarecrows { get; private set; }
    internal static ConfigEntry<MassWolfTrapMode> MassWolfTraps { get; private set; }

    // ── Mass Fill (Structures) ──
    internal static ConfigEntry<bool> RefineryMassFill { get; private set; }
    internal static ConfigEntry<bool> CookingFireMassFill { get; private set; }
    internal static ConfigEntry<bool> KitchenMassFill { get; private set; }
    internal static ConfigEntry<bool> PubMassFill { get; private set; }
}
