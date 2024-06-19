namespace CheatEnabler;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.cheatenabler";
    private const string PluginName = "Cheat Enabler";
    private const string PluginVersion = "0.2.0";
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");

    }
}