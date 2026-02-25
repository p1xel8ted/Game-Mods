using System.Text.RegularExpressions;
using BepInEx.Bootstrap;

namespace TraitControl;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
[BepInIncompatibility("NothingNegative")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.traitcontrol";
    internal const string PluginName = "Trait Control";
    private const string PluginVer = "0.1.7";

    private const string TraitReplacementSection = "── Trait Replacement ──";
    private const string UniqueTraitsSection = "── Unique Traits ──";
    private const string NotificationsSection = "── Notifications ──";
    private const string TraitWeightsSection = "── Trait Weights ──";
    private const string GoodTraitsSection = "── Good Traits ──";
    private const string BadTraitsSection = "── Bad Traits ──";
    private const string ResetSettingsSection = "── Reset Settings ──";

    internal static ManualLogSource Log { get; private set; }
    private static ConfigFile ConfigInstance { get; set; }
    private static bool? _isNothingNegativePresentCache;
    private static bool _showResetConfirmation;

    /// <summary>
    /// Traits that are granted through gameplay events and shouldn't be randomly assigned by default.
    /// </summary>
    internal static readonly HashSet<FollowerTrait.TraitType> StoryEventTraits =
    [
        // Marriage
        FollowerTrait.TraitType.MarriedHappily,
        FollowerTrait.TraitType.MarriedUnhappily,
        FollowerTrait.TraitType.MarriedJealous,
        FollowerTrait.TraitType.MarriedMurderouslyJealous,

        // Parenting
        FollowerTrait.TraitType.ProudParent,
        FollowerTrait.TraitType.OverworkedParent,

        // Widowing
        FollowerTrait.TraitType.HappilyWidowed,
        FollowerTrait.TraitType.GrievingWidow,
        FollowerTrait.TraitType.JiltedLover,

        // Criminal
        FollowerTrait.TraitType.CriminalEvangelizing,
        FollowerTrait.TraitType.CriminalHardened,
        FollowerTrait.TraitType.CriminalReformed,
        FollowerTrait.TraitType.CriminalScarred,

        // Missionary
        FollowerTrait.TraitType.MissionaryExcited,
        FollowerTrait.TraitType.MissionaryInspired,
        FollowerTrait.TraitType.MissionaryTerrified,

        // Other event traits
        FollowerTrait.TraitType.ExCultLeader,
        FollowerTrait.TraitType.ExistentialDread,

        // DLC/Special - Snowmen
        FollowerTrait.TraitType.InfusibleSnowman,
        FollowerTrait.TraitType.MasterfulSnowman,
        FollowerTrait.TraitType.ShoddySnowman,

        // DLC/Special - Other
        FollowerTrait.TraitType.MutatedVisual,
        FollowerTrait.TraitType.PureBlood,
        FollowerTrait.TraitType.PureBlood_1,
        FollowerTrait.TraitType.PureBlood_2,
        FollowerTrait.TraitType.PureBlood_3,
        FollowerTrait.TraitType.ChosenOne, // Granted to 4th generation child in PureBlood breeding chain
        FollowerTrait.TraitType.FreezeImmune
    ];

    private void Awake()
    {
        _instance = this;
        Log = Logger;
        ConfigInstance = Config;

        // Trait Replacement - 01
        NoNegativeTraits = ConfigInstance.Bind(TraitReplacementSection, "Enable Trait Replacement", false,
            new ConfigDescription(Localization.DescNoNegativeTraits, null,
                new ConfigurationManagerAttributes { Order = 10, DispName = Localization.NameEnableTraitReplacement }));
        NoNegativeTraits.SettingChanged += (_, _) => UpdateNoNegativeTraits();

        ApplyToExistingFollowers = ConfigInstance.Bind(TraitReplacementSection, "Apply To Existing Followers", false,
            new ConfigDescription(Localization.DescApplyToExisting, null,
                new ConfigurationManagerAttributes { Order = 9, DispName = Localization.NameApplyToExisting, CustomDrawer = DrawApplyToExistingToggle }));
        ApplyToExistingFollowers.SettingChanged += (_, _) => OnApplyToExistingChanged();

        UseUnlockedTraitsOnly = ConfigInstance.Bind(TraitReplacementSection, "Use Unlocked Traits Only", true,
            new ConfigDescription(Localization.DescUseUnlockedTraits, null,
                new ConfigurationManagerAttributes { Order = 8, DispName = Localization.NameUseUnlockedTraits }));
        UseUnlockedTraitsOnly.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            Patches.TraitWeights.RefreshAllTraitsList();
            UpdateTraitWeightVisibility();
        };

        UseAllTraits = ConfigInstance.Bind(TraitReplacementSection, "Use All Traits Pool", false,
            new ConfigDescription(Localization.DescUseAllTraits, null,
                new ConfigurationManagerAttributes { Order = 7, DispName = Localization.NameUseAllTraits }));

        PreferExclusiveCounterparts = ConfigInstance.Bind(TraitReplacementSection, "Prefer Exclusive Counterparts", true,
            new ConfigDescription(Localization.DescPreferExclusive, null,
                new ConfigurationManagerAttributes { Order = 6, DispName = Localization.NamePreferExclusive }));

        PreserveMutatedTrait = ConfigInstance.Bind(TraitReplacementSection, "Preserve Rot Followers", true,
            new ConfigDescription(Localization.DescPreserveMutated, null,
                new ConfigurationManagerAttributes { Order = 5, DispName = Localization.NamePreserveMutated }));

        MinimumTraits = ConfigInstance.Bind(TraitReplacementSection, "Minimum Traits", 2,
            new ConfigDescription(Localization.DescMinTraits,
                new AcceptableValueRange<int>(2, 8),
                new ConfigurationManagerAttributes { Order = 4, DispName = Localization.NameMinTraits }));

        MaximumTraits = ConfigInstance.Bind(TraitReplacementSection, "Maximum Traits", 3,
            new ConfigDescription(Localization.DescMaxTraits,
                new AcceptableValueRange<int>(2, 8),
                new ConfigurationManagerAttributes { Order = 3, DispName = Localization.NameMaxTraits }));

        // Ensure max >= min
        MaximumTraits.SettingChanged += (_, _) =>
        {
            if (MaximumTraits.Value < MinimumTraits.Value)
            {
                MaximumTraits.Value = MinimumTraits.Value;
            }
        };
        MinimumTraits.SettingChanged += (_, _) =>
        {
            if (MinimumTraits.Value > MaximumTraits.Value)
            {
                MaximumTraits.Value = MinimumTraits.Value;
            }
        };

        RandomizeTraitsOnReindoctrination = ConfigInstance.Bind(TraitReplacementSection, "Randomize Traits on Re-indoctrination", false,
            new ConfigDescription(Localization.DescRandomizeReindoc, null,
                new ConfigurationManagerAttributes { Order = 2, DispName = Localization.NameRandomizeReindoc }));

        TraitRerollOnReeducation = ConfigInstance.Bind(TraitReplacementSection, "Trait Reroll via Reeducation", false,
            new ConfigDescription(Localization.DescTraitReroll, null,
                new ConfigurationManagerAttributes { Order = 1, DispName = Localization.NameTraitReroll }));

        ProtectTraitCountOnReroll = ConfigInstance.Bind(TraitReplacementSection, "Protect Trait Count on Reroll", true,
            new ConfigDescription(Localization.DescProtectTraitCount, null,
                new ConfigurationManagerAttributes { Order = 0, DispName = Localization.NameProtectTraitCount }));

        RerollableAltarTraits = ConfigInstance.Bind(TraitReplacementSection, "Re-rollable Altar Traits", false,
            new ConfigDescription(Localization.DescRerollableAltar, null,
                new ConfigurationManagerAttributes { Order = -1, DispName = Localization.NameRerollableAltar }));

        // Unique Traits - 02
        AllowMultipleUniqueTraits = ConfigInstance.Bind(UniqueTraitsSection, "Allow Multiple Unique Traits", false,
            new ConfigDescription(Localization.DescAllowMultipleUnique, null,
                new ConfigurationManagerAttributes { Order = 100, DispName = Localization.NameAllowMultipleUnique }));

        IncludeImmortal = ConfigInstance.Bind(UniqueTraitsSection, "Include Immortal", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Immortal, Localization.SourceSpecialReward), null,
                new ConfigurationManagerAttributes { Order = 20, DispName = Localization.NameIncludeTrait("Immortal") }));
        IncludeImmortal.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeImmortal = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Immortal", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Immortal"), null,
                new ConfigurationManagerAttributes { Order = 19, DispName = Localization.NameGuaranteeTrait("Immortal") }));
        GuaranteeImmortal.SettingChanged += (_, _) =>
        {
            if (GuaranteeImmortal.Value && !IncludeImmortal.Value)
            {
                IncludeImmortal.Value = true;
            }
        };

        IncludeDisciple = ConfigInstance.Bind(UniqueTraitsSection, "Include Disciple", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Disciple, Localization.SourceSpecialReward), null,
                new ConfigurationManagerAttributes { Order = 18, DispName = Localization.NameIncludeTrait("Disciple") }));
        IncludeDisciple.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeDisciple = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Disciple", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Disciple"), null,
                new ConfigurationManagerAttributes { Order = 17, DispName = Localization.NameGuaranteeTrait("Disciple") }));
        GuaranteeDisciple.SettingChanged += (_, _) =>
        {
            if (GuaranteeDisciple.Value && !IncludeDisciple.Value)
            {
                IncludeDisciple.Value = true;
            }
        };

        IncludeDontStarve = ConfigInstance.Bind(UniqueTraitsSection, "Include Dont Starve", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.DontStarve, Localization.SourceCrossover), null,
                new ConfigurationManagerAttributes { Order = 16, DispName = Localization.NameIncludeTrait("Dont Starve") }));
        IncludeDontStarve.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeDontStarve = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Dont Starve", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Dont Starve"), null,
                new ConfigurationManagerAttributes { Order = 15, DispName = Localization.NameGuaranteeTrait("Dont Starve") }));
        GuaranteeDontStarve.SettingChanged += (_, _) =>
        {
            if (GuaranteeDontStarve.Value && !IncludeDontStarve.Value)
            {
                IncludeDontStarve.Value = true;
            }
        };

        IncludeBlind = ConfigInstance.Bind(UniqueTraitsSection, "Include Blind", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Blind, Localization.SourceCrossover), null,
                new ConfigurationManagerAttributes { Order = 14, DispName = Localization.NameIncludeTrait("Blind") }));
        IncludeBlind.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeBlind = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Blind", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Blind"), null,
                new ConfigurationManagerAttributes { Order = 13, DispName = Localization.NameGuaranteeTrait("Blind") }));
        GuaranteeBlind.SettingChanged += (_, _) =>
        {
            if (GuaranteeBlind.Value && !IncludeBlind.Value)
            {
                IncludeBlind.Value = true;
            }
        };

        IncludeBornToTheRot = ConfigInstance.Bind(UniqueTraitsSection, "Include Born To The Rot", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.BornToTheRot, Localization.SourceCrossover), null,
                new ConfigurationManagerAttributes { Order = 12, DispName = Localization.NameIncludeTrait("Born To The Rot") }));
        IncludeBornToTheRot.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeBornToTheRot = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Born To The Rot", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Born To The Rot"), null,
                new ConfigurationManagerAttributes { Order = 11, DispName = Localization.NameGuaranteeTrait("Born To The Rot") }));
        GuaranteeBornToTheRot.SettingChanged += (_, _) =>
        {
            if (GuaranteeBornToTheRot.Value && !IncludeBornToTheRot.Value)
            {
                IncludeBornToTheRot.Value = true;
            }
        };

        IncludeBishopOfCult = ConfigInstance.Bind(UniqueTraitsSection, "Include Ex-Bishop", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.BishopOfCult, Localization.SourceBishopConvert), null,
                new ConfigurationManagerAttributes { Order = 10, DispName = Localization.NameIncludeTrait("Ex-Bishop") }));
        IncludeBishopOfCult.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeBishopOfCult = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Ex-Bishop", false,
            new ConfigDescription(Localization.GuaranteeTraitDesc("Ex-Bishop"), null,
                new ConfigurationManagerAttributes { Order = 9, DispName = Localization.NameGuaranteeTrait("Ex-Bishop") }));
        GuaranteeBishopOfCult.SettingChanged += (_, _) =>
        {
            if (GuaranteeBishopOfCult.Value && !IncludeBishopOfCult.Value)
            {
                IncludeBishopOfCult.Value = true;
            }
        };

        // Notifications - 03
        ShowNotificationsWhenRemovingTraits = ConfigInstance.Bind(NotificationsSection, "Show When Removing Traits", false,
            new ConfigDescription(Localization.DescShowRemoving, null,
                new ConfigurationManagerAttributes { Order = 2, DispName = Localization.NameShowRemoving }));

        ShowNotificationsWhenAddingTraits = ConfigInstance.Bind(NotificationsSection, "Show When Adding Traits", false,
            new ConfigDescription(Localization.DescShowAdding, null,
                new ConfigurationManagerAttributes { Order = 1, DispName = Localization.NameShowAdding }));

        ShowNotificationOnTraitReroll = ConfigInstance.Bind(NotificationsSection, "Show On Trait Reroll", true,
            new ConfigDescription(Localization.DescShowReroll, null,
                new ConfigurationManagerAttributes { Order = 0, DispName = Localization.NameShowReroll }));

        // Trait Weights - 04
        EnableTraitWeights = ConfigInstance.Bind(TraitWeightsSection, "Enable Trait Weights", false,
            new ConfigDescription(Localization.DescEnableWeights, null,
                new ConfigurationManagerAttributes { Order = 100, DispName = Localization.NameEnableWeights }));
        EnableTraitWeights.SettingChanged += (_, _) => UpdateTraitWeightVisibility();

        IncludeStoryEventTraits = ConfigInstance.Bind(TraitWeightsSection, "Include Event Traits", false,
            new ConfigDescription(Localization.DescIncludeEventTraits, null,
                new ConfigurationManagerAttributes { Order = 99, DispName = Localization.NameIncludeEventTraits }));
        IncludeStoryEventTraits.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            Patches.TraitWeights.RefreshAllTraitsList();
            UpdateTraitWeightVisibility();
        };

        // Generate dynamic trait weight configs
        GenerateTraitWeightConfigs();

        // Apply initial visibility (handles unique trait toggles)
        UpdateTraitWeightVisibility();

        // Reset Settings - 07
        ConfigInstance.Bind(ResetSettingsSection, "Reset All Settings", false,
            new ConfigDescription(Localization.DescResetSettings, null,
                new ConfigurationManagerAttributes { Order = 0, DispName = Localization.NameResetSettings, HideDefaultButton = true, CustomDrawer = ResetAllSettings }));

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Log.LogInfo($"{PluginName} loaded.");
    }

    internal static void LogAllTraitNames()
    {
        Log.LogInfo("=== Trait Enum Names and Display Names ===");
        foreach (var trait in Enum.GetValues(typeof(FollowerTrait.TraitType)).Cast<FollowerTrait.TraitType>())
        {
            if (trait == FollowerTrait.TraitType.None)
            {
                continue;
            }

            var displayName = FollowerTrait.GetLocalizedTitle(trait);
            // Check if localization returned the raw key (not translated)
            var isLocalized = !string.IsNullOrEmpty(displayName) && !displayName.StartsWith("Traits/");
            Log.LogInfo($"  {trait} => \"{displayName}\"{(isLocalized ? "" : " [NOT LOCALIZED]")}");
        }
        Log.LogInfo("=== End Trait List ===");
    }

    private static void GenerateTraitWeightConfigs()
    {
        // Get ALL traits from the enum instead of just those in lists
        var allTraits = Enum.GetValues(typeof(FollowerTrait.TraitType))
            .Cast<FollowerTrait.TraitType>()
            .ToHashSet();

        // Remove None
        allTraits.Remove(FollowerTrait.TraitType.None);

        // Always exclude Spy - requires SpyJoinedDay or spies leave immediately
        allTraits.Remove(FollowerTrait.TraitType.Spy);

        // Store all traits (excluding event traits based on config) for use by patches
        AllTraitsList.Clear();
        IEnumerable<FollowerTrait.TraitType> traitsForPatches = allTraits;
        if (!IncludeStoryEventTraits.Value)
        {
            traitsForPatches = traitsForPatches.Except(StoryEventTraits);
        }
        AllTraitsList.AddRange(traitsForPatches);

        // Separate into good and bad traits
        var goodTraits = allTraits.Where(t => FollowerTrait.GoodTraits.Contains(t)).OrderBy(t => t.ToString()).ToList();
        var badTraits = allTraits.Where(t => !FollowerTrait.GoodTraits.Contains(t)).OrderBy(t => t.ToString()).ToList();

        var isHidden = !EnableTraitWeights.Value;

        // Generate good trait configs
        var goodOrder = goodTraits.Count;
        foreach (var trait in goodTraits)
        {
            CreateTraitWeightConfig(trait, GoodTraitsSection, goodOrder--, isHidden);
        }

        // Generate bad trait configs
        var badOrder = badTraits.Count;
        foreach (var trait in badTraits)
        {
            CreateTraitWeightConfig(trait, BadTraitsSection, badOrder--, isHidden);
        }

        Log.LogInfo($"Generated {TraitWeights.Count} trait weight configs ({goodTraits.Count} good, {badTraits.Count} bad).");
    }

    private static void CreateTraitWeightConfig(FollowerTrait.TraitType trait, string section, int order, bool isHidden)
    {
        var categories = GetTraitCategories(trait);
        var traitDescription = GetTraitDescription(trait);
        var internalName = trait.ToString();

        // Get localized display name for Configuration Manager UI
        var localizedName = GetTraitTitle(trait);
        var displayName = !string.IsNullOrEmpty(localizedName) ? $"{localizedName} ({internalName})" : null;

        var configDescription = string.IsNullOrEmpty(traitDescription)
            ? $"{Localization.TraitWeightDesc}{categories}"
            : $"{traitDescription}\n\n{Localization.TraitWeightDesc}{categories}";

        var weight = ConfigInstance.Bind(
            section,
            internalName,  // Use internal name as key (stable for config file)
            1.0f,
            new ConfigDescription(
                configDescription,
                new AcceptableValueRange<float>(0f, 100f),
                new ConfigurationManagerAttributes { Order = order, Browsable = !isHidden, DispName = displayName }
            )
        );

        // Snap to 0.05 increments, with values below 0.1 snapping directly to 0
        weight.SettingChanged += (_, _) =>
        {
            var rounded = weight.Value < 0.1f ? 0f : Mathf.Round(weight.Value / 0.05f) * 0.05f;
            if (!Mathf.Approximately(weight.Value, rounded))
            {
                weight.Value = rounded;
            }
        };

        TraitWeights[trait] = weight;
    }

    /// <summary>
    /// Returns category tags for a trait based on which game lists it belongs to.
    /// Traits can have multiple tags if they appear in multiple lists.
    /// </summary>
    private static string GetTraitCategories(FollowerTrait.TraitType trait)
    {
        var categories = new List<string>();

        if (StoryEventTraits.Contains(trait))
        {
            categories.Add("Event");
        }

        if (FollowerTrait.UniqueTraits.Contains(trait))
        {
            categories.Add("Unique");
        }

        if (FollowerTrait.SingleTraits.Contains(trait))
        {
            categories.Add("Single");
        }

        if (FollowerTrait.RareStartingTraits.Contains(trait))
        {
            categories.Add("Rare");
        }

        if (FollowerTrait.StartingTraits.Contains(trait))
        {
            categories.Add("Starting");
        }

        if (FollowerTrait.FaithfulTraits.Contains(trait))
        {
            categories.Add("Faithful");
        }

        if (FollowerTrait.MajorDLCTraits.Contains(trait))
        {
            categories.Add("DLC");
        }

        if (FollowerTrait.WinterSpecificTraits.Contains(trait))
        {
            categories.Add("Winter");
        }

        if (FollowerTrait.SinTraits.Contains(trait))
        {
            categories.Add("Sin");
        }

        if (FollowerTrait.RequiresOnboardingCompleted.Contains(trait))
        {
            categories.Add("Unlock");
        }

        return categories.Count > 0 ? $"\n\n{string.Format(Localization.CategoryFoundIn, string.Join(", ", categories))}" : $"\n\n{Localization.CategoryGrantedOther}";
    }

    /// <summary>
    /// Builds a description for unique trait toggles, including the game description if available.
    /// </summary>
    private static string BuildUniqueTraitDescription(FollowerTrait.TraitType trait, string source)
    {
        var gameDescription = GetTraitDescription(trait);
        var baseDescription = Localization.UniqueTraitDesc(trait.ToString(), source);

        if (!string.IsNullOrEmpty(gameDescription))
        {
            return $"{gameDescription}\n\n{baseDescription}";
        }

        return baseDescription;
    }

    /// <summary>
    /// Attempts to get the localized title for a trait.
    /// Returns null if localization is not available or returns the raw key.
    /// </summary>
    private static string GetTraitTitle(FollowerTrait.TraitType trait)
    {
        try
        {
            var title = FollowerTrait.GetLocalizedTitle(trait);
            // I2 Localization returns the key itself if no translation exists
            if (string.IsNullOrWhiteSpace(title) || title.StartsWith("Traits/"))
            {
                return null;
            }

            return StripRichText(title);
        }
        catch
        {
            // Localization system not ready
            return null;
        }
    }

    /// <summary>
    /// Attempts to get the localized description for a trait.
    /// Returns null if localization is not available or returns the raw key.
    /// </summary>
    private static string GetTraitDescription(FollowerTrait.TraitType trait)
    {
        try
        {
            var description = FollowerTrait.GetLocalizedDescription(trait);
            // I2 Localization returns the key itself if no translation exists
            if (string.IsNullOrWhiteSpace(description) || description.StartsWith("Traits/"))
            {
                return null;
            }

            return StripRichText(description);
        }
        catch
        {
            // Localization system not ready
            return null;
        }
    }

    /// <summary>
    /// Strips Unity rich text tags (color, sprite, size, etc.) from a string.
    /// </summary>
    private static string StripRichText(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Remove all XML-style tags like <color=#FFD201>, </color>, <sprite name="icon">, etc.
        return Regex.Replace(input, "<[^>]+>", string.Empty).Trim();
    }

    /// <summary>
    /// Sanitizes a string for use as a BepInEx config key.
    /// BepInEx cannot use: = \n \t \ " ' [ ]
    /// </summary>
    private static string SanitizeConfigKey(string input)
    {
        if (string.IsNullOrEmpty(input)) return null;

        // Remove or replace invalid characters
        var sanitized = input
            .Replace("=", "-")
            .Replace("\n", " ")
            .Replace("\t", " ")
            .Replace("\\", "-")
            .Replace("\"", "")
            .Replace("'", "")
            .Replace("[", "(")
            .Replace("]", ")");

        return string.IsNullOrWhiteSpace(sanitized) ? null : sanitized.Trim();
    }

    internal static Plugin _instance;

    /// <summary>
    /// Public wrapper for UpdateTraitWeightVisibility, callable from patches.
    /// </summary>
    internal static void RefreshTraitWeightVisibility() => UpdateTraitWeightVisibility();

    private static void UpdateTraitWeightVisibility()
    {
        var weightsEnabled = EnableTraitWeights.Value;

        foreach (var kvp in TraitWeights)
        {
            if (kvp.Value.Description?.Tags?.Length > 0 && kvp.Value.Description.Tags[0] is ConfigurationManagerAttributes attrs)
            {
                var show = weightsEnabled;

                // Check unique trait toggles
                if (show)
                {
                    show = kvp.Key switch
                    {
                        FollowerTrait.TraitType.Immortal => IncludeImmortal.Value,
                        FollowerTrait.TraitType.Disciple => IncludeDisciple.Value,
                        FollowerTrait.TraitType.DontStarve => IncludeDontStarve.Value,
                        FollowerTrait.TraitType.Blind => IncludeBlind.Value,
                        FollowerTrait.TraitType.BornToTheRot => IncludeBornToTheRot.Value,
                        FollowerTrait.TraitType.BishopOfCult => IncludeBishopOfCult.Value,
                        _ => true
                    };
                }

                // Check event traits toggle
                if (show && StoryEventTraits.Contains(kvp.Key))
                {
                    show = IncludeStoryEventTraits.Value;
                }

                // Check unlocked traits toggle - hide traits not available based on game unlock requirements
                if (show && UseUnlockedTraitsOnly.Value)
                {
                    show = !FollowerTrait.IsTraitUnavailable(kvp.Key);
                }

                attrs.Browsable = show;
            }
        }

        _instance?.StartCoroutine(RefreshConfigurationManager());
    }

    /// <summary>
    /// Updates the display names of trait weight configs with localized names.
    /// Called when localization initializes or language changes.
    /// Adds an asterisk (*) if localization failed for a trait.
    /// </summary>
    internal static void UpdateTraitDisplayNames()
    {
        foreach (var kvp in TraitWeights)
        {
            if (kvp.Value.Description?.Tags?.Length > 0 && kvp.Value.Description.Tags[0] is ConfigurationManagerAttributes attrs)
            {
                var internalName = kvp.Key.ToString();
                var localizedName = GetTraitTitle(kvp.Key);

                if (!string.IsNullOrEmpty(localizedName))
                {
                    attrs.DispName = $"{localizedName} ({internalName})";
                }
                else
                {
                    // Asterisk indicates localization not available
                    attrs.DispName = $"* {internalName}";
                }
            }
        }

        _instance?.StartCoroutine(RefreshConfigurationManager());
    }

    private static BaseUnityPlugin GetConfigurationManager()
    {
        return Chainloader.PluginInfos.Values
            .Where(p => p.Metadata.GUID == "com.bepis.bepinex.configurationmanager")
            .Select(p => p.Instance)
            .FirstOrDefault();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private static IEnumerator RefreshConfigurationManager()
    {
        yield return null; // Wait one frame

        var configManagerPlugin = GetConfigurationManager();
        if (configManagerPlugin == null)
        {
            Log.LogWarning("ConfigurationManager plugin not found");
            yield break;
        }

        var pluginType = configManagerPlugin.GetType();
        var displayingWindowProp = pluginType.GetProperty("DisplayingWindow");
        if (displayingWindowProp == null)
        {
            Log.LogWarning("ConfigurationManager.DisplayingWindow property not found");
            yield break;
        }

        if (!(bool)displayingWindowProp.GetValue(configManagerPlugin))
        {
            yield break;
        }

        var buildSettingListMethod = pluginType.GetMethod("BuildSettingList");
        if (buildSettingListMethod == null)
        {
            Log.LogWarning("ConfigurationManager.BuildSettingList method not found");
            yield break;
        }

        buildSettingListMethod.Invoke(configManagerPlugin, null);
    }

    private static void UpdateNoNegativeTraits()
    {
        if (IsNothingNegativePresent())
        {
            NoNegativeTraits.Value = false;
            return;
        }

        // Only process existing followers if that option is enabled
        if (!ApplyToExistingFollowers.Value) return;

        if (NoNegativeTraits.Value)
        {
            Patches.NoNegativeTraits.UpdateAllFollowerTraits();
        }
        else
        {
            Patches.NoNegativeTraits.RestoreOriginalTraits();
        }
    }

    private static void OnApplyToExistingChanged()
    {
        if (IsNothingNegativePresent()) return;

        // If turning ON while trait replacement is already enabled, process existing followers
        if (ApplyToExistingFollowers.Value && NoNegativeTraits.Value)
        {
            Patches.NoNegativeTraits.UpdateAllFollowerTraits();
        }
        // If turning OFF while trait replacement is enabled, restore original traits
        else if (!ApplyToExistingFollowers.Value && NoNegativeTraits.Value)
        {
            Patches.NoNegativeTraits.RestoreOriginalTraits();
        }
    }

    private static bool _showApplyToExistingWarning;

    private static void DrawApplyToExistingToggle(ConfigEntryBase entry)
    {
        var configEntry = (ConfigEntry<bool>)entry;

        if (_showApplyToExistingWarning)
        {
            GUILayout.Label(Localization.WarningModifyAll, GUILayout.ExpandWidth(true));
            GUILayout.Label(Localization.WarningNecklaceLoss, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Localization.ButtonConfirm, GUILayout.ExpandWidth(true)))
            {
                configEntry.Value = true;
                _showApplyToExistingWarning = false;
            }
            if (GUILayout.Button(Localization.ButtonCancel, GUILayout.ExpandWidth(true)))
            {
                _showApplyToExistingWarning = false;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            var newValue = GUILayout.Toggle(configEntry.Value, configEntry.Value ? Localization.ToggleEnabled : Localization.ToggleDisabled, GUILayout.ExpandWidth(true));
            if (newValue != configEntry.Value)
            {
                if (newValue)
                {
                    // Show warning before enabling
                    _showApplyToExistingWarning = true;
                }
                else
                {
                    configEntry.Value = false;
                }
            }
        }
    }

    internal static bool IsNothingNegativePresent()
    {
        _isNothingNegativePresentCache ??= Chainloader.PluginInfos.Any(plugin => plugin.Value.Instance.Info.Metadata.GUID.Equals("NothingNegative", StringComparison.OrdinalIgnoreCase));
        return _isNothingNegativePresentCache.Value;
    }

    internal static void L(string message)
    {
        Log.LogInfo(message);
    }

    private static void ResetAllSettings(ConfigEntryBase entry)
    {
        if (_showResetConfirmation)
        {
            DisplayResetConfirmation();
        }
        else
        {
            if (GUILayout.Button(Localization.ButtonResetAll, GUILayout.ExpandWidth(true)))
            {
                _showResetConfirmation = true;
            }
        }
    }

    private static void DisplayResetConfirmation()
    {
        GUILayout.Label(Localization.ConfirmResetAll);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(Localization.ButtonYes, GUILayout.ExpandWidth(true)))
        {
            ResetAllToDefaults();
            _showResetConfirmation = false;
        }
        if (GUILayout.Button(Localization.ButtonNo, GUILayout.ExpandWidth(true)))
        {
            _showResetConfirmation = false;
        }
        GUILayout.EndHorizontal();
    }

    private static void ResetAllToDefaults()
    {
        foreach (var entry in ConfigInstance.Entries
                     .Where(e => e.Value.BoxedValue != e.Value.DefaultValue)
                     .Where(e => e.Key.Section != ResetSettingsSection))
        {
            entry.Value.BoxedValue = entry.Value.DefaultValue;
            Log.LogInfo($"Reset {entry.Key} to default: {entry.Value.DefaultValue}");
        }

        // Regenerate traits and refresh UI
        Patches.NoNegativeTraits.GenerateAvailableTraits();
        Patches.TraitWeights.RefreshAllTraitsList();
        UpdateTraitWeightVisibility();
    }
}