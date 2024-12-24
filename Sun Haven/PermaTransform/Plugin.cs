namespace PermaTransform;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.permatransform";
    private const string PluginName = "Perma Transform";
    private const string PluginVersion = "0.1.0";
    
    private static ManualLogSource Log;

   
    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {MonoMod.Utils.PlatformHelper.Current}.");
    }
}