using MonoMod.Utils;

namespace NoTimeToStopAndEat;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency("p1xel8ted.sunhaven.keepalive")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.notimetoeat";
    private const string PluginName = "No Time To Stop & Eat!";
    private const string PluginVersion = "0.1.7";
    
    internal static ConfigEntry<bool> HideFoodItemWhenEating { get; private set; }

    private void Awake()
    {
        HideFoodItemWhenEating = Config.Bind("01. General", "Hide Food Item When Eating", true, "Hide the food item when eating.");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {PlatformHelper.Current}.");
    }
    
    private void OnDestroy()
    {
        OnDisable();
    }
    
    private void OnDisable()
    {
        Logger.LogError($"Plugin {PluginName} was disabled/destroyed! Unless you are exiting the game, please install Keep Alive! - https://www.nexusmods.com/sunhaven/mods/31");
    }
}