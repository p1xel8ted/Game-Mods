[assembly: MelonInfo(typeof(Mod), Mod.PluginName, Mod.PluginVersion, "p1xel8ted")]

namespace TavernKeeperTweaks;

public class Mod : MelonMod
{
    internal const string PluginName = "Tavern Keeper Tweaks";
    internal const string PluginVersion = "0.1.0";

    public override void OnInitializeMelon()
    {
        base.OnInitializeMelon();
        MelonLogger.Msg($"[{PluginName}] v{PluginVersion} initialized.");
        LogAndUpdateFixedDeltaTime();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LogAndUpdateFixedDeltaTime();
    }

    private static void LogAndUpdateFixedDeltaTime()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
        MelonLogger.Msg($"[{PluginName}] Current Fixed Delta Time in FPS: {1f / fixedDeltaTime}");
        var targetFps = Screen.currentResolution.refreshRate;
        MelonLogger.Msg($"[{PluginName}] Target FPS based on current resolution: {targetFps}");
        var targetFixedDeltaTime = 1f / targetFps;
        MelonLogger.Msg($"[{PluginName}] Target Fixed Delta Time: {targetFixedDeltaTime}");

        if (!Mathf.Approximately(fixedDeltaTime, targetFixedDeltaTime))
        {
            Time.fixedDeltaTime = targetFixedDeltaTime;
            MelonLogger.Msg($"[{PluginName}] Fixed Delta Time updated to: {Time.fixedDeltaTime} ({1f / Time.fixedDeltaTime} FPS)");
        }
    }
}