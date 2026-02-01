using System.Text.RegularExpressions;
using BepInEx.Bootstrap;

namespace TraitControl;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", BepInDependency.DependencyFlags.SoftDependency)]
[BepInIncompatibility("NothingNegative")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.traitcontrol";
    internal const string PluginName = "Trait Control";
    private const string PluginVer = "0.1.4";

    private const string TraitReplacementSection = "01. Trait Replacement";
    private const string UniqueTraitsSection = "02. Unique Traits";
    private const string NotificationsSection = "03. Notifications";
    private const string TraitWeightsSection = "04. Trait Weights";
    private const string GoodTraitsSection = "05. Good Traits";
    private const string BadTraitsSection = "06. Bad Traits";
    private const string ResetSettingsSection = "07. Reset Settings";

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
        FollowerTrait.TraitType.FreezeImmune
    ];

    private void Awake()
    {
        _instance = this;
        Log = Logger;
        ConfigInstance = Config;

        // Trait Replacement - 01
        NoNegativeTraits = ConfigInstance.Bind(TraitReplacementSection, "Enable Trait Replacement", false,
            new ConfigDescription("Replace negative traits with positive ones on all followers (existing and new).", null,
                new ConfigurationManagerAttributes { Order = 10 }));
        NoNegativeTraits.SettingChanged += (_, _) => UpdateNoNegativeTraits();

        UseUnlockedTraitsOnly = ConfigInstance.Bind(TraitReplacementSection, "Use Unlocked Traits Only", true,
            new ConfigDescription("Only use traits you have unlocked. Applies to both trait replacement and new follower trait selection.", null,
                new ConfigurationManagerAttributes { Order = 9 }));
        UseUnlockedTraitsOnly.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            Patches.TraitWeights.RefreshAllTraitsList();
            UpdateTraitWeightVisibility();
        };

        UseAllTraits = ConfigInstance.Bind(TraitReplacementSection, "Use All Traits Pool", false,
            new ConfigDescription("Merge all trait pools into one, bypassing vanilla's normal/rare split. Without this, vanilla assigns traits from separate pools (normal pool for first 2 traits, ~20% chance of rare pool for 3rd trait). Enable this for trait weights to have full control over distribution. Unique traits require their individual toggles.", null,
                new ConfigurationManagerAttributes { Order = 8 }));

        PreferExclusiveCounterparts = ConfigInstance.Bind(TraitReplacementSection, "Prefer Exclusive Counterparts", true,
            new ConfigDescription("When replacing negative traits, exclusive traits (like Lazy) are replaced with their positive counterpart (Industrious) instead of a random trait.", null,
                new ConfigurationManagerAttributes { Order = 7 }));

        MinimumTraits = ConfigInstance.Bind(TraitReplacementSection, "Minimum Traits", 2,
            new ConfigDescription("Minimum number of traits new followers will have. Vanilla is 2.",
                new AcceptableValueRange<int>(2, 8),
                new ConfigurationManagerAttributes { Order = 6 }));

        MaximumTraits = ConfigInstance.Bind(TraitReplacementSection, "Maximum Traits", 3,
            new ConfigDescription("Maximum number of traits new followers will have. Vanilla is 3. Limited to 8 due to UI constraints.",
                new AcceptableValueRange<int>(2, 8),
                new ConfigurationManagerAttributes { Order = 5 }));

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
            new ConfigDescription("When re-indoctrinating an existing follower (at the altar), randomize their traits using the configured min/max. Vanilla re-indoctrination only changes appearance/name.", null,
                new ConfigurationManagerAttributes { Order = 4 }));

        TraitRerollOnReeducation = ConfigInstance.Bind(TraitReplacementSection, "Trait Reroll via Reeducation", false,
            new ConfigDescription("Adds the Re-educate command to normal followers. Using it will re-roll their traits using the configured min/max and weights.", null,
                new ConfigurationManagerAttributes { Order = 3 }));

        // Unique Traits - 02
        AllowMultipleUniqueTraits = ConfigInstance.Bind(UniqueTraitsSection, "Allow Multiple Unique Traits", false,
            new ConfigDescription("Allow multiple followers to have the same unique trait (Immortal, Disciple, etc.). Normally only one follower can have each unique trait.", null,
                new ConfigurationManagerAttributes { Order = 100 }));

        IncludeImmortal = ConfigInstance.Bind(UniqueTraitsSection, "Include Immortal", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Immortal, "normally a special reward"), null,
                new ConfigurationManagerAttributes { Order = 20 }));
        IncludeImmortal.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeImmortal = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Immortal", false,
            new ConfigDescription("New followers will always receive the Immortal trait (ignores weights). Only one follower can have this trait.", null,
                new ConfigurationManagerAttributes { Order = 19, DispName = "    └ Guarantee Immortal" }));
        GuaranteeImmortal.SettingChanged += (_, _) =>
        {
            if (GuaranteeImmortal.Value && !IncludeImmortal.Value)
            {
                IncludeImmortal.Value = true;
            }
        };

        IncludeDisciple = ConfigInstance.Bind(UniqueTraitsSection, "Include Disciple", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Disciple, "normally a special reward"), null,
                new ConfigurationManagerAttributes { Order = 18 }));
        IncludeDisciple.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeDisciple = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Disciple", false,
            new ConfigDescription("New followers will always receive the Disciple trait (ignores weights). Only one follower can have this trait.", null,
                new ConfigurationManagerAttributes { Order = 17, DispName = "    └ Guarantee Disciple" }));
        GuaranteeDisciple.SettingChanged += (_, _) =>
        {
            if (GuaranteeDisciple.Value && !IncludeDisciple.Value)
            {
                IncludeDisciple.Value = true;
            }
        };

        IncludeDontStarve = ConfigInstance.Bind(UniqueTraitsSection, "Include Dont Starve", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.DontStarve, "crossover reward"), null,
                new ConfigurationManagerAttributes { Order = 16 }));
        IncludeDontStarve.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeDontStarve = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Dont Starve", false,
            new ConfigDescription("New followers will always receive the Dont Starve trait (ignores weights). Only one follower can have this trait.", null,
                new ConfigurationManagerAttributes { Order = 15, DispName = "    └ Guarantee Dont Starve" }));
        GuaranteeDontStarve.SettingChanged += (_, _) =>
        {
            if (GuaranteeDontStarve.Value && !IncludeDontStarve.Value)
            {
                IncludeDontStarve.Value = true;
            }
        };

        IncludeBlind = ConfigInstance.Bind(UniqueTraitsSection, "Include Blind", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.Blind, "crossover reward"), null,
                new ConfigurationManagerAttributes { Order = 14 }));
        IncludeBlind.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeBlind = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Blind", false,
            new ConfigDescription("New followers will always receive the Blind trait (ignores weights). Only one follower can have this trait.", null,
                new ConfigurationManagerAttributes { Order = 13, DispName = "    └ Guarantee Blind" }));
        GuaranteeBlind.SettingChanged += (_, _) =>
        {
            if (GuaranteeBlind.Value && !IncludeBlind.Value)
            {
                IncludeBlind.Value = true;
            }
        };

        IncludeBornToTheRot = ConfigInstance.Bind(UniqueTraitsSection, "Include Born To The Rot", false,
            new ConfigDescription(BuildUniqueTraitDescription(FollowerTrait.TraitType.BornToTheRot, "crossover reward"), null,
                new ConfigurationManagerAttributes { Order = 12 }));
        IncludeBornToTheRot.SettingChanged += (_, _) =>
        {
            Patches.NoNegativeTraits.GenerateAvailableTraits();
            UpdateTraitWeightVisibility();
        };

        GuaranteeBornToTheRot = ConfigInstance.Bind(UniqueTraitsSection, "Guarantee Born To The Rot", false,
            new ConfigDescription("New followers will always receive the Born To The Rot trait (ignores weights). Only one follower can have this trait.", null,
                new ConfigurationManagerAttributes { Order = 11, DispName = "    └ Guarantee Born To The Rot" }));
        GuaranteeBornToTheRot.SettingChanged += (_, _) =>
        {
            if (GuaranteeBornToTheRot.Value && !IncludeBornToTheRot.Value)
            {
                IncludeBornToTheRot.Value = true;
            }
        };

        // Notifications - 03
        ShowNotificationsWhenRemovingTraits = ConfigInstance.Bind(NotificationsSection, "Show When Removing Traits", false,
            new ConfigDescription("Show notifications when trait replacement removes negative traits.", null,
                new ConfigurationManagerAttributes { Order = 2 }));

        ShowNotificationsWhenAddingTraits = ConfigInstance.Bind(NotificationsSection, "Show When Adding Traits", false,
            new ConfigDescription("Show notifications when trait replacement adds positive traits.", null,
                new ConfigurationManagerAttributes { Order = 1 }));

        // Trait Weights - 04
        EnableTraitWeights = ConfigInstance.Bind(TraitWeightsSection, "Enable Trait Weights", false,
            new ConfigDescription("Enable weighted random selection for new followers. Weights affect trait selection within each pool. For full control over all traits, enable 'Use All Traits Pool' - otherwise vanilla's normal/rare pool split still applies (rare pool has ~20% chance for 3rd trait). Set a weight to 0 to disable a trait.", null,
                new ConfigurationManagerAttributes { Order = 100 }));
        EnableTraitWeights.SettingChanged += (_, _) => UpdateTraitWeightVisibility();

        IncludeStoryEventTraits = ConfigInstance.Bind(TraitWeightsSection, "Include Event Traits", false,
            new ConfigDescription("Include traits normally granted through gameplay events (marriage, parenting, criminal, missionary, etc.) in the weights list. Only applies when 'Use All Traits Pool' is enabled. Warning: This can result in nonsensical assignments (e.g., ProudParent on followers who have never had children).", null,
                new ConfigurationManagerAttributes { Order = 99 }));
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
            new ConfigDescription("Click to reset all settings to defaults (vanilla behavior).", null,
                new ConfigurationManagerAttributes { Order = 0, HideDefaultButton = true, CustomDrawer = ResetAllSettings }));

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

        // Always exclude - these require special game state setup
        allTraits.Remove(FollowerTrait.TraitType.Spy); // Requires SpyJoinedDay or spies leave immediately
        allTraits.Remove(FollowerTrait.TraitType.BishopOfCult); // Story-related, granted when converting a bishop

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
            ? $"Weight: Higher = more likely relative to other traits. Set to 0 to disable. Default is 1.0. With ~85 traits at weight 1: weight 10 ≈ 10%, weight 50 ≈ 37%, weight 100 ≈ 54%.{categories}"
            : $"{traitDescription}\n\nWeight: Higher = more likely relative to other traits. Set to 0 to disable. Default is 1.0. With ~85 traits at weight 1: weight 10 ≈ 10%, weight 50 ≈ 37%, weight 100 ≈ 54%.{categories}";

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

        return categories.Count > 0 ? $"\n\nFound in: {string.Join(", ", categories)}" : "\n\nGranted via other means (doctrines, rituals, events, etc.)";
    }

    /// <summary>
    /// Builds a description for unique trait toggles, including the game description if available.
    /// </summary>
    private static string BuildUniqueTraitDescription(FollowerTrait.TraitType trait, string source)
    {
        var gameDescription = GetTraitDescription(trait);
        var baseDescription = $"Allow the {trait} trait ({source}) to appear in trait pools.";

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

    private static Plugin _instance;

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
        return (from pluginInfo in Chainloader.PluginInfos.Values where pluginInfo.Metadata.GUID == "com.p1xel8ted.configurationmanagerenhanced" select pluginInfo.Instance).FirstOrDefault();
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

        if (NoNegativeTraits.Value)
        {
            Patches.NoNegativeTraits.UpdateAllFollowerTraits();
        }
        else
        {
            Patches.NoNegativeTraits.RestoreOriginalTraits();
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
            if (GUILayout.Button("Reset All Settings", GUILayout.ExpandWidth(true)))
            {
                _showResetConfirmation = true;
            }
        }
    }

    private static void DisplayResetConfirmation()
    {
        GUILayout.Label("Are you sure? This will reset all settings to defaults.");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes", GUILayout.ExpandWidth(true)))
        {
            ResetAllToDefaults();
            _showResetConfirmation = false;
        }
        if (GUILayout.Button("No", GUILayout.ExpandWidth(true)))
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