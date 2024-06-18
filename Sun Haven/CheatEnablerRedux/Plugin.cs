namespace CheatEnablerRedux;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.cheatenablerredux";
    private const string PluginName = "Cheat Enabler Redux";
    private const string PluginVersion = "0.1.0";
    private static ManualLogSource LOG { get; set; }

    private void Awake()
    {
        LOG = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        LOG.LogInfo($"Plugin {PluginName} is loaded!");

    }
}