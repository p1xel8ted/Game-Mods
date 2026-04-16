namespace WheresMaVeggies;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmaveggies";
    private const string PluginName = "Where's Ma' Veggies!";
    private const string PluginVer = "0.1.9";

    private const string AdvancedSection = "── 1. Advanced ──";
    private const string HarvestSection  = "── 2. Harvest ──";

    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static bool DebugEnabled;
    internal static bool DebugDialogShown;

    internal static ConfigEntry<bool> RequireFarmerPerk { get; private set; }

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

        Lang.Init(Assembly.GetExecutingAssembly(), Log);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
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
        GUIElements.me.dialog.OpenOK(PluginName, null, Lang.Get("DebugWarning"), true, string.Empty);
    }
}
