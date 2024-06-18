using System.Collections.Generic;

namespace Fabledom;

[Harmony]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.fabledom.tweaks";
    private const string PluginName = "Fabledom Tweaks";
    private const string PluginVersion = "0.1.0";

    private static ConfigEntry<bool> RunInBackground { get; set; }
    internal static ConfigEntry<CanvasScaler.ScreenMatchMode> ScreenMatchMode { get; private set; }
    internal static ConfigEntry<CanvasScaler.ScaleMode> ScaleMode { get; private set; }
    internal static ConfigEntry<float> ScaleFactor { get; private set; }

    internal static List<CanvasScaler> CanvasScalers { get; } = [];

    private static ManualLogSource Log { get; set; }

    private static void UpdateScalers()
    {
        foreach (var scaler in CanvasScalers.Where(scaler => scaler))
        {
            scaler.uiScaleMode = ScaleMode.Value;
            scaler.screenMatchMode = ScreenMatchMode.Value;
            if (ScaleMode.Value is CanvasScaler.ScaleMode.ScaleWithScreenSize) continue;
            scaler.scaleFactor = ScaleFactor.Value;
            scaler.SetScaleFactor(ScaleFactor.Value);
        }
    }

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;

        ScaleMode = Config.Bind("01. Scalers", "Canvas Scaler Scale Mode", CanvasScaler.ScaleMode.ScaleWithScreenSize, new ConfigDescription("The scaling mode to use for the CanvasScaler component.", null, new ConfigurationManagerAttributes {Order = 95}));
        ScaleMode.SettingChanged += (_, _) =>
        {
            UpdateScalers();
        };
        
        ScreenMatchMode = Config.Bind("01. Scalers", "Canvas Scaler Screen Match Mode", CanvasScaler.ScreenMatchMode.Expand, new ConfigDescription("The screen match mode to use for the CanvasScaler component. Has no effect unless Scale Mode is set to ScaleWithScreenSize.", null, new ConfigurationManagerAttributes {Order = 94}));
        ScreenMatchMode.SettingChanged += (_, _) =>
        {
            UpdateScalers();
        };

        ScaleFactor = Config.Bind("01. Scalers", "Canvas Scaler Scale Factor", 1f, new ConfigDescription("The scaling factor to use for the CanvasScaler component. Has no effect when Scale Mode is set to ScaleWithScreenSize.", new AcceptableValueRange<float>(0.5f, 10f), new ConfigurationManagerAttributes {Order = 93}));
        ScaleFactor.SettingChanged += (_, _) =>
        {
            ScaleFactor.Value = Mathf.Round(ScaleFactor.Value * 4) / 4;
            UpdateScalers();
        };


        RunInBackground = Config.Bind("02. Misc", "Run In Background", true, new ConfigDescription("Allows the game to run even when not in focus.", null, new ConfigurationManagerAttributes {Order = 90}));
        RunInBackground.SettingChanged += (_, _) =>
        {
            Application.runInBackground = RunInBackground.Value;
        };
        

        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);
        Log.LogInfo($"Plugin {PluginName} is loaded!");
    }
    
    private static void SceneManagerOnSceneLoaded(Scene a, LoadSceneMode l)
    {
        Application.runInBackground = RunInBackground.Value;
        UpdateScalers();
    }



}