namespace ThoughtfulReminders;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.thoughtfulreminders";
    private const string PluginName = "Thoughtful Reminders";
    private const string PluginVer = "2.2.10";

    internal static ConfigEntry<bool> SpeechBubblesConfig { get; private set; }
    internal static ConfigEntry<bool> DaysOnlyConfig { get; private set; }
    internal static ConfigEntry<bool> EnableEventMessages { get; private set; }

    internal static ManualLogSource LOG { get; private set; }

    private void Awake()
    {
        LOG = Logger;
        SpeechBubblesConfig = Config.Bind("01. General", "Speech Bubbles", true, new ConfigDescription("Enable or disable speech bubbles", null, new ConfigurationManagerAttributes {Order = 3}));
        DaysOnlyConfig = Config.Bind("01. General", "Days Only", false, new ConfigDescription("Enable or disable days only mode", null, new ConfigurationManagerAttributes {Order = 2}));
        EnableEventMessages = Config.Bind("01. General", "Event Messages", true, new ConfigDescription("Show event-specific messages (e.g. 'could drop by the tavern'). When disabled, only the day name is shown.", null, new ConfigurationManagerAttributes {Order = 1}));
        Lang.Init(Assembly.GetExecutingAssembly(), LOG);
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
    }
}
