namespace TraitControl;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("com.bepis.bepinex.configurationmanager", "18.4.1")]
[BepInIncompatibility("NothingNegative")]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.traitcontrol";
    internal const string PluginName = "Trait Control";
    private const string PluginVer = "0.1.0";

    private const string TraitReplacementSection = "01. Trait Replacement";
    private const string TraitWeightsSection = "02. Trait Weights";

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
            new ConfigDescription("Replace negative traits with positive ones based on the configuration below.", null,
                new ConfigurationManagerAttributes { Order = 10 }));
        NoNegativeTraits.SettingChanged += (_, _) => UpdateNoNegativeTraits();

        UseUnlockedTraitsOnly = ConfigInstance.Bind(TraitReplacementSection, "Use Unlocked Traits Only", true,
            new ConfigDescription("Only use unlocked traits when replacing negative traits.", null,
                new ConfigurationManagerAttributes { Order = 9 }));
        UseUnlockedTraitsOnly.SettingChanged += (_, _) => Patches.NoNegativeTraits.GenerateAvailableTraits();

        IncludeImmortal = ConfigInstance.Bind(TraitReplacementSection, "Include Immortal", false,
            new ConfigDescription("Include the Immortal trait when replacing negative traits.", null,
                new ConfigurationManagerAttributes { Order = 8 }));
        IncludeImmortal.SettingChanged += (_, _) => Patches.NoNegativeTraits.GenerateAvailableTraits();

        IncludeDisciple = ConfigInstance.Bind(TraitReplacementSection, "Include Disciple", false,
            new ConfigDescription("Include the Disciple trait when replacing negative traits.", null,
                new ConfigurationManagerAttributes { Order = 7 }));
        IncludeDisciple.SettingChanged += (_, _) => Patches.NoNegativeTraits.GenerateAvailableTraits();

        ShowNotificationsWhenRemovingTraits = ConfigInstance.Bind(TraitReplacementSection, "Show Notifications When Removing Traits", false,
            new ConfigDescription("Show notifications when removing negative traits.", null,
                new ConfigurationManagerAttributes { Order = 6 }));

        ShowNotificationsWhenAddingTraits = ConfigInstance.Bind(TraitReplacementSection, "Show Notifications When Adding Traits", false,
            new ConfigDescription("Show notifications when adding positive traits.", null,
                new ConfigurationManagerAttributes { Order = 5 }));

        // Trait Weights - 02
        EnableTraitWeights = ConfigInstance.Bind(TraitWeightsSection, "Enable Trait Weights", false,
            new ConfigDescription("Enable weighted random selection for starting traits. When enabled, you can configure how often each trait appears below. Set a weight to 0 to disable that trait entirely.", null,
                new ConfigurationManagerAttributes { Order = 1000 }));
        EnableTraitWeights.SettingChanged += (_, _) => UpdateTraitWeightVisibility();

        // Generate dynamic trait weight configs
        GenerateTraitWeightConfigs();

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Log.LogInfo($"{PluginName} loaded.");
    }

    private static void GenerateTraitWeightConfigs()
    {
        // Combine all starting trait lists to create weight configs for all possible traits
        var allStartingTraits = new HashSet<FollowerTrait.TraitType>();
        allStartingTraits.UnionWith(FollowerTrait.StartingTraits);
        allStartingTraits.UnionWith(FollowerTrait.RareStartingTraits);

        var order = allStartingTraits.Count;
        foreach (var trait in allStartingTraits.OrderBy(t => t.ToString()))
        {
            var isHidden = !EnableTraitWeights.Value;
            var weight = ConfigInstance.Bind(
                TraitWeightsSection,
                trait.ToString(),
                1.0f,
                new ConfigDescription(
                    $"Weight for {trait}. Higher = more likely. Set to 0 to disable. Default is 1.0.",
                    new AcceptableValueRange<float>(0f, 10f),
                    new ConfigurationManagerAttributes { Order = order--, Browsable = !isHidden }
                )
            );
            TraitWeights[trait] = weight;
        }
    }

    private static Plugin _instance;

    private static void UpdateTraitWeightVisibility()
    {
        var show = EnableTraitWeights.Value;

        foreach (var entry in TraitWeights.Values)
        {
            if (entry.Description?.Tags?.Length > 0 && entry.Description.Tags[0] is ConfigurationManagerAttributes attrs)
            {
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
        _isNothingNegativePresentCache ??= BepInEx.Bootstrap.Chainloader.PluginInfos.Any(
            plugin => plugin.Value.Instance.Info.Metadata.GUID.Equals("NothingNegative", StringComparison.OrdinalIgnoreCase));
        return _isNothingNegativePresentCache.Value;
    }

    internal static void L(string message)
    {
        Log.LogInfo(message);
    }
}
