using MonoMod.Utils;

namespace KeepAlive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.sunhaven.keepalive";
    private const string PluginName = "Keep Alive";
    private const string PluginVersion = "0.1.1";
    
    private static ManualLogSource Log { get; set; }

    internal static List<string> NoKillList = ["UniverseLibBehaviour", "UniverseLib", "UniverseLibBehaviour(Clone)", "UniverseLib(Clone)", "ExplorerBehaviour", "Explorer", "ExplorerBehaviour(Clone)", "Explorer(Clone)"];

    private void Awake()
    {
        Log = Logger;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Logger.LogInfo($"Plugin {PluginName} is loaded! Running game version {Application.version} on {PlatformHelper.Current}.");
        AddGameObjectToNoKillList("bepinex");
    }

    [UsedImplicitly]
    public static void AddGameObjectToNoKillList(string gameObjectName)
    {
        var callingMethod = new StackTrace().GetFrame(1).GetMethod();
        NoKillList.Add(gameObjectName);
        NoKillList = NoKillList.Distinct().ToList();
        Log.LogInfo($"Added '{gameObjectName}' to the NoKillList at the request of '{callingMethod.DeclaringType!.FullName}.{callingMethod.Name}'");
    }
}