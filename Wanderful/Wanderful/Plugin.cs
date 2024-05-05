using System.Linq;

namespace Wanderful;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.wanderfuldemo.wanderfuldemo";
    private const string PluginName = "Wanderful Demo";
    internal const string PluginVersion = "0.1.0";

    internal static ManualLogSource Logger { get; private set; }


    private static int MaxRefresh => Screen.resolutions.Where(a => a.m_Width == DisplayWidth && a.height == DisplayHeight).Max(a => a.refreshRate);

    private static int DisplayWidth => Display.main.systemWidth;
    private static int DisplayHeight => Display.main.systemHeight;

    public override void Load()
    {
        Logger = Log;
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) OnSceneLoaded;
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        AddComponent<UnityEvents>();
        Utils.WriteLog($"Plugin {PluginGuid} is loaded!");
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Screen.SetResolution(DisplayWidth, DisplayHeight, true, MaxRefresh);
        Utils.UpdateScalers();
    }
}