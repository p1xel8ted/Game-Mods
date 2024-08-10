namespace WheresMaVeggies;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.1.0")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.wheresmaveggies";
    private const string PluginName = "Where's Ma' Veggies!";
    private const string PluginVer = "0.1.4";
    internal static ConfigEntry<bool> Debug { get; private set; }
    internal static ManualLogSource Log { get; private set; }
    
    private void Awake()
    {
        Log = Logger;
        
        Debug = Config.Bind("01. Advanced", "Debug Logging", false, new ConfigDescription("Enable or disable debug logging.", null, new ConfigurationManagerAttributes {IsAdvanced = true, Order = 1}));
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);   
    }

}