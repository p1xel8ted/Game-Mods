using System;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GYKHelper;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShowMeMoar;

[BepInPlugin(PluginGuid, PluginName, PluginVer)]
[BepInDependency("p1xel8ted.gyk.gykhelper", "3.0.2")]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "p1xel8ted.gyk.showmemoar";
    private const string PluginName = "Show Me Moar!";
    private const string PluginVer = "0.1.5";
    private static Harmony Harmony { get; set; }
    private static ConfigEntry<bool> ModEnabled { get; set; }
    
    internal static ConfigEntry<bool> Ultrawide { get; private set; }
    private static ConfigEntry<KeyboardShortcut> ZoomIn { get; set; }
    private static ConfigEntry<KeyboardShortcut> ZoomOut { get; set; }

    internal static ConfigEntry<float> HudScale { get; private set; }
    internal static ConfigEntry<float> HorizontalHudPosition { get; private set; }
    internal static ConfigEntry<float> VerticalHudPosition { get; private set; }
    private static ConfigEntry<float> Zoom { get; set; }
    private static ConfigEntry<float> CraftIconAboveStations { get; set; }

    private static GameObject Icons { get; set; }
    internal static ManualLogSource Log { get; private set; }

    private void Awake()
    {
        Log = Logger;
        SceneManager.sceneLoaded += (_, _) =>
        {
            var smallFont = GameObject.Find("UI Root/Label size calculators/small_font");
            if (smallFont == null) return;
            Log.LogInfo("Hiding small font. We can't disable it as it breaks UI.");
            smallFont.ChangeColor(new Color(0,0,0,0), 0f);
        };
        Actions.GameStartedPlaying += OnGameStartedPlaying;
        Harmony = new Harmony(PluginGuid);
        InitConfiguration();
        ApplyPatches(this, null);
    }

    private static void OnGameStartedPlaying()
    {
        Patches.ScreenSize = GameObject.Find("UI Root/Screen size panel").transform;
        if (Patches.ScreenSize == null)
        {
            Log.LogError("Screen size panel not found!");
        }
    }

    private void InitConfiguration()
    {
        var defaultZoom = Screen.currentResolution.height / 2f;
        var min = 0 - defaultZoom;

        ModEnabled = Config.Bind("1. General", "Enabled", true, new ConfigDescription($"Enable or disable {PluginName}", null, new ConfigurationManagerAttributes {Order = 8}));
        ModEnabled.SettingChanged += ApplyPatches;

        Ultrawide = Config.Bind("2. Ultrawide", "Ultrawide", false, new ConfigDescription("Enable or disable ultrawide support. You must restart the game after changing this setting.", null, new ConfigurationManagerAttributes {Order = 7}));
        
        CraftIconAboveStations = Config.Bind("3. Scale", "Interaction Bubble Scale", 1f, new ConfigDescription("Changes the scale of the icons that appear above crafting stations and interaction icons.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 6}));
        CraftIconAboveStations.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            UpdateCraftIconScale(CraftIconAboveStations.Value);
        };
        HudScale = Config.Bind("3. Scale", "HUD Scale", 1f, new ConfigDescription("Changes the scale of the HUD.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes {Order = 5}));
        HudScale.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.HUD != null) Patches.HUD.transform.localScale = new Vector3(HudScale.Value, HudScale.Value, 1);
        };
        
        HorizontalHudPosition = Config.Bind("4. Positions", "Horizontal HUD Position", 1f, new ConfigDescription("Changes the horizontal position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 4}));
        HorizontalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };
        
        VerticalHudPosition = Config.Bind("4. Positions", "Vertical HUD Position", 1f, new ConfigDescription("Changes the vertical position of the HUD.", new AcceptableValueRange<float>(-5, 5), new ConfigurationManagerAttributes {Order = 3}));
        VerticalHudPosition.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            if (Patches.ScreenSize != null) Patches.ScreenSize.transform.localScale = new Vector3(HorizontalHudPosition.Value, VerticalHudPosition.Value, 1);
        };
        
        
        Zoom = Config.Bind("5. Zoom", "Zoom", 0f, new ConfigDescription("Zoom", new AcceptableValueRange<float>(min + 10, defaultZoom * 2), new ConfigurationManagerAttributes {Order = 2}));
        Zoom.SettingChanged += (_, _) =>
        {
            if (!MainGame.game_started) return;
            Camera.main!.orthographicSize = defaultZoom + Zoom.Value;
        };

        ZoomIn = Config.Bind("5. Zoom", "Zoom In", new KeyboardShortcut(KeyCode.KeypadPlus), new ConfigDescription("Zoom In", null, new ConfigurationManagerAttributes {Order = 1}));
        ZoomOut = Config.Bind("5. Zoom", "Zoom Out", new KeyboardShortcut(KeyCode.KeypadMinus), new ConfigDescription("Zoom Out", null, new ConfigurationManagerAttributes {Order = 0}));
    }



    private void Update()
    {
        if (!MainGame.game_started) return;

        if (ZoomIn.Value.IsPressed())
        {
            Zoom.Value -= 5f;
        }

        if (ZoomOut.Value.IsPressed())
        {
            Zoom.Value += 5f;
        }
    }

    private static void UpdateCraftIconScale(float scale)
    {
        Icons ??= GameObject.Find("UI Root/Interaction bubbles");
        if (Icons != null)
        {
            Icons.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    private static void GameStartedPlaying()
    {
        if (!MainGame.game_started) return;
        var setting = Zoom.Value;
        var defaultZoom = GameSettings.current_resolution.y / 2f;
        Camera.main!.orthographicSize = defaultZoom + setting;
    }

    private static void ApplyPatches(object sender, EventArgs eventArgs)
    {
        if (ModEnabled.Value)
        {
            Actions.GameStartedPlaying += GameStartedPlaying;
            Log.LogInfo($"Applying patches for {PluginName}");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            Actions.GameStartedPlaying -= GameStartedPlaying;
            Camera.main!.orthographicSize = Screen.currentResolution.height / 2f;
            Log.LogInfo($"Removing patches for {PluginName}");
            Harmony.UnpatchSelf();
        }
    }
}