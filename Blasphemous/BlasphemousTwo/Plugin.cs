using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using TGK.Game.Systems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace BlasphemousTwo;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static ManualLogSource Logger { get; set; }
    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);
    private static int DisplayWidth { get; } = Display.main.systemWidth;
    private static int DisplayHeight { get; } = Display.main.systemHeight;
    
    public override void Load()
    {
        Logger = Log;
        SceneManager.sceneLoaded += (UnityAction<Scene, LoadSceneMode>) OnSceneLoaded;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
    
    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Time.fixedDeltaTime = 1f / MaxRefresh;
        Application.targetFrameRate = MaxRefresh;
        Screen.SetResolution(DisplayWidth, DisplayHeight, FullScreenMode.FullScreenWindow, MaxRefresh);
    }
}