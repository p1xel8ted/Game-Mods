using System.Linq;
using System.Reflection;
using AnodeHeart;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HavenPark;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.havenpark.tweaks";
    private const string PluginName = "Haven Park Tweaks";
    private const string PluginVersion = "0.1.0";
    private const string Intro = "Intro";
    private const string Menu = "Menu";
    private static ManualLogSource Log { get; set; }

    internal static CanvasScaler UiCanvasScaler { get; set; }

    internal static ConfigEntry<float> ConfigFoV { get; private set; }
    internal static ConfigEntry<float> ConfigScale { get; private set; }
    private static int MaxRefresh => Screen.resolutions.Max(a => a.refreshRate);

    private void Awake()
    {
        Log = Logger;
        ConfigFoV = Config.Bind("01. General", "Field of View", 50f,
            new ConfigDescription("Field of View",
                new AcceptableValueRange<float>(30f, 150f), new ConfigurationManagerAttributes {Order = 1}));
        ConfigFoV.SettingChanged += (_, _) => { Utils.UpdateCamera(); };
        ConfigScale = Config.Bind("01. General", "UI Scale", 1f,
            new ConfigDescription("Scale",
                new AcceptableValueRange<float>(0.5f, 2f), new ConfigurationManagerAttributes {Order = 2}));
        ConfigScale.SettingChanged += (_, _) => { Utils.UpdateScaler(UiCanvasScaler); };
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Logger.LogInfo($"Plugin {PluginName} is loaded!");
    }


    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Log.LogInfo($"Scene loaded: {arg0.name}");
        if (arg0.name.Equals(Intro))
        {
            Log.LogInfo($"Skipping intros.");
            SceneManager.LoadScene(Menu);
        }

        Log.LogInfo($"Current FixedDeltaTime: {Time.fixedDeltaTime}, Current RefreshRate: {Application.targetFrameRate}");
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, FullScreenMode.FullScreenWindow, MaxRefresh);
        Time.fixedDeltaTime = 1f / MaxRefresh;
        Application.targetFrameRate = MaxRefresh;
        Log.LogInfo($"New FixedDeltaTime: {Time.fixedDeltaTime}, New RefreshRate: {Application.targetFrameRate}");
        Utils.UpdateScaler(UiCanvasScaler);
        Utils.UpdateCamera();
    }
}