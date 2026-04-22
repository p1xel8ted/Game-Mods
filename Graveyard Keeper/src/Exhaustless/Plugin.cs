namespace Exhaustless;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection       = "── Advanced ──";
    private const string ToolsSection          = "── Tools ──";
    private const string MeditationSection     = "── Meditation ──";
    private const string SleepSection          = "── Sleep ──";
    private const string GameplaySection       = "── Gameplay ──";
    private const string UnlimitedStatsSection = "── Unlimited Stats ──";
    private const string UpdatesSection        = "── Updates ──";

    private static readonly Dictionary<string, string> SectionRenames = new()
    {
        ["01. Tools"]            = ToolsSection,
        ["02. Meditation"]       = MeditationSection,
        ["03. Sleep"]            = SleepSection,
        ["04. Gameplay"]         = GameplaySection,
        ["05. Unlimited Stats"]  = UnlimitedStatsSection,
    };

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;

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
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }
    private static ManualLogSource Log { get; set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;
        MigrateRenamedSections();
        InitConfiguration();
        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        DebugWarningDialog.Register(MyPluginInfo.PLUGIN_NAME, () => DebugEnabled);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    // Rewrites legacy numbered section headers to the plain "── Name ──" style so existing
    // user values survive the rename. Idempotent.
    private void MigrateRenamedSections()
    {
        var path = Config.ConfigFilePath;
        if (!File.Exists(path)) return;

        string content;
        try { content = File.ReadAllText(path); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not read {path}: {ex.Message}"); return; }

        var renamed = 0;
        foreach (var kv in SectionRenames)
        {
            var oldHeader = $"[{kv.Key}]";
            var newHeader = $"[{kv.Value}]";
            if (!content.Contains(oldHeader)) continue;
            content = content.Replace(oldHeader, newHeader);
            renamed++;
        }
        if (renamed == 0) return;

        try { File.WriteAllText(path, content); }
        catch (Exception ex) { Log.LogWarning($"[Migration] Could not write {path}: {ex.Message}"); return; }

        Log.LogInfo($"[Migration] Renamed {renamed} legacy config section header(s) to the '── Name ──' style. Existing user values preserved.");
        Config.Reload();
    }

    private void InitConfiguration()
    {
        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write verbose tool, meditation, sleep and energy diagnostics to the BepInEx console. Leave off for normal play.", null,
                new ConfigurationManagerAttributes {Order = 50}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        AutoEquipNewTool = Config.Bind(ToolsSection, "Auto Equip New Tool", true, new ConfigDescription("Automatically equip a new tool if the current one breaks", null, new ConfigurationManagerAttributes {Order = 49}));
        MakeToolsLastLonger = Config.Bind(ToolsSection, "Make Tools Last Longer", true, new ConfigDescription("Increase the durability of tools", null, new ConfigurationManagerAttributes {Order = 48}));

        AutoWakeFromMeditationWhenStatsFull = Config.Bind(MeditationSection, "Auto Wake From Meditation When Stats Full", true, new ConfigDescription("Automatically wake up when meditation is complete", null, new ConfigurationManagerAttributes {Order = 47}));
        SpeedUpMeditation = Config.Bind(MeditationSection, "Speed Up Meditation", true, new ConfigDescription("Reduce the time needed for meditation", null, new ConfigurationManagerAttributes {Order = 46}));

        EnergySpendBeforeSleepDebuff = Config.Bind(SleepSection, "Energy Spend Before Sleep Debuff", 1200, new ConfigDescription("Set the total energy spent in a day required (game's default is 300) before sleep debuff is applied", new AcceptableValueRange<int>(350, 50000), new ConfigurationManagerAttributes {Order = 45}));
        SpeedUpSleep = Config.Bind(SleepSection, "Speed Up Sleep", true, new ConfigDescription("Decrease the time needed for sleep", null, new ConfigurationManagerAttributes {Order = 44}));

        SpendHalfEnergy = Config.Bind(GameplaySection, "Spend Half Energy", true, new ConfigDescription("Reduce energy consumption by half. Enabling this will disable Unlimited Energy.", null, new ConfigurationManagerAttributes {Order = 43}));
        SpendHalfEnergy.SettingChanged += (_, _) =>
        {
            if(SpendHalfEnergy.Value)
                UnlimitedEnergy.Value = false;
        };
        
        
        SpendHalfGratitude = Config.Bind(GameplaySection, "Spend Half Gratitude", true, new ConfigDescription("Reduce gratitude consumption by half. Enabling this will disable Unlimited Gratitude.", null, new ConfigurationManagerAttributes {Order = 42}));
        SpendHalfGratitude.SettingChanged += (_, _) =>
        {
            if(SpendHalfGratitude.Value)
                UnlimitedGratitude.Value = false;
        };
        
        SpendHalfSanity = Config.Bind(GameplaySection, "Spend Half Sanity", true, new ConfigDescription("Reduce sanity consumption by half. Enabling this will disable Unlimited Sanity.", null, new ConfigurationManagerAttributes {Order = 41}));
        SpendHalfSanity.SettingChanged += (_, _) =>
        {
            if(SpendHalfSanity.Value)
                UnlimitedSanity.Value = false;
        };
        
        UnlimitedEnergy = Config.Bind(UnlimitedStatsSection, "Unlimited Energy", false, new ConfigDescription("Unlimited energy. Enabling this will disable Spend Half Energy.", null, new ConfigurationManagerAttributes { Order = 40 }));
        UnlimitedEnergy.SettingChanged += (_, _) =>
        {
            if(UnlimitedEnergy.Value)
                SpendHalfEnergy.Value = false;
        };
        UnlimitedGratitude = Config.Bind(UnlimitedStatsSection, "Unlimited Gratitude", false, new ConfigDescription("Unlimited gratitude. Enabling this will disable Spend Half Gratitude.", null, new ConfigurationManagerAttributes { Order = 39 }));
        UnlimitedGratitude.SettingChanged += (_, _) =>
        {
            if(UnlimitedGratitude.Value)
                SpendHalfGratitude.Value = false;
        }; 
        UnlimitedSanity = Config.Bind(UnlimitedStatsSection, "Unlimited Sanity", false, new ConfigDescription("Unlimited sanity. This overrides Spend Half Sanity. Enabling this will disable Spend Half Sanity.", null, new ConfigurationManagerAttributes { Order = 38 }));
        UnlimitedSanity.SettingChanged += (_, _) =>
        {
            if(UnlimitedSanity.Value)
                SpendHalfSanity.Value = false;
        };
        
        UnlimitedHealth = Config.Bind(UnlimitedStatsSection, "Unlimited Health", false, new ConfigDescription("Unlimited health.", null, new ConfigurationManagerAttributes { Order = 37 }));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true, new ConfigDescription(
            "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
            null,
            new ConfigurationManagerAttributes { Order = 36 }));
    }

}