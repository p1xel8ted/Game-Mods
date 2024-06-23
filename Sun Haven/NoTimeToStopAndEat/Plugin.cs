namespace NoTimeToStopAndEat;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.notimetoeat";
    private const string PluginName = "No Time To Stop & Eat!";
    private const string PluginVersion = "0.1.3";
    
    internal static ConfigEntry<bool> HideFoodItemWhenEating { get; private set; }

    private void Awake()
    {
        HideFoodItemWhenEating = Config.Bind("General", "Hide Food Item When Eating", true, "Hide the food item when eating.");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded!");
    }
}