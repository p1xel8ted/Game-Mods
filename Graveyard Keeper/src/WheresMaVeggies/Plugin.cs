namespace WheresMaVeggies;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private const string AdvancedSection = "── 1. Advanced ──";
    private const string HarvestSection  = "── 2. Harvest ──";
    private const string UpdatesSection  = "── 3. Updates ──";

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> RequireFarmerPerk { get; private set; }
    internal static ConfigEntry<bool> CheckForUpdates { get; private set; }

    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        LogHelper.Log = Logger;

        Debug = Config.Bind(AdvancedSection, "Debug Logging", false,
            new ConfigDescription("Write detailed diagnostic info to the BepInEx log while you play. Turn this on before reporting a bug so the log has the context I need to help.", null,
                new ConfigurationManagerAttributes {Order = 100}));
        DebugEnabled = Debug.Value;
        Debug.SettingChanged += (_, _) => DebugEnabled = Debug.Value;

        RequireFarmerPerk = Config.Bind(HarvestSection, "Require Farmer Perk", true,
            new ConfigDescription("On: nearby garden plots only mass-harvest after you've unlocked the Farmer perk in the tech tree. This matches the mod's original behaviour. Off: every harvest immediately reaps neighbouring plots of the same crop, even before the perk is unlocked — useful if you want the convenience from day one.", null,
                new ConfigurationManagerAttributes {Order = 100}));

        CheckForUpdates = Config.Bind(UpdatesSection, "Check for Updates", true,
            new ConfigDescription(
                "Show a notice on the main menu when a newer version of this mod is available on NexusMods. Click the notice to open the mod's page.",
                null,
                new ConfigurationManagerAttributes {Order = 100}));

        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        UpdateChecker.Register(Info, CheckForUpdates);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    internal static void WriteLog(string message, bool error = false)
    {
        if (error)
        {
            LogHelper.Error(message);
        }
        else
        {
            LogHelper.Info(message);
        }
    }

    internal static void ShowDebugWarningOnce()
    {
        if (!DebugEnabled || DebugDialogShown) return;
        DebugDialogShown = true;
        Lang.Reload();
        GUIElements.me.dialog.OpenOK(MyPluginInfo.PLUGIN_NAME, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
