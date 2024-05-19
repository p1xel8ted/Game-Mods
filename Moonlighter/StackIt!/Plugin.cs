namespace StackIt;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.moonlighter.stackit";
    private const string PluginName = "StackIt!";
    private const string PluginVersion = "0.1.3";
    
    public static ManualLogSource LOG { get; private set; }
    internal static ConfigEntry<bool> StackIt { get; private set; }
    internal static ConfigEntry<bool> CustomStackSizes { get; private set; }
    internal static ConfigEntry<int> MaxStackSize { get; private set; }
    internal static ConfigEntry<bool> IncludeUniqueItems { get; private set; }
    internal static ConfigEntry<bool> Debug { get; private set; }

    public void Awake()
    {
    
        LOG = Logger;
        Debug = Config.Bind("0. Debug", "Debug", false, new ConfigDescription("Enables debug logging.", null, new ConfigurationManagerAttributes {Order = 99}));

        StackIt = Config.Bind("1. Stack It!", "Stack It!", true, new ConfigDescription("Doubles the max stack size of eligible items.", null, new ConfigurationManagerAttributes {Order = 50}));
        StackIt.SettingChanged += (sender, args) =>
        {
            if (StackIt.Value)
            {
                CustomStackSizes.Value = false;
            }
            ItemPatches.ReProcessAll();
        };

        IncludeUniqueItems = Config.Bind("1. Stack It!", "Include Unique Items", false, new ConfigDescription("Includes unique items in the stack size modification.", null, new ConfigurationManagerAttributes {Order = 49}));
        IncludeUniqueItems.SettingChanged += (sender, args) =>
        {
            ItemPatches.ReProcessAll();
        };
            
        CustomStackSizes = Config.Bind("2. Custom", "Custom Stack Size", false, new ConfigDescription("Allows you to set custom stack sizes for eligible items.", null, new ConfigurationManagerAttributes {Order = 98}));
        CustomStackSizes.SettingChanged += (sender, args) =>
        {
            if (CustomStackSizes.Value)
            {
                StackIt.Value = false;
            }
            ItemPatches.ReProcessAll();
        };

        MaxStackSize = Config.Bind("2. Custom", "Max Stack Size", 999, new ConfigDescription("The maximum stack size for eligible items.", new AcceptableValueRange<int>(1, 999), new ConfigurationManagerAttributes {Order = 97}));

        SceneManager.sceneLoaded += (_, _) =>
        {
            ItemPatches.ReProcessAll();
        };
            
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");
    }
}