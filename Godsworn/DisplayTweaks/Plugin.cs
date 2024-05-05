using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DisplayTweaks;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    private const string PluginGuid = "p1xel8ted.godsworn.displaytweaks";
    private const string PluginName = "Display Tweaks";
    private const string PluginVersion = "0.1.0";
    private static ConfigEntry<int> DisplayToUse { get; set; }
    private static ManualLogSource Logger { get; set; }
    private static int MainWidth => Display.displays[DisplayToUse.Value].systemWidth;
    private static int MainHeight => Display.displays[DisplayToUse.Value].systemHeight;
    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);

    public override void Load()
    {
        Logger = Log;
        
        DisplayToUse = Config.Bind("01. Display", "Display To Use", 0, new ConfigDescription("Display to use", new AcceptableValueList<int>(Display.displays.Select((_, i) => i).ToArray())));

        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) SceneLoaded;
        //
        // Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

        Logger.LogInfo($"Plugin {PluginGuid} is loaded!");
    }

    private static void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Logger.LogInfo($"Scene loaded: {scene.name}");
        Display.displays[DisplayToUse.Value].Activate();
        Screen.SetResolution(MainWidth, MainHeight, FullScreenMode.FullScreenWindow, MaxRefresh);
        Time.fixedDeltaTime = 1 / (float) MaxRefresh;
        Application.targetFrameRate = MaxRefresh;
        QualitySettings.antiAliasing = 8;
        QualitySettings.realtimeReflectionProbes = true;
        QualitySettings.softParticles = true;
        QualitySettings.particleRaycastBudget = 4096;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.lodBias = 10.0f;
        QualitySettings.maximumLODLevel = 0;
    }

}