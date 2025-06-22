namespace ModMenu;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.modmenu";
    private const string PluginName = "Mod Menu";
    private const string PluginVersion = "0.1.0";
    internal static ManualLogSource Log { get; set; }
    
    private void Awake()
    {
        Log = Logger;

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }
}