namespace TraitControl;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
[BepInIncompatibility("NothingNegative")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.traitcontrol";
    internal const string PluginName = "Trait Control";
    private const string PluginVer = "0.1.3";

    private const string TraitReplacementSection = "01. Trait Replacement";
    private const string UniqueTraitsSection = "02. Unique Traits";
    private const string NotificationsSection = "03. Notifications";
    private const string TraitWeightsSection = "04. Trait Weights";
    private const string GoodTraitsSection = "05. Good Traits";
    private const string BadTraitsSection = "06. Bad Traits";

    internal static ManualLogSource Log { get; private set; }
    private static ConfigFile ConfigInstance { get; set; }
    private static bool? _isNothingNegativePresentCache;

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
        UseUnlockedTraitsOnly.SettingChanged += (_, _) => Patches.NoNegativeTraits.GenerateAvailableTraits();

        UseAllTraits = ConfigInstance.Bind(TraitReplacementSection, "Use All Traits Pool", false,
            new ConfigDescription("Pull from ALL traits instead of the game's separate pools (Starting, Rare, Faithful). If 'Use Unlocked Traits Only' is enabled, only unlocked traits will be used. Unique traits require their individual toggles to be enabled.", null,
                new ConfigurationManagerAttributes { Order = 8 }));

        PreferExclusiveCounterparts = ConfigInstance.Bind(TraitReplacementSection, "Prefer Exclusive Counterparts", true,
            new ConfigDescription("When replacing negative traits, exclusive traits (like Lazy) are replaced with their positive counterpart (Industrious) instead of a random trait.", null,
                new ConfigurationManagerAttributes { Order = 7 }));

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
            new ConfigDescription("Enable weighted random selection for new followers. When enabled, you can configure how often each trait appears below. Set a weight to 0 to disable that trait entirely. This does not override settings in the sections above.", null,
                new ConfigurationManagerAttributes { Order = 100 }));
        EnableTraitWeights.SettingChanged += (_, _) => UpdateTraitWeightVisibility();

        // Generate dynamic trait weight configs
        GenerateTraitWeightConfigs();

        // Apply initial visibility (handles unique trait toggles)
        UpdateTraitWeightVisibility();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Log.LogInfo($"{PluginName} loaded.");
    }

    private static void GenerateTraitWeightConfigs()
    {
        // Merge all trait lists into one unique set
        var allTraits = new HashSet<FollowerTrait.TraitType>();

        var traitFields = AccessTools.GetDeclaredFields(typeof(FollowerTrait))
            .Where(f => f.FieldType == typeof(List<FollowerTrait.TraitType>))
            .ToList();

        foreach (var field in traitFields)
        {
            if (field.GetValue(null) is List<FollowerTrait.TraitType> traitList)
            {
                allTraits.UnionWith(traitList);
            }
        }

        // Remove None if present
        allTraits.Remove(FollowerTrait.TraitType.None);

        // Store for use by patches
        AllTraitsList.Clear();
        AllTraitsList.AddRange(allTraits);

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
        var traitDescription = GetTraitDescription(trait);
        var configDescription = string.IsNullOrEmpty(traitDescription)
            ? $"Weight for {trait}. Higher = more likely relative to other traits. Set to 0 to disable. Default is 1.0. With ~85 traits at weight 1: weight 10 ≈ 10%, weight 50 ≈ 37%, weight 100 ≈ 54%."
            : $"{traitDescription}\n\nWeight: Higher = more likely relative to other traits. Set to 0 to disable. Default is 1.0. With ~85 traits at weight 1: weight 10 ≈ 10%, weight 50 ≈ 37%, weight 100 ≈ 54%.";

        var weight = ConfigInstance.Bind(
            section,
            trait.ToString(),
            1.0f,
            new ConfigDescription(
                configDescription,
                new AcceptableValueRange<float>(0f, 100f),
                new ConfigurationManagerAttributes { Order = order, Browsable = !isHidden }
            )
        );

        TraitWeights[trait] = weight;
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
        return System.Text.RegularExpressions.Regex.Replace(input, "<[^>]+>", string.Empty).Trim();
    }

    private static Plugin _instance;

    private static void UpdateTraitWeightVisibility()
    {
        var weightsEnabled = EnableTraitWeights.Value;

        foreach (var kvp in TraitWeights)
        {
            if (kvp.Value.Description?.Tags?.Length > 0 && kvp.Value.Description.Tags[0] is ConfigurationManagerAttributes attrs)
            {
                var show = weightsEnabled;

                // Also check unique trait toggles
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

                attrs.Browsable = show;
            }
        }

        _instance?.StartCoroutine(RefreshConfigurationManager());
    }

    private static BaseUnityPlugin GetConfigurationManager()
    {
        return (from pluginInfo in BepInEx.Bootstrap.Chainloader.PluginInfos.Values where pluginInfo.Metadata.GUID == "com.bepis.bepinex.configurationmanager" select pluginInfo.Instance).FirstOrDefault();
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
        _isNothingNegativePresentCache ??= BepInEx.Bootstrap.Chainloader.PluginInfos.Any(plugin => plugin.Value.Instance.Info.Metadata.GUID.Equals("NothingNegative", StringComparison.OrdinalIgnoreCase));
        return _isNothingNegativePresentCache.Value;
    }

    internal static void L(string message)
    {
        Log.LogInfo(message);
    }
}