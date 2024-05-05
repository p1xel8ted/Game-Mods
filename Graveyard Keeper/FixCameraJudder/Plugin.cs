using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FixCameraJudder;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.fixcamerajudder";
    private const string PluginName = "Fix Camera Judder";
    private const string PluginVer = "0.1.0";

    private static ManualLogSource Log { get; set; }
    private static Harmony Harmony { get; set; }

    private static ConfigEntry<bool> ModEnabled { get; set; }
    private static ConfigEntry<int> FixedStatusUpdate { get; set; }

    private void Awake()
    {
        Log = Logger;
        Log.LogWarning($"Current FixedStatusUpdate Interval: {Time.fixedDeltaTime}");
        Harmony = new Harmony(PluginGuid);
        ModEnabled = Config.Bind("1. General", "Enabled", true, $"Toggle {PluginName}");
        ModEnabled.SettingChanged += ApplyPatches;
        
        FixedStatusUpdate = Config.Bind("2. Camera Update", "Interval", Screen.currentResolution.refreshRate, $"Set the interval for how often the camera updates. Be aware that physics calculations are usually tied to this update interval, so setting it too high (or low) may cause issues. Default is your screen refresh rate.");
        FixedStatusUpdate.SettingChanged += (sender, args) =>
        {
            Time.fixedDeltaTime = 1f / FixedStatusUpdate.Value;
            Log.LogWarning($"Current FixedStatusUpdate Interval: {Time.fixedDeltaTime}");
        };
        ApplyPatches(this,null);
    }

    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Actions.GameStartedPlaying += GameStartedPlaying;
        }
        else
        {
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            Actions.GameStartedPlaying -= GameStartedPlaying;
        }
    }

    private static void GameStartedPlaying()
    {
        Log.LogWarning($"GameStartedPlaying Called. Original FixedStatusUpdate: {Time.fixedDeltaTime}");
        Time.fixedDeltaTime = 1f / FixedStatusUpdate.Value;
        Log.LogWarning($"New FixedStatusUpdate Interval: {Time.fixedDeltaTime}");
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameStartedPlaying();
    }
}