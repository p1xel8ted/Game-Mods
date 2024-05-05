using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Framework.FrameworkCore;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlasphemousOne;

[Harmony]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private static ManualLogSource Log { get; set; }
    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);
    private static int DisplayWidth { get; } = Display.main.systemWidth;
    private static int DisplayHeight { get; } = Display.main.systemHeight;

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += OnSceneLoaded;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), MyPluginInfo.PLUGIN_GUID);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FpsLimiter.SetTargetFrameRate(MaxRefresh);
        Application.targetFrameRate = MaxRefresh;
        Screen.SetResolution(DisplayWidth, DisplayHeight, true, MaxRefresh);
    }


    [HarmonyPostfix]
    [HarmonyPatch(typeof(FpsLimiter), nameof(FpsLimiter.Start))]
    private static void FpsLimiter_Start(ref FpsLimiter __instance)
    {
        __instance.TargetFrameRate = MaxRefresh;
        __instance.UpdateTargetDeltaTime();
        __instance.OldTime = Time.realtimeSinceStartup;
        FpsLimiter.SetTargetFrameRate(MaxRefresh);
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(FpsLimiter), nameof(FpsLimiter.SetTargetFrameRate))]
    private static void FpsLimiter_SetTargetFrameRate(ref int targetFrameRate)
    {
        targetFrameRate = MaxRefresh;
    }
}