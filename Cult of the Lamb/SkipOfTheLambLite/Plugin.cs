namespace SkipOfTheLambLite;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInIncompatibility("p1xel8ted.cotl.skipofthelamb")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.cotl.skipofthelamblite";
    private const string PluginName = "Skip of the Lamb Lite";
    private const string PluginVer = "0.1.1";

    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"{PluginName} loaded.");
    }
}
