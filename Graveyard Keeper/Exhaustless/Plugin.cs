namespace Exhaustless;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.exhaustless";
    private const string PluginName = "Exhaust-less!";
    private const string PluginVer = "3.4.8";

    internal static ConfigEntry<bool> MakeToolsLastLonger { get;private set; }
    internal static ConfigEntry<bool> SpendHalfGratitude { get; private set; }
    internal static ConfigEntry<bool> AutoEquipNewTool { get; private set; }
    internal static ConfigEntry<bool> SpeedUpSleep { get; private set; }
    internal static ConfigEntry<bool> AutoWakeFromMeditationWhenStatsFull { get; private set; }
    internal static ConfigEntry<bool> SpendHalfSanity { get; private set; }
    internal static ConfigEntry<bool> SpeedUpMeditation { get; private set; }
    internal static ConfigEntry<bool> SpendHalfEnergy { get; private set; }
    internal static ConfigEntry<bool> UnlimitedEnergy { get; private set; }
    internal static ConfigEntry<bool> UnlimitedGratitude { get; private set; }
    internal static ConfigEntry<bool> UnlimitedHealth { get; private set; }
    internal static ConfigEntry<bool> UnlimitedSanity { get; private set; }
    internal static ConfigEntry<int> EnergySpendBeforeSleepDebuff { get; private set; }
    private static ManualLogSource Log { get; set; }
    
    private void Awake()
    {
        Log = Logger;
        InitConfiguration();
        Actions.GameBalanceLoad += Patches.GameBalance_LoadGameBalance;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }

    private void InitConfiguration()
    {
        AutoEquipNewTool = Config.Bind("01. Tools", "Auto Equip New Tool", true, new ConfigDescription("Automatically equip a new tool if the current one breaks", null, new ConfigurationManagerAttributes {Order = 49}));
        MakeToolsLastLonger = Config.Bind("01. Tools", "Make Tools Last Longer", true, new ConfigDescription("Increase the durability of tools", null, new ConfigurationManagerAttributes {Order = 48}));

        AutoWakeFromMeditationWhenStatsFull = Config.Bind("02. Meditation", "Auto Wake From Meditation When Stats Full", true, new ConfigDescription("Automatically wake up when meditation is complete", null, new ConfigurationManagerAttributes {Order = 47}));
        SpeedUpMeditation = Config.Bind("02. Meditation", "Speed Up Meditation", true, new ConfigDescription("Reduce the time needed for meditation", null, new ConfigurationManagerAttributes {Order = 46}));

        EnergySpendBeforeSleepDebuff = Config.Bind("03. Sleep", "Energy Spend Before Sleep Debuff", 1200, new ConfigDescription("Set the total energy spent in a day required (game's default is 300) before sleep debuff is applied", new AcceptableValueRange<int>(350, 50000), new ConfigurationManagerAttributes {Order = 45}));
        SpeedUpSleep = Config.Bind("03. Sleep", "Speed Up Sleep", true, new ConfigDescription("Decrease the time needed for sleep", null, new ConfigurationManagerAttributes {Order = 44}));

        SpendHalfEnergy = Config.Bind("04. Gameplay", "Spend Half Energy", true, new ConfigDescription("Reduce energy consumption by half. Enabling this will disable Unlimited Energy.", null, new ConfigurationManagerAttributes {Order = 43}));
        SpendHalfEnergy.SettingChanged += (_, _) =>
        {
            if(SpendHalfEnergy.Value)
                UnlimitedEnergy.Value = false;
        };
        
        
        SpendHalfGratitude = Config.Bind("04. Gameplay", "Spend Half Gratitude", true, new ConfigDescription("Reduce gratitude consumption by half. Enabling this will disable Unlimited Gratitude.", null, new ConfigurationManagerAttributes {Order = 42}));
        SpendHalfGratitude.SettingChanged += (_, _) =>
        {
            if(SpendHalfGratitude.Value)
                UnlimitedGratitude.Value = false;
        };
        
        SpendHalfSanity = Config.Bind("04. Gameplay", "Spend Half Sanity", true, new ConfigDescription("Reduce sanity consumption by half. Enabling this will disable Unlimited Sanity.", null, new ConfigurationManagerAttributes {Order = 41}));
        SpendHalfSanity.SettingChanged += (_, _) =>
        {
            if(SpendHalfSanity.Value)
                UnlimitedSanity.Value = false;
        };
        
        UnlimitedEnergy = Config.Bind("05. Unlimited Stats", "Unlimited Energy", false, new ConfigDescription("Unlimited energy. Enabling this will disable Spend Half Energy.", null, new ConfigurationManagerAttributes { Order = 40 }));
        UnlimitedEnergy.SettingChanged += (_, _) =>
        {
            if(UnlimitedEnergy.Value)
                SpendHalfEnergy.Value = false;
        };
        UnlimitedGratitude = Config.Bind("05. Unlimited Stats", "Unlimited Gratitude", false, new ConfigDescription("Unlimited gratitude. Enabling this will disable Spend Half Gratitude.", null, new ConfigurationManagerAttributes { Order = 39 }));
        UnlimitedGratitude.SettingChanged += (_, _) =>
        {
            if(UnlimitedGratitude.Value)
                SpendHalfGratitude.Value = false;
        }; 
        UnlimitedSanity = Config.Bind("05. Unlimited Stats", "Unlimited Sanity", false, new ConfigDescription("Unlimited sanity. This overrides Spend Half Sanity. Enabling this will disable Spend Half Sanity.", null, new ConfigurationManagerAttributes { Order = 38 }));
        UnlimitedSanity.SettingChanged += (_, _) =>
        {
            if(UnlimitedSanity.Value)
                SpendHalfSanity.Value = false;
        };
        
        UnlimitedHealth = Config.Bind("05. Unlimited Stats", "Unlimited Health", false, new ConfigDescription("Unlimited health.", null, new ConfigurationManagerAttributes { Order = 37 }));
    }
    
}