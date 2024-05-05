using System.Linq;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tyranny;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.tyranny.tweaks";
    private const string PluginName = "Tyranny Tweaks";
    private const string PluginVersion = "0.1.0";
    private static ManualLogSource Log { get; set; }

    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Logger.LogInfo($"Plugin {PluginName} is loaded!");
    }


    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Log.LogInfo($"Current FixedDeltaTime: {Time.fixedDeltaTime}, Current RefreshRate: {Application.targetFrameRate}");
        Screen.SetResolution(Display.displays[0].systemWidth, Display.displays[0].systemHeight, true, MaxRefresh);
        Time.fixedDeltaTime = 1f / MaxRefresh;
        Application.targetFrameRate = MaxRefresh;
        Log.LogInfo($"New FixedDeltaTime: {Time.fixedDeltaTime}, New RefreshRate: {Application.targetFrameRate}");
    }
}